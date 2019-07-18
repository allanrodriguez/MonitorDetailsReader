using EDIDParser;

namespace MonitorDetails.Interfaces
{
    public interface IEdidFactory
    {
        EDID Create(byte[] edidData);
    }
}
