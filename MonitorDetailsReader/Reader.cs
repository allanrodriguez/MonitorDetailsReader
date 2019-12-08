using MonitorDetails.Factories;
using MonitorDetails.Interfaces;
using MonitorDetails.Models;
using MonitorDetails.Models.Native;
using MonitorDetails.Wrappers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Permissions;
using System.Threading;

namespace MonitorDetails
{
    /// <summary>
    ///     Retrieves current settings and useful information about the monitors
    ///     connected to this computer.
    /// </summary>
    public class Reader : IReader
    {
        readonly IMarshal _marshal;
        readonly INativeMethods _nativeMethods;
        readonly IEdidFactory _edidFactory;

        public Reader() : this(new MarshalWrapper(), new NativeMethodsWrapper(), new EdidFactory())
        {
        }

        public Reader(IMarshal marshal, INativeMethods nativeMethods, IEdidFactory edidFactory)
        {
            _marshal = marshal ?? throw new ArgumentNullException(nameof(marshal));
            _nativeMethods = nativeMethods ?? throw new ArgumentNullException(nameof(nativeMethods));
            _edidFactory = edidFactory ?? throw new ArgumentNullException(nameof(edidFactory));
        }

        /// <summary>
        ///     Creates a collection of <see cref="Monitor"/> objects containing information about
        ///     all monitors connected to this computer.
        /// </summary>
        /// <returns>
        ///     A collection of <see cref="Monitor"/> objects containing information about all
        ///     monitors connected to this computer.
        /// </returns>
        public IEnumerable<Models.Monitor> GetMonitorDetails()
        {
            var monitors = new List<Models.Monitor>();

            var enumSuccess =
                _nativeMethods.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, (IntPtr a, IntPtr b, ref Rect c, IntPtr d) =>
                {
                    var monitorInfo = new MonitorInfoEx { Size = (uint)_marshal.SizeOf<MonitorInfoEx>() };

                    if (!_nativeMethods.GetMonitorInfo(a, ref monitorInfo))
                    {
                        return false;
                    }

                    monitors.Add(new Models.Monitor
                    {
                        IsPrimaryMonitor = monitorInfo.Flags == NativeConstants.MONITORINFOF_PRIMARY,
                        Handle = a,
                        DisplayAdapter = new Models.DisplayDevice { Name = monitorInfo.Device },
                        MonitorCoordinates = Helpers.GetRectangle(ref monitorInfo.Monitor),
                        WorkAreaCoordinates = Helpers.GetRectangle(ref monitorInfo.Work)
                    });

                    return true;
                }, IntPtr.Zero);

