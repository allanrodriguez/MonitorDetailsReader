using MDReader.Abstractions;
using System;
using System.Collections.ObjectModel;

namespace MDReader
{
    /// <summary>
    ///     Stores details about a specific monitor connected to this computer.
    /// </summary>
    public class MonitorDetails : IMonitorDetails
    {
        internal MonitorDetails() { }

        public string DisplayAdapterId { get; internal set; }
        public string DisplayAdapterKey { get; internal set; }
        public string DisplayAdapterName { get; internal set; }
        public uint DisplayAdapterStateFlags { get; internal set; }
        public string DisplayAdapterString { get; internal set; }

        public string DisplayDeviceId { get; internal set; }
        public string DisplayDeviceKey { get; internal set; }
        public string DisplayDeviceName { get; internal set; }
        public uint DisplayDeviceStateFlags { get; internal set; }
        public string DisplayDeviceString { get; internal set; }

        public int Dpi { get; internal set; }

        public ReadOnlyCollection<byte> Edid { get; internal set; }

        public int Frequency { get; internal set; }

        public IntPtr Handle { get; internal set; }

        public bool IsPrimaryMonitor { get; internal set; }

        public string Name { get; internal set; }

        public double ScalingFactor { get; internal set; }

        public double WidthCentimeters { get; internal set; }
        public double HeightCentimeters { get; internal set; }

        public int WidthPhysicalPixels { get; internal set; }
        public int HeightPhysicalPixels { get; internal set; }

        public RECT MonitorCoordinates { get; internal set; }
        public RECT WorkAreaCoordinates { get; internal set; }

        public int WorkAreaWidth { get; internal set; }
        public int WorkAreaHeight { get; internal set; }

        public int WidthScaledPixels { get; internal set; }
        public int HeightScaledPixels { get; internal set; }
    }
}
