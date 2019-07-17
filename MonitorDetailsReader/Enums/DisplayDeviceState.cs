using System;

namespace MonitorDetails.Enums
{
    [Flags]
    public enum DisplayDeviceState : uint
    {
        /// <summary>
        ///     The device is part of the desktop.
        /// </summary>
        AttachedToDesktop = 1,

        /// <summary>
        ///     Represents a pseudo device used to mirror application drawing for remoting
        ///     or other purposes. An invisible pseudo monitor is associated with this
        ///     device.
        /// </summary>
        MirroringDriver = 8,

        /// <summary>
        ///     The device has more display modes than its output devices support.
        /// </summary>
        ModesPruned = 134217728,

        /// <summary>
        ///     The device is part of the desktop.
        /// </summary>
        MultiDriver = 2,

        /// <summary>
        ///     The device is part of the desktop.
        /// </summary>
        PrimaryDevice = 4,

        /// <summary>
        ///     The device is removable; it cannot be the primary display.
        /// </summary>
        Removable = 32,

        /// <summary>
        ///     The device is VGA compatible.
        /// </summary>
        VgaCompatible = 16,

        None = 0
    }
}
