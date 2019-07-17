using EDIDParser;
using System;
using System.Drawing;

namespace MonitorDetails.Models
{
    /// <summary>
    ///     Stores details about a monitor connected to this computer.
    /// </summary>
    public class Monitor : DisplayDevice
    {
        public RectangleF Dimensions { get; set; }

        public DisplayDevice DisplayAdapter { get; set; }

        public int Dpi { get; set; }

        public EDID Edid { get; set; }

        public int Frequency { get; set; }

        public IntPtr Handle { get; set; }

        public bool IsPrimaryMonitor { get; set; }

        public Rectangle MonitorCoordinates { get; set; }

        public Rectangle Resolution { get; set; }

        public float ScalingFactor { get; set; }

        public Rectangle WorkAreaCoordinates { get; set; }

        public override string ToString()
        {
            var primary = IsPrimaryMonitor ? "Primary, " : string.Empty;

            return $"{Description} ({primary}{MonitorCoordinates.Width}x{MonitorCoordinates.Height})";
        }
    }
}