            if (enumSuccess)
            {
                GetDisplayDevicesForMonitors(monitors);

                using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10d)))
                {
                    GetSizeForDisplayDevices(monitors, cts.Token);
                }
            }

            return monitors;
        }

        void GetDisplayAdapterInfoForMonitor(Models.Monitor monitorDetails)
        {
            var devMode = new DevMode { Size = (ushort)_marshal.SizeOf<DevMode>() };

            if (!_nativeMethods.EnumDisplaySettings(monitorDetails.DisplayAdapter.Name,
                NativeConstants.ENUM_CURRENT_SETTINGS, ref devMode))
            {
                return;
            }

            monitorDetails.Frequency = Convert.ToInt32(devMode.DisplayFrequency);
            monitorDetails.Resolution = new Rectangle
            {
                Width = Convert.ToInt32(devMode.PelsWidth),
                Height = Convert.ToInt32(devMode.PelsHeight)
            };

            // DevMode.LogPixels was returning the same density for all monitors, so some division is used to get the
            // scaling factor and DPI.
            monitorDetails.ScalingFactor =
                Convert.ToSingle(monitorDetails.Resolution.Width) / monitorDetails.MonitorCoordinates.Width;
            monitorDetails.Dpi = Convert.ToInt32(96f * monitorDetails.ScalingFactor);
        }

        [EnvironmentPermission(SecurityAction.Demand)]
        void GetDisplayDevicesForMonitors(List<Models.Monitor> monitors)
        {
            var displayAdapter = new Models.Native.DisplayDevice
                { Size = (uint)_marshal.SizeOf<Models.Native.DisplayDevice>() };
            uint devId = 0;

            while (_nativeMethods.EnumDisplayDevices(null, devId, ref displayAdapter, 0))
            {
                ++devId;

                var monitor = monitors.Find(m => m.DisplayAdapter.Name == displayAdapter.DeviceName);

                if (monitor == null)
                {
                    continue;
                }

                var displayMonitor = new Models.Native.DisplayDevice
                    { Size = (uint)_marshal.SizeOf<Models.Native.DisplayDevice>() };
                uint monId = 0;

                monitor.DisplayAdapter = new Models.DisplayDevice();
                Helpers.FillDisplayDevice(monitor.DisplayAdapter, displayAdapter);

                while (_nativeMethods.EnumDisplayDevices(displayAdapter.DeviceName, monId, ref displayMonitor, 0))
                {
                    ++monId;
                    Helpers.FillDisplayDevice(monitor, displayMonitor);
                }

                GetDisplayAdapterInfoForMonitor(monitor);
            }
        }

        byte[] GetMonitorEdidFromRegistry(IntPtr edidRegKey)
        {
            uint edidSize = 128;
            var edidData = new byte[edidSize];

            return _nativeMethods.RegQueryValueEx(edidRegKey, "EDID", IntPtr.Zero, IntPtr.Zero, edidData, ref edidSize)
                == NativeConstants.ERROR_SUCCESS
                ? edidData
                : new byte[0];
        }

        [EnvironmentPermission(SecurityAction.Demand)]
        void GetSizeForDisplayDevices(List<Models.Monitor> monitors, CancellationToken token)
        {
            IntPtr devInfo = _nativeMethods.SetupDiGetClassDevsEx(new[] { NativeConstants.MonitorClassId }, null,
                IntPtr.Zero, NativeConstants.DIGCF_PRESENT | NativeConstants.DIGCF_PROFILE, IntPtr.Zero, null,
                IntPtr.Zero);

            if (devInfo == IntPtr.Zero)
            {
                return;
            }

            for (uint i = 0; ShouldDisplayDeviceLoopContinue(token); ++i)
            {
                var devInfoData = new SpDevInfoData { Size = (uint)_marshal.SizeOf<SpDevInfoData>() };

                if (!_nativeMethods.SetupDiEnumDeviceInfo(devInfo, i, ref devInfoData))
                {
                    continue;
                }

                var instanceId = new char[NativeConstants.MAX_PATH];

                if (!_nativeMethods.SetupDiGetDeviceInstanceId(devInfo, ref devInfoData, instanceId,
                    NativeConstants.MAX_PATH, IntPtr.Zero))
                {
                    continue;
                }

                var instanceIdSize = Array.IndexOf(instanceId, '\0');

                var instanceIdString =
                    new string(instanceId, 0, instanceIdSize > 0 ? instanceIdSize : instanceId.Length);
                var monitor = monitors.Find(m => instanceIdString.Contains(Helpers.GetDeviceId(m.Id)));

                if (monitor == null)
                {
                    continue;
                }

                IntPtr edidRegKey = _nativeMethods.SetupDiOpenDevRegKey(devInfo, ref devInfoData,
                    NativeConstants.DICS_FLAG_GLOBAL, 0, NativeConstants.DIREG_DEV, NativeConstants.KEY_READ);

                if (edidRegKey != NativeConstants.INVALID_HANDLE_VALUE)
                {
                    var edid = GetMonitorEdidFromRegistry(edidRegKey);
                    if (edid.Length > 0)
                    {
                        monitor.Edid = _edidFactory.Create(edid);
                        monitor.Dimensions = Helpers.GetMonitorSizeFromEdid(edid);
                    }
                }

                _nativeMethods.RegCloseKey(edidRegKey);
            }

            _nativeMethods.SetupDiDestroyDeviceInfoList(devInfo);
        }

        bool ShouldDisplayDeviceLoopContinue(CancellationToken token)
        {
            return _marshal.GetLastWin32Error() != NativeConstants.ERROR_NO_MORE_ITEMS
                    && !token.IsCancellationRequested;
        }
    }
}
