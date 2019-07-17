using MonitorDetails.Models.Native;
using System;
using System.Runtime.InteropServices;

namespace MonitorDetails
{
    /// <summary>
    ///     Monitor enum delegate
    /// </summary>
    /// <param name="hMonitor">
    ///     A handle to the display monitor.
    /// </param>
    /// <param name="hdcMonitor">
    ///     A handle to a device context.
    /// </param>
    /// <param name="lprcMonitor">
    ///     A pointer to a <see cref="Rect"/> structure.
    /// </param>
    /// <param name="dwData">
    ///     Application-defined data that
    ///     <see cref="NativeMethods.EnumDisplayMonitors(IntPtr, IntPtr, MonitorEnumDelegate, IntPtr)"/> passes
    ///     directly to the enumeration function.
    /// </param>
    /// <returns></returns>
    public delegate bool MonitorEnumDelegate(IntPtr hMonitor,
                                             IntPtr hdcMonitor,
                                             ref Rect lprcMonitor,
                                             IntPtr dwData);

    public class NativeConstants
    {
        public static readonly Guid MonitorClassId = new Guid(0x4d36e96e, 0xe325, 0x11ce, 0xbf, 0xc1, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18);
        public static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        public const int CCHDEVICENAME = 32;
        public const int CCHFORMNAME = 32;
        public const uint DICS_FLAG_GLOBAL = 1;
        public const uint DIGCF_PRESENT = 0x00000002;
        public const uint DIGCF_PROFILE = 0x00000008;
        public const uint DIREG_DEV = 0x00000001;
        public const uint ENUM_CURRENT_SETTINGS = 0xffffffff;
        public const uint ENUM_REGISTRY_SETTINGS = 0xfffffffe;
        public const int ERROR_NO_MORE_ITEMS = 259;
        public const uint ERROR_SUCCESS = 0;
        public const uint KEY_READ = 0x20019;
        public const uint MAX_PATH = 260;
        public const uint MONITORINFOF_PRIMARY = 1;
    }

    class NativeMethods
    {
        /// <summary>
        ///     Obtains information about the display devices in the current session.
        /// </summary>
        /// <param name="lpDevice">
        ///     A pointer to the device name. If <see langword="null"/>, function returns information for the display
        ///     adapter(s) on the machine, based on <paramref name="iDevNum"/>.
        /// </param>
        /// <param name="iDevNum">
        ///     An index value that specifies the display device of interest.
        /// </param>
        /// <param name="lpDisplayDevice">
        ///     A pointer to a <see cref="DisplayDevice"/> structure that receives information about the display device
        ///     specified by <paramref name="iDevNum"/>.
        /// </param>
        /// <param name="dwFlags"></param>
        /// <returns>
        ///     If the function succeeds, the return value is <see langword="true"/>. If the function fails, the return
        ///     value is <see langword="false"/>. The function fails if iDevNum is greater than the largest device
        ///     index.
        /// </returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool EnumDisplayDevices(string lpDevice,
                                                       uint iDevNum,
                                                       ref DisplayDevice lpDisplayDevice,
                                                       uint dwFlags);

        /// <summary>
        ///     Enumerates through the display monitors.
        /// </summary>
        /// <param name="hdc">
        ///     A handle to a display device context that defines the visible region of interest.
        /// </param>
        /// <param name="lprcClip">
        ///     A pointer to a <see cref="Rect"/> structure that specifies a clipping rectangle.
        /// </param>
        /// <param name="lpfnEnum">
        ///     A pointer to a MonitorEnumProc application-defined callback function.
        /// </param>
        /// <param name="dwData">
        ///     Application-defined data that
        ///     <see cref="EnumDisplayMonitors(IntPtr, IntPtr, MonitorEnumDelegate, IntPtr)"/> passes directly to the
        ///     MonitorEnumProc function.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is <see langword="true"/>. If the function fails, the
        ///     return value is <see langword="false"/>.
        /// </returns>
        [DllImport("user32.dll")]
        internal static extern bool EnumDisplayMonitors(IntPtr hdc,
                                                        IntPtr lprcClip,
                                                        MonitorEnumDelegate lpfnEnum,
                                                        IntPtr dwData);

