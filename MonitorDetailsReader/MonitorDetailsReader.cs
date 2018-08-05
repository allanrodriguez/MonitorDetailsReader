using MDReader.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;

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

            NativeMethods.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, MonitorEnum, IntPtr.Zero);
            GetDisplayDevicesForMonitors();
            GetSizeForDisplayDevices();

            return new ReadOnlyCollection<IMonitorDetails>(_monitors);
        }

        #region Helper methods
        bool MonitorEnum(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData)
        {
            var monitorInfoEx = new MONITORINFOEX { Size = (uint)Marshal.SizeOf(typeof(MONITORINFOEX)) };

            bool success = NativeMethods.GetMonitorInfo(hMonitor, ref monitorInfoEx);

            _monitors.Add(new MonitorDetails
            {
                IsPrimaryMonitor = monitorInfoEx.Flags == NativeConstants.MONITORINFOF_PRIMARY,
                Handle = hMonitor,
                DisplayAdapter = new DisplayDevice(monitorInfoEx.Device),
                MonitorCoordinates = GetRectangleFromRECT(monitorInfoEx.Monitor),
                WorkAreaCoordinates = GetRectangleFromRECT(monitorInfoEx.Work)
            });
            
            return success;
        }

        void GetDisplayAdapterInfoForMonitor(MonitorDetails monitorDetails)
        {
            var devMode = new DEVMODE { Size = (ushort)Marshal.SizeOf(typeof(DEVMODE)) };

            if (NativeMethods.EnumDisplaySettings(monitorDetails.DisplayAdapter.Name, NativeConstants.ENUM_CURRENT_SETTINGS, ref devMode))
            {
                monitorDetails.Frequency = Convert.ToInt32(devMode.DisplayFrequency);
                monitorDetails.Resolution = new Rectangle
                {
                    Width = Convert.ToInt32(devMode.PelsWidth),
                    Height = Convert.ToInt32(devMode.PelsHeight)
                };

                // DEVMODE.LogPixels was returning the same density for all monitors, so some division is used
                // to get the scaling factor and DPI.
                monitorDetails.ScalingFactor = Convert.ToSingle(monitorDetails.Resolution.Width) / monitorDetails.MonitorCoordinates.Width;
                monitorDetails.Dpi = Convert.ToInt32(96f * monitorDetails.ScalingFactor);
            }
        }

        IDisplayDevice GetDisplayDeviceFromDISPLAY_DEVICE(DISPLAY_DEVICE displayDevice)
        {
            return new DisplayDevice(displayDevice.DeviceId, 
                                     displayDevice.DeviceKey,
                                     displayDevice.DeviceName,
                                     displayDevice.StateFlags,
                                     displayDevice.DeviceString);
        }

        [EnvironmentPermission(SecurityAction.Demand)]
        void GetDisplayDevicesForMonitors()
        {
            var displayAdapter = new DISPLAY_DEVICE { Size = (uint)Marshal.SizeOf(typeof(DISPLAY_DEVICE)) };
            uint devId = 0;

            while (NativeMethods.EnumDisplayDevices(null, devId, ref displayAdapter, 0))
            {
                ++devId;

                if (_monitors.Find(m => m.DisplayAdapter.Name == displayAdapter.DeviceName) is MonitorDetails monitorDetails)
                {
                    var displayMonitor = new DISPLAY_DEVICE { Size = (uint)Marshal.SizeOf(typeof(DISPLAY_DEVICE)) };
                    uint monId = 0;

                    monitorDetails.DisplayAdapter = GetDisplayDeviceFromDISPLAY_DEVICE(displayAdapter);

                    while (NativeMethods.EnumDisplayDevices(displayAdapter.DeviceName, monId, ref displayMonitor, 0))
                    {
                        ++monId;
                        monitorDetails.Name = displayMonitor.DeviceName;
                        monitorDetails.String = displayMonitor.DeviceString;
                        monitorDetails.StateFlags = (DeviceStateFlags)displayMonitor.StateFlags;
                        monitorDetails.Id = displayMonitor.DeviceId;
                        monitorDetails.Key = displayMonitor.DeviceKey;
                    }

                    GetDisplayAdapterInfoForMonitor(monitorDetails);
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

            if (NativeMethods.RegQueryValueEx(edidRegKey, "EDID", IntPtr.Zero, IntPtr.Zero, edidData, ref edidSize) == NativeConstants.ERROR_SUCCESS)
            {
                return new ReadOnlyCollection<byte>(edidData);
            }

            return null;
        }

        RectangleF GetMonitorSizeFromEdid(IList<byte> edidData)
        {
            return new RectangleF(0f,
                                  0f,
                                  // Dividing by 10 because the width and height are originally in millimeters.
                                  (((edidData[68] & 0xf0) << 4) + edidData[66]) / 10f,
                                  (((edidData[68] & 0x0f) << 8) + edidData[67]) / 10f);
        }

        Rectangle GetRectangleFromRECT(RECT rect)
        {
            return new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
        }

        [EnvironmentPermission(SecurityAction.Demand)]
        void GetSizeForDisplayDevices()
        {
            IntPtr devInfo = NativeMethods.SetupDiGetClassDevsEx(new[] { NativeConstants.MonitorClassGuid2 }, 
                                                                 null, 
                                                                 IntPtr.Zero, 
                                                                 NativeConstants.DIGCF_PRESENT | NativeConstants.DIGCF_PROFILE,
                                                                 IntPtr.Zero,
                                                                 null,
                                                                 IntPtr.Zero);

            if (devInfo == null)
            {
                return;
            }

            for (uint i = 0; Marshal.GetLastWin32Error() != NativeConstants.ERROR_NO_MORE_ITEMS; ++i)
            {
                var devInfoData = new SP_DEVINFO_DATA { Size = (uint)Marshal.SizeOf(typeof(SP_DEVINFO_DATA)) };

                if (NativeMethods.SetupDiEnumDeviceInfo(devInfo, i, ref devInfoData))
                {
                    char[] instance = new char[NativeConstants.MAX_PATH];
                    NativeMethods.SetupDiGetDeviceInstanceId(devInfo, ref devInfoData, instance, NativeConstants.MAX_PATH, IntPtr.Zero);

                    bool DoMonitorDetailsExistForDeviceId(IMonitorDetails s)
                    {
                        string deviceId = Get2ndSlashBlock(s.Id);
                        string instanceString = new string(instance);

                        return instanceString.Contains(deviceId);
                    }

                    if (_monitors.Find(DoMonitorDetailsExistForDeviceId) is MonitorDetails monitorDetails)
                    {
                        IntPtr edidRegKey = NativeMethods.SetupDiOpenDevRegKey(devInfo, ref devInfoData, NativeConstants.DICS_FLAG_GLOBAL, 0, NativeConstants.DIREG_DEV, NativeConstants.KEY_READ);

                        if (edidRegKey != NativeConstants.INVALID_HANDLE_VALUE)
                        {
                            monitorDetails.Edid = GetMonitorEdidFromRegistry(edidRegKey);
                            monitorDetails.Dimensions = GetMonitorSizeFromEdid(monitorDetails.Edid);
                        }

                        NativeMethods.RegCloseKey(edidRegKey);
                    }
                }
            }

            NativeMethods.SetupDiDestroyDeviceInfoList(devInfo);
        }
        #endregion
    }
}
