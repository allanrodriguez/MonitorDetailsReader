using System.Runtime.InteropServices;

namespace MonitorDetails.Models
{
    [StructLayout(LayoutKind.Explicit)]
    public class DeviceFlags
    {
        [FieldOffset(0)] public uint DisplayFlags;
        [FieldOffset(0)] public uint Nup;
    }
}
