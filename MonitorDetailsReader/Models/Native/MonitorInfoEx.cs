using System.Runtime.InteropServices;

namespace MonitorDetails.Models.Native
{
    /// <summary>
    ///     Contains information about a display monitor.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct MonitorInfoEx
    {
        public uint Size;
        public Rect Monitor;
        public Rect Work;
        public uint Flags;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = NativeConstants.CCHDEVICENAME)]
        public string Device;
    }
}
