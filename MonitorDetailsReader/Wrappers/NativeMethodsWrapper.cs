using MonitorDetails.Interfaces;
using MonitorDetails.Models.Native;
using System;

namespace MonitorDetails.Wrappers
{
    class NativeMethodsWrapper : INativeMethods
    {
        public bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref DisplayDevice lpDisplayDevice, uint dwFlags)
        {
            return NativeMethods.EnumDisplayDevices(lpDevice, iDevNum, ref lpDisplayDevice, dwFlags);
        }

        public bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumDelegate lpfnEnum, IntPtr dwData)
        {
            return NativeMethods.EnumDisplayMonitors(hdc, lprcClip, lpfnEnum, dwData);
        }

        public bool EnumDisplaySettings(string deviceName, uint modeNum, ref DevMode devMode)
        {
            return NativeMethods.EnumDisplaySettings(deviceName, modeNum, ref devMode);
        }

        public bool GetMonitorInfo(IntPtr hMonitor, ref MonitorInfoEx lpmi)
        {
            return NativeMethods.GetMonitorInfo(hMonitor, ref lpmi);
        }

        public uint RegCloseKey(IntPtr hKey)
        {
            return NativeMethods.RegCloseKey(hKey);
        }

        public uint RegQueryValueEx(IntPtr hKey, string lpValueName, IntPtr lpReserved, IntPtr lpType, byte[] lpData, ref uint lpcbData)
        {
            return NativeMethods.RegQueryValueEx(hKey, lpValueName, lpReserved, lpType, lpData, ref lpcbData);
        }

        public bool SetupDiDestroyDeviceInfoList(IntPtr deviceInfoSet)
        {
            return NativeMethods.SetupDiDestroyDeviceInfoList(deviceInfoSet);
        }

        public bool SetupDiEnumDeviceInfo(IntPtr deviceInfoSet, uint memberIndex, ref SpDevInfoData deviceInfoData)
        {
            return NativeMethods.SetupDiEnumDeviceInfo(deviceInfoSet, memberIndex, ref deviceInfoData);
        }

        public IntPtr SetupDiGetClassDevsEx(Guid[] classGuid, string enumerator, IntPtr hwndParent, uint flags, IntPtr deviceInfoSet,
            string machineName, IntPtr reserved)
        {
            return NativeMethods.SetupDiGetClassDevsEx(classGuid, enumerator, hwndParent, flags, deviceInfoSet, machineName, reserved);
        }

        public bool SetupDiGetDeviceInstanceId(IntPtr deviceInfoSet, ref SpDevInfoData deviceInfoData, char[] deviceInstanceId,
            uint deviceInstanceIdSize, IntPtr requiredSize)
        {
            return NativeMethods.SetupDiGetDeviceInstanceId(deviceInfoSet, ref deviceInfoData, deviceInstanceId, deviceInstanceIdSize,
                requiredSize);
        }

        public IntPtr SetupDiOpenDevRegKey(IntPtr deviceInfoSet, ref SpDevInfoData deviceInfoData, uint scope, uint hwProfile,
            uint keyType, uint samDesired)
        {
            return NativeMethods.SetupDiOpenDevRegKey(deviceInfoSet, ref deviceInfoData, scope, hwProfile, keyType, samDesired);
        }
    }
}
