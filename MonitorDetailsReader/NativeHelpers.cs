using System;
using System.Runtime.InteropServices;
using System.Text;

namespace MDReader
{
    class NativeHelpers
    {
        #region Constants
        internal static readonly GUID MonitorClassGuid = new GUID
        {
            Data1 = 0x4d36e96e,
            Data2 = 0xe325,
            Data3 = 0x11ce,
            Data4 = new byte[] { 0xbf, 0xc1, 0x08, 0x00, 0x2b, 0xe1, 0x03, 0x18 }
        };
        internal static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        internal const int CCHDEVICENAME = 32;
        internal const int CCHFORMNAME = 32;
        internal const uint DICS_FLAG_GLOBAL = 1;
        internal const uint DIGCF_PRESENT = 0x00000002;
        internal const uint DIGCF_PROFILE = 0x00000008;
        internal const uint DIREG_DEV = 0x00000001;
        internal const uint ENUM_CURRENT_SETTINGS = 0xffffffff;
        internal const uint ENUM_REGISTRY_SETTINGS = 0xfffffffe;
        internal const uint ERROR_NO_MORE_ITEMS = 259;
        internal const uint ERROR_SUCCESS = 0;
        internal const uint KEY_READ = 0x20019;
        internal const uint MAX_PATH = 260;
        internal const uint MONITORINFOF_PRIMARY = 0x00000001;
        #endregion

        /// <summary>
        ///     Monitor Enum Delegate
        /// </summary>
        /// <param name="hMonitor">
        ///     A handle to the display monitor.
        /// </param>
        /// <param name="hdcMonitor">
        ///     A handle to a device context.
        /// </param>
        /// <param name="lprcMonitor">
        ///     A pointer to a <see cref="RECT"/> structure.
        /// </param>
        /// <param name="dwData">
        ///     Application-defined data that
        ///     <see cref="EnumDisplayMonitors(IntPtr, IntPtr, MonitorEnumDelegate, IntPtr)"/>
        ///     passes directly to the enumeration function.
        /// </param>
        /// <returns></returns>
        internal delegate bool MonitorEnumDelegate(IntPtr hMonitor,
                                                   IntPtr hdcMonitor,
                                                   ref RECT lprcMonitor,
                                                   IntPtr dwData);

        /// <summary>
        ///     Obtains information about the display devices in the current session.
        /// </summary>
        /// <param name="lpDevice">
        ///     A pointer to the device name. If <see langword="null"/>, function returns
        ///     information for the display adapter(s) on the machine, based on
        ///     <paramref name="iDevNum"/>.
        /// </param>
        /// <param name="iDevNum">
        ///     An index value that specifies the display device of interest.
        /// </param>
        /// <param name="lpDisplayDevice">
        ///     A pointer to a <see cref="DISPLAY_DEVICE"/> structure that receives
        ///     information about the display device specified by
        ///     <paramref name="iDevNum"/>.
        /// </param>
        /// <param name="dwFlags"></param>
        /// <returns>
        ///     If the function succeeds, the return value is <see langword="true"/>. If the
        ///     function fails, the return value is <see langword="false"/>. The function
        ///     fails if iDevNum is greater than the largest device index.
        /// </returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool EnumDisplayDevices(string lpDevice,
                                                       uint iDevNum,
                                                       ref DISPLAY_DEVICE lpDisplayDevice,
                                                       uint dwFlags);