        /// <summary>
        ///     Retrieves information about one of the graphics modes for a display device.
        /// </summary>
        /// <param name="deviceName">
        ///     A <see cref="string"/> that specifies the display device about whose graphics mode the function will
        ///     obtain information.
        ///     <para>
        ///         This parameter is either <see langword="null"/> or a <see cref="DisplayDevice.DeviceName"/>
        ///         returned from <see cref="EnumDisplayDevices(string, uint, ref DisplayDevice, uint)"/>. A
        ///         <see langword="null"/> value specifies the current display device on the computer on which the
        ///         calling thread is running.
        ///     </para>
        /// </param>
        /// <param name="modeNum">
        ///     The type of information to be retrieved.
        /// </param>
        /// <param name="devMode">
        ///     A pointer to a <see cref="DevMode"/> structure into which the function stores information about the
        ///     specified graphics mode.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is <see langword="true"/>. If the function fails, the return
        ///     value is <see langword="false"/>.
        /// </returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool EnumDisplaySettings(string deviceName,
                                                        uint modeNum,
                                                        ref DevMode devMode);

        /// <summary>
        ///     Retrieves information about a display monitor.
        /// </summary>
        /// <param name="hMonitor">
        ///     A handle to the display monitor of interest.
        /// </param>
        /// <param name="lpmi">
        ///     A pointer to a <see cref="MonitorInfoEx"/> structure that receives information about the specified
        ///     display monitor.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is <see langword="true"/>. If the function fails, the
        ///     return value is <see langword="false"/>.
        /// </returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor,
                                                   ref MonitorInfoEx lpmi);

        /// <summary>
        ///     Closes a handle to the specified registry key.
        /// </summary>
        /// <param name="hKey">
        ///     A handle to the open key to be closed.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is <see cref="ERROR_SUCCESS"/>. If the function fails, the
        ///     return value is a nonzero error code.
        /// </returns>
        [DllImport("advapi32.dll")]
        internal static extern uint RegCloseKey(IntPtr hKey);

        /// <summary>
        ///     Retrieves the type and data for the specified value name associated with an open registry key.
        /// </summary>
        /// <param name="hKey">
        ///     A handle to an open registry key.
        /// </param>
        /// <param name="lpValueName">
        ///     The name of the registry value.
        /// </param>
        /// <param name="lpReserved">
        ///     This parameter is reserved and must be 0.
        /// </param>
        /// <param name="lpType">
        ///     A pointer to a variable that receives a code indicating the type of data stored in the specified value.
        /// </param>
        /// <param name="lpData">
        ///     A pointer to a buffer that receives the value's data. This parameter can be <see langword="null"/> if
        ///     the data is not required.
        /// </param>
        /// <param name="lpcbData">
        ///     A pointer to a variable that specifies the size of the buffer pointed to by the
        ///     <paramref name="lpData"/> parameter, in bytes.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is <see cref="ERROR_SUCCESS"/>. If the function fails, the
        ///     return value is a system error code.
        /// </returns>
        [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
        internal static extern uint RegQueryValueEx(IntPtr hKey,
                                                    string lpValueName,
                                                    IntPtr lpReserved,
                                                    IntPtr lpType,
                                                    byte[] lpData,
                                                    ref uint lpcbData);

        /// <summary>
        ///     Returns a <see cref="SpDevInfoData"/> structure that specifies a device information element in a device
        ///     information set.
        /// </summary>
        /// <param name="deviceInfoSet">
        ///     A handle to the device information set for which to return an <see cref="SpDevInfoData"/> structure
        ///     that represents a device information element.
        /// </param>
        /// <param name="memberIndex">
        ///     A zero-based index of the device information element to retrieve.
        /// </param>
        /// <param name="deviceInfoData">
        ///     A pointer to an <see cref="SpDevInfoData"/> structure to receive information about an enumerated device
        ///     information element.
        /// </param>
        /// <returns>
        ///     Returns <see langword="true"/> if it is successful. Otherwise, it returns <see langword="false"/> and
        ///     the logged error can be retrieved with a call to <see cref="Marshal.GetLastWin32Error"/>.
        /// </returns>
        [DllImport("setupapi.dll", SetLastError = true)]
        internal static extern bool SetupDiEnumDeviceInfo(IntPtr deviceInfoSet,
                                                          uint memberIndex,
                                                          ref SpDevInfoData deviceInfoData);

        /// <summary>
        ///     Deletes a device information set and frees all associated memory.
        /// </summary>
        /// <param name="deviceInfoSet">
        ///     A handle to the device information set to delete.
        /// </param>
        /// <returns>
        ///     Returns <see langword="true"/> if it is successful. Otherwise, it returns <see langword="false"/> and
        ///     the logged error can be retrieved with a call to <see cref="Marshal.GetLastWin32Error"/>.
        /// </returns>
        [DllImport("setupapi.dll", SetLastError = true)]
        internal static extern bool SetupDiDestroyDeviceInfoList(IntPtr deviceInfoSet);

        /// <summary>
        ///     Returns a handle to a device information set that contains requested device information elements for a
        ///     local or a remote computer.
        /// </summary>
        /// <param name="classGuid">
        ///     A pointer to the GUID for a device setup class or a device interface class. This pointer is optional
        ///     and can be <see langword="null"/>. If a GUID value is not used to select devices, set
        ///     <paramref name="classGuid"/> to <see langword="null"/>.
        /// </param>
        /// <param name="enumerator">
        ///     A pointer to a <see cref="string"/> that specifies an identifier (ID) of a Plug and Play (PnP)
        ///     enumerator or a PnP device instance ID.
        /// </param>
        /// <param name="hwndParent">
        ///     A handle to the top-level window to be used for a user interface that is associated with installing a
        ///     device instance in the device information set. This handle is optional and can be
        ///     <see langword="null"/>.
        /// </param>
        /// <param name="flags">
        ///     Specifies control options that filter the device information elements that are added to the device
        ///     information set.
        /// </param>
        /// <param name="deviceInfoSet">
        ///     The handle to an existing device information set to which
        ///     <see cref="SetupDiGetClassDevsEx(GUID, string, IntPtr, uint, IntPtr, string, IntPtr)"/>
        ///     adds the requested device information elements. This parameter is optional and can be set to
        ///     <see langword="null"/>.
        /// </param>
        /// <param name="machineName">
        ///     A pointer to a constant <see cref="string"/> that contains the name of a remote computer on which the
        ///     devices reside. A value of <see langword="null"/> for <paramref name="machineName"/> specifies that the
        ///     device is installed on the local computer.
        /// </param>
        /// <param name="reserved">
        ///     Reserved for internal use. This parameter must be set to <see langword="null"/>.
        /// </param>
        /// <returns>
        ///     If the operation succeeds, returns a handle to a device information set that contains all installed
        ///     devices that matched the supplied parameters. If the operation fails, the function returns
        ///     <see cref="NativeConstants.INVALID_HANDLE_VALUE"/> (-1). To get extended error information, call
        ///     <see cref="Marshal.GetLastWin32Error"/>.
        /// </returns>
        [DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr SetupDiGetClassDevsEx(Guid[] classGuid,
                                                            string enumerator,
                                                            IntPtr hwndParent,
                                                            uint flags,
                                                            IntPtr deviceInfoSet,
                                                            string machineName,
                                                            IntPtr reserved);

        /// <summary>
        ///     Retrieves the device instance ID that is associated with a device information element.
        /// </summary>
        /// <param name="deviceInfoSet">
        ///     A handle to the device information set that contains the device information element that represents the
        ///     device for which to retrieve a device instance ID.
        /// </param>
        /// <param name="deviceInfoData">
        ///     A pointer to an <see cref="SpDevInfoData"/> structure that specifies the device information element in
        ///     <paramref name="deviceInfoSet"/>.
        /// </param>
        /// <param name="deviceInstanceId">
        ///     A pointer to the character buffer that will receive the device instance ID for the specified device
        ///     information element.
        /// </param>
        /// <param name="deviceInstanceIdSize">
        ///     The size, in characters, of the <paramref name="deviceInstanceId"/> buffer.
        /// </param>
        /// <param name="requiredSize">
        ///     A pointer to the variable that receives the number of characters required to store the device instance
        ///     ID.
        /// </param>
        /// <returns>
        ///     The function returns <see langword="true"/> if it is successful. Otherwise, it returns
        ///     <see langword="false"/> and the logged error can be retrieved by making a call to
        ///     <see cref="Marshal.GetLastWin32Error"/>.
        /// </returns>
        [DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool SetupDiGetDeviceInstanceId(IntPtr deviceInfoSet,
                                                               ref SpDevInfoData deviceInfoData,
                                                               char[] deviceInstanceId,
                                                               uint deviceInstanceIdSize,
                                                               IntPtr requiredSize);

        /// <summary>
        ///     Opens a registry key for device-specific configuration information.
        /// </summary>
        /// <param name="deviceInfoSet">
        ///     A handle to the device information set that contains a device information element that represents the
        ///     device for which to open a registry key.
        /// </param>
        /// <param name="deviceInfoData">
        ///     A pointer to an <see cref="SpDevInfoData"/> structure that specifies the device information element in
        ///     <paramref name="deviceInfoSet"/>.
        /// </param>
        /// <param name="scope">
        ///     The scope of the registry key to open.
        /// </param>
        /// <param name="hwProfile">
        ///     A hardware profile value.
        /// </param>
        /// <param name="keyType">
        ///     The type of registry storage key to open.
        /// </param>
        /// <param name="samDesired">
        ///     The registry security access that is required for the requested key.
        /// </param>
        /// <returns>
        ///     If the function is successful, it returns a handle to an opened registry key where private
        ///     configuration data about this device instance can be stored/retrieved. If the function fails, it
        ///     returns <see cref="NativeConstants.INVALID_HANDLE_VALUE"/> (-1). To get extended error information,
        ///     call <see cref="Marshal.GetLastWin32Error"/>.
        /// </returns>
        [DllImport("setupapi.dll", SetLastError = true)]
        internal static extern IntPtr SetupDiOpenDevRegKey(IntPtr deviceInfoSet,
                                                           ref SpDevInfoData deviceInfoData,
                                                           uint scope,
                                                           uint hwProfile,
                                                           uint keyType,
                                                           uint samDesired);
    }
}
