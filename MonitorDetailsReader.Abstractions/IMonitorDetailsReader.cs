using System.Collections.ObjectModel;

namespace MDReader.Abstractions
{
    /// <summary>
    ///     Retrieves current settings and useful information about the monitors
    ///     connected to this computer.
    /// </summary>
    public interface IMonitorDetailsReader
    {
        /// <summary>
        ///     Creates a read-only collection of <see cref="IMonitorDetails"/> objects
        ///     containing information about all monitors connected to this computer.
        /// </summary>
        /// <returns>
        ///     A read-only collection of <see cref="IMonitorDetails"/> objects containing
        ///     information about all monitors connected to this computer.
        /// </returns>
        ReadOnlyCollection<IMonitorDetails> GetMonitorDetails();
    }
}
