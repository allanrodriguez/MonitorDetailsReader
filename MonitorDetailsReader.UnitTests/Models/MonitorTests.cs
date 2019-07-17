using MonitorDetails.Models;
using System.Drawing;
using Xunit;

namespace MonitorDetails.UnitTests.Models
{
    public class MonitorTests
    {
        readonly Monitor _monitor;

        public MonitorTests()
        {
            _monitor = new Monitor();
        }

        [Fact]
        public void ToStringTest()
        {
            const string description = "Monitor";
            const int pixelWidth = 1920;
            const int pixelHeight = 1080;
            _monitor.Description = description;
            _monitor.MonitorCoordinates = new Rectangle(0, 0, pixelWidth, pixelHeight);

            var result = _monitor.ToString();

            Assert.Equal($"{description} ({pixelWidth}x{pixelHeight})", result);
        }

        [Fact]
        public void ToStringPrimaryMonitorTest()
        {
            const string description = "Monitor";
            const int pixelWidth = 1920;
            const int pixelHeight = 1080;
            const bool isPrimaryMonitor = true;
            _monitor.Description = description;
            _monitor.IsPrimaryMonitor = isPrimaryMonitor;
            _monitor.MonitorCoordinates = new Rectangle(0, 0, pixelWidth, pixelHeight);

            var result = _monitor.ToString();

            Assert.Equal($"{description} (Primary, {pixelWidth}x{pixelHeight})", result);
        }
    }
}
