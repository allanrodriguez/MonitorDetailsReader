using MonitorDetails.Models;
using System.Collections.Generic;

namespace MonitorDetails.Interfaces
{
    /// <summary>
    ///     Retrieves current settings and useful information about the monitors connected to this computer.
    /// </summary>
    public interface IReader
    {
        /// <summary>
        ///     Creates an enumerable collection of <see cref="Monitor"/> objects containing information about all monitors connected to this
        ///     computer.
        /// </summary>
        /// <returns>
        ///     An enumerable collection of <see cref="Monitor"/> objects containing information about all monitors connected to this computer.
        /// </returns>
        IEnumerable<Monitor> GetMonitorDetails();
    }
}
