namespace MDReader.Abstractions
{
    public interface IDisplayDevice
    {
        string Id { get; }

        string Key { get; }

        /// <summary>
        ///     Specifies the monitor or display adapter name.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Monitor or display adapter state flags.
        /// </summary>
        DeviceStateFlags StateFlags { get; }

        /// <summary>
        ///     A description of the monitor or display device.
        /// </summary>
        string String { get; }
    }
}
