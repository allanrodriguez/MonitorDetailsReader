using MDReader.Abstractions;

namespace MDReader
{
    struct DisplayDevice : IDisplayDevice
    {
        internal DisplayDevice(string id, string key, string name, DeviceStateFlags stateFlags, string stringParam)
        {
            Id = id;
            Key = key;
            Name = name;
            StateFlags = stateFlags;
            String = stringParam;
        }

        internal DisplayDevice(string name)
        {
            Id = string.Empty;
            Key = string.Empty;
            Name = name;
            StateFlags = DeviceStateFlags.None;
            String = string.Empty;
        }

        public string Id { get; }

        public string Key { get; }

        public string Name { get; }

        public DeviceStateFlags StateFlags { get; }

        public string String { get; }
    }
}
