using System;

namespace MDReader.Abstractions
{
    [Flags]
    public enum DeviceStateFlags : uint
    {
        /// <summary>
        ///     The device is part of the desktop.
        /// </summary>
        DISPLAY_DEVICE_ATTACHED_TO_DESKTOP = 1,

        /// <summary>
        ///     Represents a pseudo device used to mirror application drawing for remoting
        ///     or other purposes. An invisible pseudo monitor is associated with this
        ///     device.
        /// </summary>
        DISPLAY_DEVICE_MIRRORING_DRIVER = 8,

        /// <summary>
        ///     The device has more display modes than its output devices support.
        /// </summary>
        DISPLAY_DEVICE_MODESPRUNED = 134217728,

        /// <summary>
        ///     The device is part of the desktop.
        /// </summary>
        DISPLAY_DEVICE_MULTI_DRIVER = 2,

        /// <summary>
        ///     The device is part of the desktop.
        /// </summary>
        DISPLAY_DEVICE_PRIMARY_DEVICE = 4,

        /// <summary>
        ///     The device is removable; it cannot be the primary display.
        /// </summary>
        DISPLAY_DEVICE_REMOVABLE = 32,

        /// <summary>
        ///     The device is VGA compatible.
        /// </summary>
        DISPLAY_DEVICE_VGA_COMPATIBLE = 16,

        None = 0
    }
}
