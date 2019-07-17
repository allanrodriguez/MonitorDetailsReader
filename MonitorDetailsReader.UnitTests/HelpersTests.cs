using MonitorDetails.Enums;
using MonitorDetails.Models.Native;
using Xunit;

namespace MonitorDetails.UnitTests
{
    public class HelpersTests
    {
        [Fact]
        public void FillDisplayDeviceIdTest()
        {
            var nativeModel = new DisplayDevice { DeviceId = "Id" };
            var model = new MonitorDetails.Models.DisplayDevice();

            Helpers.FillDisplayDevice(model, nativeModel);

            Assert.Equal(nativeModel.DeviceId, model.Id);
        }

        [Fact]
        public void FillDisplayDeviceKeyTest()
        {
            var nativeModel = new DisplayDevice { DeviceKey = "Key" };
            var model = new MonitorDetails.Models.DisplayDevice();

            Helpers.FillDisplayDevice(model, nativeModel);

            Assert.Equal(nativeModel.DeviceKey, model.Key);
        }

        [Fact]
        public void FillDisplayDeviceNameTest()
        {
            var nativeModel = new DisplayDevice { DeviceName = "Name" };
            var model = new MonitorDetails.Models.DisplayDevice();

            Helpers.FillDisplayDevice(model, nativeModel);

            Assert.Equal(nativeModel.DeviceName, model.Name);
        }

        [Fact]
        public void FillDisplayDeviceDescriptionTest()
        {
            var nativeModel = new DisplayDevice { DeviceString = "String" };
            var model = new MonitorDetails.Models.DisplayDevice();

            Helpers.FillDisplayDevice(model, nativeModel);

            Assert.Equal(nativeModel.DeviceString, model.Description);
        }

        [Fact]
        public void FillDisplayDeviceStateTest()
        {
            var nativeModel = new DisplayDevice { StateFlags = DisplayDeviceState.PrimaryDevice };
            var model = new MonitorDetails.Models.DisplayDevice();

            Helpers.FillDisplayDevice(model, nativeModel);

            Assert.Equal(nativeModel.StateFlags, model.State);
        }

        [Fact]
        public void GetDeviceIdTest()
        {
            const string instanceId = @"a\id\c\d";
            const string expectedDeviceId = "id";

            var result = Helpers.GetDeviceId(instanceId);

            Assert.Equal(expectedDeviceId, result);
        }

        [Fact]
        public void GetMonitorSizeFromEdidXTest()
        {
            const float expectedX = 0f;
            var edid = new byte[128];

            var result = Helpers.GetMonitorSizeFromEdid(edid);

            Assert.Equal(expectedX, result.X);
        }

        [Fact]
        public void GetMonitorSizeFromEdidYTest()
        {
            const float expectedY = 0f;
            var edid = new byte[128];

            var result = Helpers.GetMonitorSizeFromEdid(edid);

            Assert.Equal(expectedY, result.Y);
        }

        [Fact]
        public void GetMonitorSizeFromEdidWidthTest()
        {
            const float expectedWidth = 16f;
            var edid = new byte[128];
            edid[66] = 160;

            var result = Helpers.GetMonitorSizeFromEdid(edid);

            Assert.Equal(expectedWidth, result.Width);
        }

        [Fact]
        public void GetMonitorSizeFromEdidHeightTest()
        {
            const float expectedHeight = 9f;
            var edid = new byte[128];
            edid[67] = 90;

            var result = Helpers.GetMonitorSizeFromEdid(edid);

            Assert.Equal(expectedHeight, result.Height);
        }

        [Fact]
        public void GetRectangleXTest()
        {
            var expectedX = 5;
            var rect = new Rect { Left = 5 };

            var result = Helpers.GetRectangle(ref rect);

            Assert.Equal(expectedX, result.X);
        }

        [Fact]
        public void GetRectangleYTest()
        {
            var expectedY = 5;
            var rect = new Rect { Top = 5 };

            var result = Helpers.GetRectangle(ref rect);

            Assert.Equal(expectedY, result.Y);
        }

        [Fact]
        public void GetRectangleWidthTest()
        {
            var expectedWidth = 5;
            var rect = new Rect
            {
                Left = 5,
                Right = 10
            };

            var result = Helpers.GetRectangle(ref rect);

            Assert.Equal(expectedWidth, result.Width);
        }

        [Fact]
        public void GetRectangleHeightTest()
        {
            var expectedHeight = 5;
            var rect = new Rect
            {
                Top = 5,
                Bottom = 10
            };

            var result = Helpers.GetRectangle(ref rect);

            Assert.Equal(expectedHeight, result.Height);
        }
    }
}
