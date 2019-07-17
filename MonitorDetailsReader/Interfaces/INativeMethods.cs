using MonitorDetails.Models.Native;
using System;

namespace MonitorDetails.Interfaces
{
    public interface INativeMethods
    {
        bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DisplayDevice lpDisplayDevice, uint dwFlags);

        bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumDelegate lpfnEnum, IntPtr dwData);

        bool EnumDisplaySettings(string deviceName, uint modeNum, ref DevMode devMode);

        bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfoEx lpmi);

        uint RegCloseKey(IntPtr hKey);

        uint RegQueryValueEx(IntPtr hKey, string lpValueName, IntPtr lpReserved, IntPtr lpType, byte[] lpData, ref uint lpcbData);

        bool SetupDiEnumDeviceInfo(IntPtr deviceInfoSet, uint memberIndex, ref SpDevInfoData deviceInfoData);

        bool SetupDiDestroyDeviceInfoList(IntPtr deviceInfoSet);

        IntPtr SetupDiGetClassDevsEx(Guid[] classGuid, string enumerator, IntPtr hwndParent, uint flags, IntPtr deviceInfoSet,
            string machineName, IntPtr reserved);

        bool SetupDiGetDeviceInstanceId(IntPtr deviceInfoSet, ref SpDevInfoData deviceInfoData, char[] deviceInstanceId,
            uint deviceInstanceIdSize, IntPtr requiredSize);

        IntPtr SetupDiOpenDevRegKey(IntPtr deviceInfoSet, ref SpDevInfoData deviceInfoData, uint scope, uint hwProfile, uint keyType,
            uint samDesired);
    }
}
