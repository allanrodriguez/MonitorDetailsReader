using MDReader.Abstractions;
using System;
using System.Collections.ObjectModel;
using System.Drawing;

namespace MDReader
{
    /// <summary>
    ///     Stores details about a specific monitor connected to this computer.
    /// </summary>
    public class MonitorDetails : IMonitorDetails
    {
        internal MonitorDetails() { }

        public RectangleF Dimensions { get; internal set; }

        public IDisplayDevice DisplayAdapter { get; internal set; }

        public int Dpi { get; internal set; }

        public ReadOnlyCollection<byte> Edid { get; internal set; }

        public int Frequency { get; internal set; }

        public IntPtr Handle { get; internal set; }

        public bool IsPrimaryMonitor { get; internal set; }

        public Rectangle MonitorCoordinates { get; internal set; }

        public Rectangle Resolution { get; internal set; }

        public float ScalingFactor { get; internal set; }

        public Rectangle WorkAreaCoordinates { get; internal set; }

        public string Id { get; internal set; }

        public string Key { get; internal set; }

        public string Name { get; internal set; }

        public DeviceStateFlags StateFlags { get; internal set; }

        public string String { get; internal set; }
    }
}
