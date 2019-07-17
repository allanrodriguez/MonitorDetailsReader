using EDIDParser;
using MonitorDetails.Interfaces;
using System;

namespace MonitorDetails.Factories
{
    class EdidFactory : IEdidFactory
    {
        public EDID Create(byte[] edidData)
        {
            return new EDID(edidData ?? throw new ArgumentNullException(nameof(edidData)));
        }
    }
}
