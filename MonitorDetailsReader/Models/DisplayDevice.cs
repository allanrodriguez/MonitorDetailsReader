using MonitorDetails.Enums;

namespace MonitorDetails.Models
{
    /// <summary>
    ///     Stores basic information about a display device, such as a monitor or adapter.
    /// </summary>
    public class DisplayDevice
    {
        public string Id { get; set; }

        public string Key { get; set; }

        public string Name { get; set; }

        public DisplayDeviceState State { get; set; }

        public string Description { get; set; }
    }
}
