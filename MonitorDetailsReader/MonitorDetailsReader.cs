using MDReader.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace MDReader
{
    /// <summary>
    ///     Retrieves current settings and useful information about the monitors
    ///     connected to this computer.
    /// </summary>
    public class MonitorDetailsReader : IMonitorDetailsReader
    {
        List<IMonitorDetails> _monitors;

        public MonitorDetailsReader()
        {
            _monitors = new List<IMonitorDetails>();
        }

        /// <summary>
        ///     Creates a read-only collection of <see cref="IMonitorDetails"/> objects
        ///     containing information about all monitors connected to this computer.
        /// </summary>
        /// <returns>
        ///     A read-only collection of <see cref="IMonitorDetails"/> objects containing
        ///     information about all monitors connected to this computer.
        /// </returns>
        public ReadOnlyCollection<IMonitorDetails> GetMonitorDetails()
        {
            _monitors.Clear();

            NativeHelpers.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, MonitorEnum, IntPtr.Zero);
            GetDisplayDevicesForMonitors();
            GetSizeForDisplayDevices();

            return new ReadOnlyCollection<IMonitorDetails>(_monitors);
        }

        #region Helper methods
        bool MonitorEnum(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData)
        {
            var monitorInfoEx = new MONITORINFOEX { Size = (uint)Marshal.SizeOf(typeof(MONITORINFOEX)) };

            bool success = NativeHelpers.GetMonitorInfo(hMonitor, ref monitorInfoEx);

            _monitors.Add(new MonitorDetails
            {
                IsPrimaryMonitor = monitorInfoEx.Flags == NativeHelpers.MONITORINFOF_PRIMARY,
                Handle = hMonitor,
                Name = monitorInfoEx.Device,
                HeightScaledPixels = monitorInfoEx.Monitor.Bottom - monitorInfoEx.Monitor.Top,
                WidthScaledPixels = monitorInfoEx.Monitor.Right - monitorInfoEx.Monitor.Left,
                MonitorCoordinates = monitorInfoEx.Monitor,
                WorkAreaHeight = monitorInfoEx.Work.Bottom - monitorInfoEx.Work.Top,
                WorkAreaWidth = monitorInfoEx.Work.Right - monitorInfoEx.Work.Left,
                WorkAreaCoordinates = monitorInfoEx.Work
            });

            return success;
        }

        void GetDisplayDevicesForMonitors()
        {
            var dd = new DISPLAY_DEVICE { Size = (uint)Marshal.SizeOf(typeof(DISPLAY_DEVICE)) };

            uint devIdx = 0;

            while (NativeHelpers.EnumDisplayDevices(null, devIdx, ref dd, 0))
            {
                ++devIdx;

                if (_monitors.Find(m => m.Name == dd.DeviceName) is MonitorDetails monitorDetails)
                {
                    var ddMon = new DISPLAY_DEVICE { Size = (uint)Marshal.SizeOf(typeof(DISPLAY_DEVICE)) };
                    uint monIdx = 0;

                    while (NativeHelpers.EnumDisplayDevices(dd.DeviceName, monIdx, ref ddMon, 0))
                    {
                        ++monIdx;
                        monitorDetails.DisplayDeviceName = ddMon.DeviceName;
                        monitorDetails.DisplayDeviceString = ddMon.DeviceString;
                        monitorDetails.DisplayDeviceStateFlags = ddMon.StateFlags;
                        monitorDetails.DisplayDeviceId = ddMon.DeviceId;
                        monitorDetails.DisplayDeviceKey = ddMon.DeviceKey;
                    }

                    var devMode = new DEVMODE
                    {
                        Size = (ushort)Marshal.SizeOf(typeof(DEVMODE)),
                        DriverExtra = 2048,
                        DeviceName = new Char32Array(),
                        FormName = new Char32Array()
                    };

                    if (NativeHelpers.EnumDisplaySettings(dd.DeviceName, NativeHelpers.ENUM_CURRENT_SETTINGS, ref devMode))
                    {
                        monitorDetails.DisplayAdapterName = dd.DeviceName;
                        monitorDetails.DisplayAdapterString = dd.DeviceString;
                        monitorDetails.DisplayAdapterStateFlags = dd.StateFlags;
                        monitorDetails.DisplayAdapterId = dd.DeviceId;
                        monitorDetails.DisplayAdapterKey = dd.DeviceKey;
                        
                        monitorDetails.Frequency = (int)devMode.DisplayFrequency;
                        monitorDetails.WidthPhysicalPixels = (int)devMode.PelsWidth;
                        monitorDetails.HeightPhysicalPixels = (int)devMode.PelsHeight;

                        // DEVMODE.LogPixels was returning the same density for all monitors, so some division is used
                        // to get the scaling factor and DPI.
                        monitorDetails.ScalingFactor = Convert.ToDouble(monitorDetails.WidthPhysicalPixels) / monitorDetails.WidthScaledPixels;
                        monitorDetails.Dpi = Convert.ToInt32(96.0 * monitorDetails.ScalingFactor);
                    }
                }
            }
        }

        string Get2ndSlashBlock(string sIn)
        {
            int firstSlash = sIn.IndexOf('\\');
            string sOut = sIn.Substring(firstSlash + 1);

            firstSlash = sOut.IndexOf('\\');

            return sOut.Substring(0, firstSlash);
        }

        ReadOnlyCollection<byte> GetMonitorEdidFromRegistry(IntPtr edidRegKey)
        {
            uint edidSize = 128;
            byte[] edidData = new byte[edidSize];

            if (NativeHelpers.RegQueryValueEx(edidRegKey, "EDID", 0, 0, edidData, ref edidSize) == NativeHelpers.ERROR_SUCCESS)
            {
                return new ReadOnlyCollection<byte>(edidData);
            }

            return null;
        }

        double[] GetMonitorSizeFromEdid(IList<byte> edidData)
        {
            return new double[] 
            {
                ((edidData[68] & 0xf0) << 4) + edidData[66],
                ((edidData[68] & 0x0f) << 8) + edidData[67]
            };
        }

        void GetSizeForDisplayDevices()
        {
            IntPtr devInfo = NativeHelpers.SetupDiGetClassDevsEx(new[] { NativeHelpers.MonitorClassGuid }, null, IntPtr.Zero, NativeHelpers.DIGCF_PRESENT | NativeHelpers.DIGCF_PROFILE, IntPtr.Zero, null, IntPtr.Zero);

            if (devInfo == null)
            {
                return;
            }

            for (uint i = 0; Marshal.GetLastWin32Error() != NativeHelpers.ERROR_NO_MORE_ITEMS; ++i)
            {
                var devInfoData = new SP_DEVINFO_DATA { Size = (uint)Marshal.SizeOf(typeof(SP_DEVINFO_DATA)) };

                if (NativeHelpers.SetupDiEnumDeviceInfo(devInfo, i, ref devInfoData))
                {
                    char[] instance = new char[NativeHelpers.MAX_PATH];
                    NativeHelpers.SetupDiGetDeviceInstanceId(devInfo, ref devInfoData, instance, NativeHelpers.MAX_PATH, 0);

                    bool DoMonitorDetailsExistForDeviceId(IMonitorDetails s)
                    {
                        string deviceId = Get2ndSlashBlock(s.DisplayDeviceId);
                        string instanceString = new string(instance);

                        return instanceString.Contains(deviceId);
                    }

                    if (_monitors.Find(DoMonitorDetailsExistForDeviceId) is MonitorDetails monitorDetails)
                    {
                        IntPtr edidRegKey = NativeHelpers.SetupDiOpenDevRegKey(devInfo, ref devInfoData, NativeHelpers.DICS_FLAG_GLOBAL, 0, NativeHelpers.DIREG_DEV, NativeHelpers.KEY_READ);

                        if (edidRegKey != NativeHelpers.INVALID_HANDLE_VALUE)
                        {
                            monitorDetails.Edid = GetMonitorEdidFromRegistry(edidRegKey);

                            double[] widthHeight = GetMonitorSizeFromEdid(monitorDetails.Edid);

                            // Dividing by 10 because the width and height are originally in millimeters.
                            monitorDetails.WidthCentimeters = widthHeight[0] / 10.0;
                            monitorDetails.HeightCentimeters = widthHeight[1] / 10.0;
                        }

                        NativeHelpers.RegCloseKey(edidRegKey);
                    }
                }
            }

            NativeHelpers.SetupDiDestroyDeviceInfoList(devInfo);
        }
        #endregion
    }
}
