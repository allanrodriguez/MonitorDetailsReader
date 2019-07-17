namespace MonitorDetails.Interfaces
{
    public interface IMarshal
    {
        int GetLastWin32Error();

        int SizeOf<T>() where T : struct;
    }
}
