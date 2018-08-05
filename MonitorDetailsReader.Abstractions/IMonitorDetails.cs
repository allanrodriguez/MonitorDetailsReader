using System;
using System.Collections.ObjectModel;
using System.Drawing;

namespace MDReader.Abstractions
{
    /// <summary>
    ///     Stores details about a specific monitor connected to this computer.
    /// </summary>
    public interface IMonitorDetails : IDisplayDevice
    {
        /// <summary>
        ///     Specifies the width and height of the monitor display, in centimeters. These
        ///     values have been parsed from the monitor's EDID.
        /// </summary>
        RectangleF Dimensions { get; }

        /// <summary>
        ///     Specifies details about the display adapter the monitor is connected to.
        /// </summary>
        IDisplayDevice DisplayAdapter { get; }

        /// <summary>
        ///     Specifies the pixels per logical inch of the monitor. By default, a logical
        ///     inch is equal to 96 pixels.
        ///     <para>
        ///         For more information regarding DPI and logical inches in Windows, see
        ///         https://docs.microsoft.com/en-us/windows/desktop/learnwin32/dpi-and-device-independent-pixels.
        ///     </para>
        /// </summary>
        int Dpi { get; }

        /// <summary>
        ///     Specifies the Extended Display Identification Data, or EDID, of the display
        ///     monitor.
        /// </summary>
        ReadOnlyCollection<byte> Edid { get; }

        /// <summary>
        ///     The frequency of the monitor, in hertz.
        /// </summary>
        int Frequency { get; }

        /// <summary>
        /// A handle to the display monitor.
        /// </summary>
        IntPtr Handle { get; }

        /// <summary>
        ///     If the monitor is the primary display monitor, this property is
        ///     <see langword="true"/>. If the monitor is not the primary display monitor,
        ///     this property is <see langword="false"/>.
        /// </summary>
        bool IsPrimaryMonitor { get; }

        /// <summary>
        ///     Specifies the DPI-scaled display monitor rectangle, expressed in
        ///     virtual-screen coordinates. Note that if the monitor is not the primary
        ///     display monitor, some of the rectangle's coordinates may be negative values.
        /// </summary>
        Rectangle MonitorCoordinates { get; }

        /// <summary>
        ///     Specifies the native resolution of the monitor in pixels, regardless of DPI
        ///     scaling.
        /// </summary>
        Rectangle Resolution { get; }

        /// <summary>
        ///     The DPI scaling factor applied to this monitor.
        /// </summary>
        float ScalingFactor { get; }

        /// <summary>
        ///     Specifies the DPI-scaled work area rectangle of the display monitor,
        ///     expressed in virtual-screen coordinates. Note that if the monitor is not the
        ///     primary display monitor, some of the rectangle's coordinates may be negative
        ///     values.
        /// <para>
        ///     The work area is the portion of the screen not obscured by the system
        ///     taskbar or by application desktop toolbars.
        /// </para>
        /// </summary>
        Rectangle WorkAreaCoordinates { get; }
    }
}
