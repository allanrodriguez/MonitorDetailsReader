using System.Runtime.InteropServices;

namespace MonitorDetails.Models
{
    [StructLayout(LayoutKind.Explicit)]
    public class DeviceMode
    {
        [FieldOffset(0)] public short Orientation;
        [FieldOffset(2)] public short PaperSize;
        [FieldOffset(4)] public short PaperLength;
        [FieldOffset(6)] public short PaperWidth;
        [FieldOffset(8)] public short Scale;
        [FieldOffset(10)] public short Copies;
        [FieldOffset(12)] public short DefaultSource;
        [FieldOffset(14)] public short PrintQuality;

        [FieldOffset(0)] public int X;
        [FieldOffset(4)] public int Y;

        [FieldOffset(8)] public uint DisplayOrientation;
        [FieldOffset(12)] public uint DisplayFixedOutput;
    }
}
