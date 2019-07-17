using MonitorDetails.Interfaces;
using System.Runtime.InteropServices;

namespace MonitorDetails.Wrappers
{
    class MarshalWrapper : IMarshal
    {
        public int GetLastWin32Error()
        {
            return Marshal.GetLastWin32Error();
        }

        public int SizeOf<T>() where T : struct
        {
            return Marshal.SizeOf<T>();
        }
    }
}
