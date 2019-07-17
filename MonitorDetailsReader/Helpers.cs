using MonitorDetails.Models.Native;
using System.Drawing;
using System.Text.RegularExpressions;

namespace MonitorDetails
{
    public class Helpers
    {
        public static void FillDisplayDevice(Models.DisplayDevice model, Models.Native.DisplayDevice native)
        {
            model.Id = native.DeviceId;
            model.Key = native.DeviceKey;
            model.Name = native.DeviceName;
            model.State = native.StateFlags;
            model.Description = native.DeviceString;
        }

        public static string GetDeviceId(string instanceId)
        {
            return Regex.Match(instanceId, "(?<=\\\\)[^\\\\]*(?=\\\\)").Value;
        }

        public static RectangleF GetMonitorSizeFromEdid(byte[] edidData)
        {
            return new RectangleF(0f,
                                  0f,
                                  // Dividing by 10 because the width and height are originally in millimeters.
                                  (((edidData[68] & 0xf0) << 4) + edidData[66]) / 10f,
                                  (((edidData[68] & 0x0f) << 8) + edidData[67]) / 10f);
        }

        public static Rectangle GetRectangle(ref Rect rect)
        {
            return new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
        }
    }
}
