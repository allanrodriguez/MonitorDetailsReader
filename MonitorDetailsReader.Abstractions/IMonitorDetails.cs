using System;
using System.Collections.ObjectModel;

namespace MDReader.Abstractions
{
    /// <summary>
    ///     Stores details about a specific monitor connected to this computer.
    /// </summary>
    public interface IMonitorDetails
    {
        /// <summary>
        /// Specifies the width of the monitor in centimeters. This value has been parsed from the monitor's EDID.
        /// </summary>
        double WidthCentimeters { get; }

        /// <summary>
        /// Specifies the height of the monitor in centimeters. This value has been parsed from the monitor's EDID.
        /// </summary>
        double HeightCentimeters { get; }

        string DisplayAdapterId { get; }

        string DisplayAdapterKey { get; }

        /// <summary>
        ///     The adapter device name.
        /// </summary>
        string DisplayAdapterName { get; }

        /// <summary>
        ///     Adapter state flags.
        /// </summary>
        uint DisplayAdapterStateFlags { get; }

        /// <summary>
        ///     A description of the display adapter.
        /// </summary>
        string DisplayAdapterString { get; }

        string DisplayDeviceId { get; }
        
        string DisplayDeviceKey { get; }

        /// <summary>
        ///     The monitor device name.
        /// </summary>
        string DisplayDeviceName { get; }

        /// <summary>
        ///     Device state flags.
        /// </summary>
        uint DisplayDeviceStateFlags { get; }

        /// <summary>
        ///     A description of the display monitor.
        /// </summary>
        string DisplayDeviceString { get; }

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
        ///     If the monitor is the primary display monitor, this property is
        ///     <see langword="true"/>. If the monitor is not the primary display monitor,
        ///     this property is <see langword="false"/>.
        /// </summary>
        bool IsPrimaryMonitor { get; }

        /// <summary>
        /// A handle to the display monitor.
        /// </summary>
        IntPtr Handle { get; }

        /// <summary>
        ///     Specifies the device name of the monitor being used.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Specifies the display monitor rectangle, expressed in virtual-screen
        ///     coordinates. Note that if the monitor is not the primary display monitor,
        ///     some of the rectangle's coordinates may be negative values.
        /// </summary>
        RECT MonitorCoordinates { get; }

        /// <summary>
        ///     Specifies the DPI scaling factor applied to this monitor.
        /// </summary>
        double ScalingFactor { get; }

        /// <summary>
        ///     Specifies the width of the monitor in pixels regardless of DPI scaling.
        /// </summary>
        int WidthPhysicalPixels { get; }

        /// <summary>
        ///     Specifies the width of the monitor in pixels regardless of DPI scaling.
        /// </summary>
        int HeightPhysicalPixels { get; }

        /// <summary>
        ///     Specifies the width of the monitor in pixels after DPI scaling has taken
        ///     place.
        /// </summary>
        int WidthScaledPixels { get; }

        /// <summary>
        ///     Specifies the height of the monitor in pixels after DPI scaling has taken
        ///     place.
        /// </summary>
        int HeightScaledPixels { get; }

        /// <summary>
        ///     Specifies the width of the work area of the monitor in scaled pixels.
        /// <para>
        ///     The work area is the portion of the screen not obscured by the system
        ///     taskbar or by application desktop toolbars.
        /// </para>
        /// </summary>
        int WorkAreaWidth { get; }

        /// <summary>
        ///     Specifies the height of the work area of the monitor in scaled pixels.
        /// <para>
        ///     The work area is the portion of the screen not obscured by the system
        ///     taskbar or by application desktop toolbars.
        /// </para>
        /// </summary>
        int WorkAreaHeight { get; }

        /// <summary>
        ///     Specifies the work area rectangle of the display monitor, expressed in
        ///     virtual-screen coordinates. Note that if the monitor is not the primary
        ///     display monitor, some of the rectangle's coordinates may be negative values.
        /// <para>
        ///     The work area is the portion of the screen not obscured by the system
        ///     taskbar or by application desktop toolbars.
        /// </para>
        /// </summary>
        RECT WorkAreaCoordinates { get; }
    }
}