        /// <summary>
        ///     Enumerates through the display monitors.
        /// </summary>
        /// <param name="hdc">
        ///     A handle to a display device context that defines the
        ///     visible region of interest.
        /// </param>
        /// <param name="lprcClip">
        ///     A pointer to a <see cref="RECT"/> structure that specifies a
        ///     clipping rectangle.
        /// </param>
        /// <param name="lpfnEnum">
        ///     A pointer to a MonitorEnumProc application-defined callback
        ///     function.
        /// </param>
        /// <param name="dwData">
        ///     Application-defined data that
        ///     <see cref="EnumDisplayMonitors(IntPtr, IntPtr, MonitorEnumDelegate, IntPtr)"/>
        ///     passes directly to the MonitorEnumProc function.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is
        ///     <see langword="true"/>. If the function fails, the return
        ///     value is <see langword="false"/>.
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
        ///     A <see cref="string"/> that specifies the display device about whose graphics
        ///     mode the function will obtain information.
        ///     <para>
        ///         This parameter is either <see langword="null"/> or a
        ///         <see cref="DISPLAY_DEVICE.DeviceName"/> returned from
        ///         <see cref="EnumDisplayDevices(string, uint, ref DISPLAY_DEVICE, uint)"/>.
        ///         A <see langword="null"/> value specifies the current display device on the
        ///         computer on which the calling thread is running.
        ///     </para>
        /// </param>
        /// <param name="modeNum">
        ///     The type of information to be retrieved.
        /// </param>
        /// <param name="devMode">
        ///     A pointer to a <see cref="DEVMODE"/> structure into which the function stores
        ///     information about the specified graphics mode.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is <see langword="true"/>. If the
        ///     function fails, the return value is <see langword="false"/>.
        /// </returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool EnumDisplaySettings(string deviceName,
                                                        uint modeNum,
                                                        ref DEVMODE devMode);

        /// <summary>
        ///     Retrieves information about a display monitor.
        /// </summary>
        /// <param name="hMonitor">
        ///     A handle to the display monitor of interest.
        /// </param>
        /// <param name="lpmi">
        ///     A pointer to a <see cref="MONITORINFOEX"/>
        ///     structure that receives information about the
        ///     specified display monitor.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is
        ///     <see langword="true"/>. If the function fails, the
        ///     return value is <see langword="false"/>.
        /// </returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor,
                                                   ref MONITORINFOEX lpmi);

        /// <summary>
        ///     Closes a handle to the specified registry
        ///     key.
        /// </summary>
        /// <param name="hKey">
        ///     A handle to the open key to be closed.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is
        ///     <see cref="ERROR_SUCCESS"/>. If the function
        ///     fails, the return value is a nonzero error
        ///     code.
        /// </returns>
        [DllImport("advapi32.dll")]
        internal static extern uint RegCloseKey(IntPtr hKey);

        /// <summary>
        ///     Retrieves the type and data for the specified
        ///     value name associated with an open registry
        ///     key.
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
        ///     A pointer to a variable that receives a code
        ///     indicating the type of data stored in the
        ///     specified value.
        /// </param>
        /// <param name="lpData">
        ///     A pointer to a buffer that receives the
        ///     value's data. This parameter can be
        ///     <see langword="null"/> if the data is not
        ///     required.
        /// </param>
        /// <param name="lpcbData">
        ///     A pointer to a variable that specifies the
        ///     size of the buffer pointed to by the
        ///     <paramref name="lpData"/> parameter, in bytes.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is
        ///     <see cref="ERROR_SUCCESS"/>. If the function
        ///     fails, the return value is a system error code.
        /// </returns>
        [DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
        internal static extern uint RegQueryValueEx(IntPtr hKey,
                                                    string lpValueName,
                                                    uint lpReserved,
                                                    uint lpType,
                                                    byte[] lpData,
                                                    ref uint lpcbData);

        /// <summary>
        ///     Returns a <see cref="SP_DEVINFO_DATA"/> structure that specifies a
        ///     device information element in a device information set.
        /// </summary>
        /// <param name="deviceInfoSet">
        ///     A handle to the device information set for which to return an
        ///     <see cref="SP_DEVINFO_DATA"/> structure that represents a device
        ///     information element.
        /// </param>
        /// <param name="memberIndex">
        ///     A zero-based index of the device information element to retrieve.
        /// </param>
        /// <param name="deviceInfoData">
        ///     A pointer to an <see cref="SP_DEVINFO_DATA"/> structure to receive
        ///     information about an enumerated device information element.
        /// </param>
        /// <returns>
        ///     Returns <see langword="true"/> if it is successful. Otherwise, it
        ///     returns <see langword="false"/> and the logged error can be
        ///     retrieved with a call to <see cref="Marshal.GetLastWin32Error"/>.
        /// </returns>
        [DllImport("setupapi.dll", SetLastError = true)]
        internal static extern bool SetupDiEnumDeviceInfo(IntPtr deviceInfoSet,
                                                          uint memberIndex,
                                                          ref SP_DEVINFO_DATA deviceInfoData);

        /// <summary>
        ///     Deletes a device information set and frees all associated memory.
        /// </summary>
        /// <param name="deviceInfoSet">
        ///     A handle to the device information set to delete.
        /// </param>
        /// <returns>
        ///     Returns <see langword="true"/> if it is successful. Otherwise, it
        ///     returns <see langword="false"/> and the logged error can be retrieved
        ///     with a call to <see cref="Marshal.GetLastWin32Error"/>.
        /// </returns>
        [DllImport("setupapi.dll", SetLastError = true)]
        internal static extern bool SetupDiDestroyDeviceInfoList(IntPtr deviceInfoSet);

        /// <summary>
        ///     Returns a handle to a device information set that contains
        ///     requested device information elements for a local or a remote
        ///     computer.
        /// </summary>
        /// <param name="classGuid">
        ///     A pointer to the GUID for a device setup class or a device
        ///     interface class. This pointer is optional and can be
        ///     <see langword="null"/>. If a GUID value is not used to select
        ///     devices, set <paramref name="classGuid"/> to
        ///     <see langword="null"/>.
        /// </param>
        /// <param name="enumerator">
        ///     A pointer to a <see cref="string"/> that specifies an identifier
        ///     (ID) of a Plug and Play (PnP) enumerator or a PnP device instance
        ///     ID.
        /// </param>
        /// <param name="hwndParent">
        ///     A handle to the top-level window to be used for a user interface
        ///     that is associated with installing a device instance in the device
        ///     information set. This handle is optional and can be
        ///     <see langword="null"/>.
        /// </param>
        /// <param name="flags">
        ///     Specifies control options that filter the device information
        ///     elements that are added to the device information set.
        /// </param>
        /// <param name="deviceInfoSet">
        ///     The handle to an existing device information set to which
        ///     <see cref="SetupDiGetClassDevsEx(GUID, string, IntPtr, uint, IntPtr, string, IntPtr)"/>
        ///     adds the requested device information elements. This parameter is
        ///     optional and can be set to <see langword="null"/>.
        /// </param>
        /// <param name="machineName">
        ///     A pointer to a constant <see cref="string"/> that contains the name
        ///     of a remote computer on which the devices reside. A value of
        ///     <see langword="null"/> for <paramref name="machineName"/> specifies
        ///     that the device is installed on the local computer.
        /// </param>
        /// <param name="reserved">
        ///     Reserved for internal use. This parameter must be set to
        ///     <see langword="null"/>.
        /// </param>
        /// <returns>
        ///     If the operation succeeds, returns a handle to a device information
        ///     set that contains all installed devices that matched the supplied
        ///     parameters. If the operation fails, the function returns
        ///     INVALID_HANDLE_VALUE (-1). To get extended error information, call
        ///     <see cref="Marshal.GetLastWin32Error"/>.
        /// </returns>
        [DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr SetupDiGetClassDevsEx(GUID[] classGuid,
                                                            string enumerator,
                                                            IntPtr hwndParent,
                                                            uint flags,
                                                            IntPtr deviceInfoSet,
                                                            string machineName,
                                                            IntPtr reserved);

        /// <summary>
        ///     Retrieves the device instance ID that is associated with a device
        ///     information element.
        /// </summary>
        /// <param name="deviceInfoSet">
        ///     A handle to the device information set that contains the device
        ///     information element that represents the device for which to retrieve a
        ///     device instance ID.
        /// </param>
        /// <param name="deviceInfoData">
        ///     A pointer to an <see cref="SP_DEVINFO_DATA"/> structure that specifies
        ///     the device information element in <paramref name="deviceInfoSet"/>.
        /// </param>
        /// <param name="deviceInstanceId">
        ///     A pointer to the character buffer that will receive the device instance
        ///     ID for the specified device information element.
        /// </param>
        /// <param name="deviceInstanceIdSize">
        ///     The size, in characters, of the <paramref name="deviceInstanceId"/>
        ///     buffer.
        /// </param>
        /// <param name="requiredSize">
        ///     A pointer to the variable that receives the number of characters required
        ///     to store the device instance ID.
        /// </param>
        /// <returns>
        ///     The function returns <see langword="true"/> if it is successful.
        ///     Otherwise, it returns <see langword="false"/> and the logged error can be
        ///     retrieved by making a call to <see cref="Marshal.GetLastWin32Error"/>.
        /// </returns>
        [DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern bool SetupDiGetDeviceInstanceId(IntPtr deviceInfoSet,
                                                               ref SP_DEVINFO_DATA deviceInfoData,
                                                               char[] deviceInstanceId,
                                                               uint deviceInstanceIdSize,
                                                               uint requiredSize);

        /// <summary>
        ///     Opens a registry key for device-specific configuration information.
        /// </summary>
        /// <param name="deviceInfoSet">
        ///     A handle to the device information set that contains a device
        ///     information element that represents the device for which to open a
        ///     registry key.
        /// </param>
        /// <param name="deviceInfoData">
        ///     A pointer to an <see cref="SP_DEVINFO_DATA"/> structure that
        ///     specifies the device information element in
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
        ///     If the function is successful, it returns a handle to an opened
        ///     registry key where private configuration data about this device
        ///     instance can be stored/retrieved. If the function fails, it returns
        ///     INVALID_HANDLE_VALUE (-1). To get extended error information, call
        ///     <see cref="Marshal.GetLastWin32Error"/>.
        /// </returns>
        [DllImport("setupapi.dll", SetLastError = true)]
        internal static extern IntPtr SetupDiOpenDevRegKey(IntPtr deviceInfoSet,
                                                           ref SP_DEVINFO_DATA deviceInfoData,
                                                           uint scope,
                                                           uint hwProfile,
                                                           uint keyType,
                                                           uint samDesired);
    }

    #region Native structs
    /// <summary>
    /// Contains information about the initialization and environment of a printer or a
    /// display device.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
    struct DEVMODE
    {
        [FieldOffset(0)]
        public Char32Array DeviceName;
        [FieldOffset(64)]
        public ushort SpecVersion;
        [FieldOffset(66)]
        public ushort DriverVersion;
        [FieldOffset(68)]
        public ushort Size;
        [FieldOffset(70)]
        public ushort DriverExtra;
        [FieldOffset(72)]
        public uint Fields;

        #region DummyUnion
        [FieldOffset(76)]
        public short Orientation;
        [FieldOffset(78)]
        public short PaperSize;
        [FieldOffset(80)]
        public short PaperLength;
        [FieldOffset(82)]
        public short PaperWidth;
        [FieldOffset(84)]
        public short Scale;
        [FieldOffset(86)]
        public short Copies;
        [FieldOffset(88)]
        public short DefaultSource;
        [FieldOffset(90)]
        public short PrintQuality;

        [FieldOffset(76)]
        public int X;
        [FieldOffset(80)]
        public int Y;

        [FieldOffset(84)]
        public uint DisplayOrientation;
        [FieldOffset(88)]
        public uint DisplayFixedOutput;
        #endregion

        [FieldOffset(92)]
        public short Color;
        [FieldOffset(94)]
        public short Duplex;
        [FieldOffset(96)]
        public short YResolution;
        [FieldOffset(98)]
        public short TTOption;
        [FieldOffset(100)]
        public short Collate;
        [FieldOffset(102)]
        public Char32Array FormName;
        [FieldOffset(166)]
        public ushort LogPixels;
        [FieldOffset(168)]
        public uint BitsPerPel;
        [FieldOffset(172)]
        public uint PelsWidth;
        [FieldOffset(176)]
        public uint PelsHeight;

        #region DummyUnion 2
        [FieldOffset(180)]
        public uint DisplayFlags;

        [FieldOffset(180)]
        public uint Nup;
        #endregion

        [FieldOffset(184)]
        public uint DisplayFrequency;
        [FieldOffset(188)]
        public uint ICMMethod;
        [FieldOffset(192)]
        public uint ICMIntent;
        [FieldOffset(196)]
        public uint MediaType;
        [FieldOffset(200)]
        public uint DitherType;
        [FieldOffset(204)]
        public uint Reserved1;
        [FieldOffset(208)]
        public uint Reserved2;
        [FieldOffset(212)]
        public uint PanningWidth;
        [FieldOffset(216)]
        public uint PanningHeight;
    }

    /// <summary>
    /// Receives information about the display device specified by
    /// the iDevNum parameter of the EnumDisplayDevices function.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    struct DISPLAY_DEVICE
    {
        public uint Size;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string DeviceName;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceString;

        public uint StateFlags;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceId;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string DeviceKey;
    }

    /// <summary>
    /// Stores a GUID.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    struct GUID
    {
        public uint Data1;
        public ushort Data2;
        public ushort Data3;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] Data4;
    }

    /// <summary>
    /// Contains information about a display monitor.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    struct MONITORINFOEX
    {
        public uint Size;
        public RECT Monitor;
        public RECT Work;
        public uint Flags;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NativeHelpers.CCHDEVICENAME)]
        public string Device;
    }

    /// <summary>
    /// Defines a rectangle by the
    /// coordinates of its upper-left and
    /// lower-right corners.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    /// <summary>
    /// Defines a device instance that is
    /// a member of a device information
    /// set.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    struct SP_DEVINFO_DATA
    {
        public uint Size;
        public GUID ClassGuid;
        public uint DevInst;
        public IntPtr Reserved;
    }
    #endregion

    [StructLayout(LayoutKind.Sequential)]
    internal struct Char32Array
    {
        public char A;
        public char B;
        public char C;
        public char D;
        public char E;
        public char F;
        public char G;
        public char H;
        public char I;
        public char J;
        public char K;
        public char L;
        public char M;
        public char N;
        public char O;
        public char P;
        public char Q;
        public char R;
        public char S;
        public char T;
        public char U;
        public char V;
        public char W;
        public char X;
        public char Y;
        public char Z;
        public char Aa;
        public char Ab;
        public char Ac;
        public char Ad;
        public char Ae;
        public char Af;

        public override string ToString()
        {
            return new StringBuilder(32).Append(A)
                                        .Append(B)
                                        .Append(C)
                                        .Append(D)
                                        .Append(E)
                                        .Append(F)
                                        .Append(G)
                                        .Append(H)
                                        .Append(I)
                                        .Append(J)
                                        .Append(K)
                                        .Append(L)
                                        .Append(M)
                                        .Append(N)
                                        .Append(O)
                                        .Append(P)
                                        .Append(Q)
                                        .Append(R)
                                        .Append(S)
                                        .Append(T)
                                        .Append(U)
                                        .Append(V)
                                        .Append(W)
                                        .Append(X)
                                        .Append(Y)
                                        .Append(Z)
                                        .Append(Aa)
                                        .Append(Ab)
                                        .Append(Ac)
                                        .Append(Ad)
                                        .Append(Ae)
                                        .Append(Af)
                                        .ToString();
        }
    }
}
