using System.Runtime.InteropServices;

namespace MonitorDetails.Models.Native
{
    /// <summary>
    ///     Defines a rectangle by the coordinates of its upper-left and lower-right
    ///     corners.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }
}
