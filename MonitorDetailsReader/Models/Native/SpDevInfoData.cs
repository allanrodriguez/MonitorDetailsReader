using System;
using System.Runtime.InteropServices;

namespace MonitorDetails.Models.Native
{
    /// <summary>
    ///     Defines a device instance that is a member of a device information set.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SpDevInfoData
    {
        public uint Size;
        public Guid ClassGuid;
        public uint DevInst;
        public IntPtr Reserved;
    }
}
