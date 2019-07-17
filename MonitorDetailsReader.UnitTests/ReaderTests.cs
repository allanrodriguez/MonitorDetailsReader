using MonitorDetails.Interfaces;
using MonitorDetails.Models.Native;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace MonitorDetails.UnitTests
{
    public class ReaderTests
    {
        readonly Mock<IMarshal> _marshal;
        readonly Mock<INativeMethods> _nativeMethods;
        readonly Mock<IEdidFactory> _edidFactory;

        readonly Reader _reader;

        public ReaderTests()
        {
            _marshal = new Mock<IMarshal>();
            _nativeMethods = new Mock<INativeMethods>();
            _edidFactory = new Mock<IEdidFactory>();

            var rect = new Rect();
            const string adapterName = "Adapter";
            const string monitorName = "Monitor";
            const string monitorId = "a\\Id\\c\\d";
            const string instanceId = "Id";

            _marshal
                .Setup(a => a.GetLastWin32Error())
                .Returns(() => _marshal.Invocations.Count(a => a.Method.Name == nameof(IMarshal.GetLastWin32Error)) > 1
                    ? NativeConstants.ERROR_NO_MORE_ITEMS
                    : 0);

            _nativeMethods
                .Setup(
                    a => a.EnumDisplayMonitors(It.IsAny<IntPtr>(), It.IsAny<IntPtr>(), It.IsAny<MonitorEnumDelegate>(),
                        It.IsAny<IntPtr>()))
                .Callback<IntPtr, IntPtr, MonitorEnumDelegate, IntPtr>(
                    (a, b, c, d) => c(new IntPtr(1), IntPtr.Zero, ref rect, IntPtr.Zero))
                .Returns(true);

            _nativeMethods
                .Setup(
                    a => a.EnumDisplayDevices(null, It.IsAny<uint>(), ref It.Ref<DisplayDevice>.IsAny,
                        It.IsAny<uint>()))
                .Callback(new EnumDisplayDevicesDelegate((string a, uint b, ref DisplayDevice c, uint d) =>
                    c.DeviceName = adapterName))
                .Returns<string, uint, DisplayDevice, uint>((a, b, c, d) => b == 0);

            _nativeMethods
                .Setup(
                    a => a.EnumDisplayDevices(adapterName, It.IsAny<uint>(), ref It.Ref<DisplayDevice>.IsAny,
                        It.IsAny<uint>()))
                .Callback(new EnumDisplayDevicesDelegate((string a, uint b, ref DisplayDevice c, uint d) =>
                {
                    c.DeviceId = monitorId;
                    c.DeviceName = monitorName;
                }))
                .Returns<string, uint, DisplayDevice, uint>((a, b, c, d) => b == 0);

            _nativeMethods
                .Setup(
                    a => a.SetupDiEnumDeviceInfo(It.IsAny<IntPtr>(), It.IsAny<uint>(),
                        ref It.Ref<SpDevInfoData>.IsAny))
                .Returns(true);

            _nativeMethods
                .Setup(a => a.GetMonitorInfo(It.IsAny<IntPtr>(), ref It.Ref<MonitorInfoEx>.IsAny))
                .Callback(new GetMonitorInfoDelegate((IntPtr a, ref MonitorInfoEx b) => b = new MonitorInfoEx
                {
                    Device = adapterName,
                    Flags = NativeConstants.MONITORINFOF_PRIMARY,
                    Monitor = new Rect
                    {
                        Right = 10,
                        Bottom = 10
                    }
                }))
                .Returns(true);

            _nativeMethods
                .Setup(a => a.EnumDisplaySettings(It.IsAny<string>(), It.IsAny<uint>(), ref It.Ref<DevMode>.IsAny))
                .Callback(new EnumDisplaySettingsDelegate((string a, uint b, ref DevMode c) => c = new DevMode
                {
                    DisplayFrequency = 1,
                    PelsWidth = 20,
                    PelsHeight = 20
                }))
                .Returns(true);

            _nativeMethods
                .Setup(a => a.SetupDiGetClassDevsEx(It.IsAny<Guid[]>(), It.IsAny<string>(), It.IsAny<IntPtr>(),
                    It.IsAny<uint>(), It.IsAny<IntPtr>(), It.IsAny<string>(), It.IsAny<IntPtr>()))
                .Returns(new IntPtr(1));

            _nativeMethods
                .Setup(a => a.SetupDiGetDeviceInstanceId(It.IsAny<IntPtr>(), ref It.Ref<SpDevInfoData>.IsAny,
                        It.IsAny<char[]>(), It.IsAny<uint>(), It.IsAny<IntPtr>()))
                .Callback(new SetupDiGetDeviceInstanceIdDelegate(
                    (IntPtr a, ref SpDevInfoData b, char[] c, uint d, IntPtr e) =>
                        Array.Copy(instanceId.ToCharArray(), 0, c, 0, instanceId.Length)))
                .Returns(true);

            _nativeMethods
                .Setup(a => a.SetupDiOpenDevRegKey(It.IsAny<IntPtr>(), ref It.Ref<SpDevInfoData>.IsAny,
                    It.IsAny<uint>(), It.IsAny<uint>(), It.IsAny<uint>(), It.IsAny<uint>()))
                .Returns(IntPtr.Zero);

            _reader = new Reader(_marshal.Object, _nativeMethods.Object, _edidFactory.Object);
        }

        public delegate void EnumDisplayDevicesDelegate(string lpDevice, uint iDevNum,
            ref DisplayDevice lpDisplayDevice, uint dwFlags);

        public delegate void GetMonitorInfoDelegate(IntPtr hMonitor, ref MonitorInfoEx lpmi);

        public delegate void EnumDisplaySettingsDelegate(string deviceName, uint modeNum, ref DevMode devMode);

        public delegate void SetupDiGetDeviceInstanceIdDelegate(IntPtr deviceInfoSet, ref SpDevInfoData deviceInfoData, char[] deviceInstanceId,
            uint deviceInstanceIdSize, IntPtr requiredSize);

        [Fact]
        public void DefaultConstructorTest()
        {
            var reader = new Reader();

            Assert.IsType<Reader>(reader);
        }

        [Fact]
        public void ReaderThrowsArgumentNullExceptionTest()
        {
            Assert.Throws<ArgumentNullException>(() => new Reader(null, null, null));
        }

        [Fact]
        public void GetMonitorDetailsCallsEnumDisplayMonitorsTest()
        {
            _reader.GetMonitorDetails();

            _nativeMethods.Verify(
                a => a.EnumDisplayMonitors(It.IsAny<IntPtr>(), It.IsAny<IntPtr>(), It.IsAny<MonitorEnumDelegate>(),
                    It.IsAny<IntPtr>()), Times.Once);
        }

        [Fact]
        public void GetMonitorDetailsCallsGetMonitorInfoTest()
        {
            _reader.GetMonitorDetails();

            _nativeMethods.Verify(a => a.GetMonitorInfo(It.IsAny<IntPtr>(), ref It.Ref<MonitorInfoEx>.IsAny),
                Times.Once);
        }

        [Fact]
        public void GetMonitorDetailsGetMonitorInfoFalseTest()
        {
            _nativeMethods
                .Setup(a => a.GetMonitorInfo(It.IsAny<IntPtr>(), ref It.Ref<MonitorInfoEx>.IsAny))
                .Returns(false);

            var result = _reader.GetMonitorDetails();

            Assert.Empty(result);
        }

        [Fact]
        public void GetMonitorDetailsIsPrimaryMonitorTrueTest()
        {
            var result = _reader.GetMonitorDetails().FirstOrDefault();

            Assert.True(result.IsPrimaryMonitor);
        }

        [Fact]
        public void GetMonitorDetailsDisplayAdapterTest()
        {
            const string expectedDisplayDeviceName = "Adapter";

            var result = _reader.GetMonitorDetails().FirstOrDefault();

            Assert.Equal(expectedDisplayDeviceName, result.DisplayAdapter.Name);
        }

        [Fact]
        public void GetMonitorDetailsEnumDisplayMonitorsFalseTest()
        {
            _nativeMethods
                .Setup(
                    a => a.EnumDisplayMonitors(It.IsAny<IntPtr>(), It.IsAny<IntPtr>(), It.IsAny<MonitorEnumDelegate>(),
                        It.IsAny<IntPtr>()))
                .Returns(false);

            _reader.GetMonitorDetails();

            _nativeMethods
                .Verify(
                    a => a.EnumDisplayDevices(It.IsAny<string>(), It.IsAny<uint>(), ref It.Ref<DisplayDevice>.IsAny,
                        It.IsAny<uint>()),
                    Times.Never);
        }

        [Fact]
        public void GetMonitorDetailsCallsEnumDisplayDevicesForDisplayTest()
        {
            _reader.GetMonitorDetails();

            _nativeMethods
                .Verify(
                    a => a.EnumDisplayDevices(null, It.IsAny<uint>(), ref It.Ref<DisplayDevice>.IsAny,
                        It.IsAny<uint>()), Times.Exactly(2));
        }

        [Fact]
        public void GetMonitorDetailsCallsEnumDisplayDevicesForMonitorTest()
        {
            const string deviceName = "Adapter";

            _reader.GetMonitorDetails();

            _nativeMethods
                .Verify(
                    a => a.EnumDisplayDevices(deviceName, It.IsAny<uint>(), ref It.Ref<DisplayDevice>.IsAny,
                        It.IsAny<uint>()), Times.Exactly(2));
        }

        [Fact]
        public void GetMonitorDetailsCallsEnumDisplaySettingsTest()
        {
            _reader.GetMonitorDetails();

            _nativeMethods
                .Verify(
                    a => a.EnumDisplaySettings(It.IsAny<string>(), It.IsAny<uint>(), ref It.Ref<DevMode>.IsAny),
                        Times.Once);
        }

        [Fact]
        public void GetMonitorDetailsEnumDisplaySettingsReturnsFalseTest()
        {
            const int expectedFrequency = 0;
            _nativeMethods
                .Setup(a => a.EnumDisplaySettings(It.IsAny<string>(), It.IsAny<uint>(), ref It.Ref<DevMode>.IsAny))
                .Callback(new EnumDisplaySettingsDelegate((string a, uint b, ref DevMode c) => c.DisplayFrequency = 1))
                .Returns(false);

            var result = _reader.GetMonitorDetails().FirstOrDefault();

            Assert.Equal(expectedFrequency, result.Frequency);
        }

        [Fact]
        public void GetMonitorDetailsFrequencyTest()
        {
            const int expectedFrequency = 1;

            var result = _reader.GetMonitorDetails().FirstOrDefault();

            Assert.Equal(expectedFrequency, result.Frequency);
        }

        [Fact]
        public void GetMonitorDetailsResolutionWidthTest()
        {
            const int expectedWidth = 20;

            var result = _reader.GetMonitorDetails().FirstOrDefault();

            Assert.Equal(expectedWidth, result.Resolution.Width);
        }

        [Fact]
        public void GetMonitorDetailsResolutionHeightTest()
        {
            const int expectedHeight = 20;

            var result = _reader.GetMonitorDetails().FirstOrDefault();

            Assert.Equal(expectedHeight, result.Resolution.Height);
        }

        [Fact]
        public void GetMonitorDetailsScalingFactorTest()
        {
            const float expectedScalingFactor = 2f;

            var result = _reader.GetMonitorDetails().FirstOrDefault();

            Assert.Equal(expectedScalingFactor, result.ScalingFactor);
        }

        [Fact]
        public void GetMonitorDetailsDpiTest()
        {
            const int expectedDpi = 192;

            var result = _reader.GetMonitorDetails().FirstOrDefault();

            Assert.Equal(expectedDpi, result.Dpi);
        }

        [Fact]
        public void GetMonitorDetailsCallsSetupDiGetClassDevsExTest()
        {
            _reader.GetMonitorDetails();

            _nativeMethods
                .Verify(
                    a => a.SetupDiGetClassDevsEx(It.IsAny<Guid[]>(), It.IsAny<string>(), It.IsAny<IntPtr>(),
                        It.IsAny<uint>(), It.IsAny<IntPtr>(), It.IsAny<string>(), It.IsAny<IntPtr>()), Times.Once);
        }

        [Fact]
        public void GetMonitorDetailsSetupDiGetClassDevsExReturnsZeroTest()
        {
            _nativeMethods
                .Setup(a => a.SetupDiGetClassDevsEx(It.IsAny<Guid[]>(), It.IsAny<string>(), It.IsAny<IntPtr>(),
                    It.IsAny<uint>(), It.IsAny<IntPtr>(), It.IsAny<string>(), It.IsAny<IntPtr>()))
                .Returns(IntPtr.Zero);

            _reader.GetMonitorDetails();

            _nativeMethods
                .Verify(
                    a => a.SetupDiEnumDeviceInfo(It.IsAny<IntPtr>(), It.IsAny<uint>(),
                        ref It.Ref<SpDevInfoData>.IsAny), Times.Never);
        }

        [Fact]
        public void GetMonitorDetailsCallsSetupDiEnumDeviceInfoTest()
        {
            _reader.GetMonitorDetails();

            _nativeMethods
                .Verify(
                    a => a.SetupDiEnumDeviceInfo(It.IsAny<IntPtr>(), It.IsAny<uint>(),
                        ref It.Ref<SpDevInfoData>.IsAny), Times.Once);
        }

        [Fact]
        public void GetMonitorDetailsSetupDiEnumDeviceInfoReturnsFalseTest()
        {
            _nativeMethods
                .Setup(a => a.SetupDiEnumDeviceInfo(It.IsAny<IntPtr>(), It.IsAny<uint>(),
                    ref It.Ref<SpDevInfoData>.IsAny))
                .Returns(false);

            _reader.GetMonitorDetails();

            _nativeMethods
                .Verify(
                    a => a.SetupDiGetDeviceInstanceId(It.IsAny<IntPtr>(), ref It.Ref<SpDevInfoData>.IsAny,
                        It.IsAny<char[]>(), It.IsAny<uint>(), It.IsAny<IntPtr>()), Times.Never);
        }

        [Fact]
        public void GetMonitorDetailsCallsSetupDiGetDeviceInstanceIdTest()
        {
            _reader.GetMonitorDetails();

            _nativeMethods
                .Verify(
                    a => a.SetupDiGetDeviceInstanceId(It.IsAny<IntPtr>(), ref It.Ref<SpDevInfoData>.IsAny,
                        It.IsAny<char[]>(), It.IsAny<uint>(), It.IsAny<IntPtr>()), Times.Once);
        }

        [Fact]
        public void GetMonitorDetailsSetupDiGetDeviceInstanceIdReturnsFalseTest()
        {
            _nativeMethods
                .Setup(a => a.SetupDiGetDeviceInstanceId(It.IsAny<IntPtr>(), ref It.Ref<SpDevInfoData>.IsAny,
                    It.IsAny<char[]>(), It.IsAny<uint>(), It.IsAny<IntPtr>()))
                .Returns(false);

            _reader.GetMonitorDetails();

            _nativeMethods
                .Verify(
                    a => a.SetupDiOpenDevRegKey(It.IsAny<IntPtr>(), ref It.Ref<SpDevInfoData>.IsAny, It.IsAny<uint>(),
                        It.IsAny<uint>(), It.IsAny<uint>(), It.IsAny<uint>()), Times.Never);
        }

        [Fact]
        public void GetMonitorDetailsMonitorNotFoundTest()
        {
            const string instanceId = "Test";
            _nativeMethods
                .Setup(a => a.SetupDiGetDeviceInstanceId(It.IsAny<IntPtr>(), ref It.Ref<SpDevInfoData>.IsAny,
                    It.IsAny<char[]>(), It.IsAny<uint>(), It.IsAny<IntPtr>()))
                .Callback(new SetupDiGetDeviceInstanceIdDelegate(
                    (IntPtr a, ref SpDevInfoData b, char[] c, uint d, IntPtr e) =>
                        Array.Copy(instanceId.ToCharArray(), 0, c, 0, instanceId.Length)))
                .Returns(true);

            _reader.GetMonitorDetails();

            _nativeMethods
                .Verify(
                    a => a.SetupDiOpenDevRegKey(It.IsAny<IntPtr>(), ref It.Ref<SpDevInfoData>.IsAny, It.IsAny<uint>(),
                        It.IsAny<uint>(), It.IsAny<uint>(), It.IsAny<uint>()), Times.Never);
        }

        [Fact]
        public void GetMonitorDetailsCallsSetupDiOpenDevRegKeyTest()
        {
            _reader.GetMonitorDetails();

            _nativeMethods
                .Verify(
                    a => a.SetupDiOpenDevRegKey(It.IsAny<IntPtr>(), ref It.Ref<SpDevInfoData>.IsAny, It.IsAny<uint>(),
                        It.IsAny<uint>(), It.IsAny<uint>(), It.IsAny<uint>()), Times.Once);
        }

        [Fact]
        public void GetMonitorDetailsSetupDiOpenDevRegKeyReturnsInvalidHandleTest()
        {
            _nativeMethods
                .Setup(a => a.SetupDiOpenDevRegKey(It.IsAny<IntPtr>(), ref It.Ref<SpDevInfoData>.IsAny,
                    It.IsAny<uint>(), It.IsAny<uint>(), It.IsAny<uint>(), It.IsAny<uint>()))
                .Returns(NativeConstants.INVALID_HANDLE_VALUE);

            _reader.GetMonitorDetails();

            _nativeMethods
                .Verify(
                    a => a.RegQueryValueEx(It.IsAny<IntPtr>(), It.IsAny<string>(), It.IsAny<IntPtr>(),
                        It.IsAny<IntPtr>(), It.IsAny<byte[]>(), ref It.Ref<uint>.IsAny), Times.Never);
        }

        [Fact]
        public void GetMonitorDetailsCallsRegQueryValueExTest()
        {
            _reader.GetMonitorDetails();

            _nativeMethods
                .Verify(
                    a => a.RegQueryValueEx(It.IsAny<IntPtr>(), It.IsAny<string>(), It.IsAny<IntPtr>(),
                        It.IsAny<IntPtr>(), It.IsAny<byte[]>(), ref It.Ref<uint>.IsAny), Times.Once);
        }

        [Fact]
        public void GetMonitorDetailsCallsRegCloseKeyTest()
        {
            _reader.GetMonitorDetails();

            _nativeMethods.Verify(a => a.RegCloseKey(It.IsAny<IntPtr>()), Times.Once);
        }

        [Fact]
        public void GetMonitorDetailsCallsSetupDiDestroyDeviceInfoListTest()
        {
            _reader.GetMonitorDetails();

            _nativeMethods.Verify(a => a.SetupDiDestroyDeviceInfoList(It.IsAny<IntPtr>()), Times.Once);
        }
    }
}
