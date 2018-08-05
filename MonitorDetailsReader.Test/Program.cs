using MDReader.Abstractions;
using System;
using System.Collections.Generic;

namespace MDReader.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            IMonitorDetailsReader monitorDetailsReader = new MonitorDetailsReader();
            IList<IMonitorDetails> monitorDetails = monitorDetailsReader.GetMonitorDetails();

            int i = 0;

            foreach (var monitor in monitorDetails)
            {
                ++i;
                Console.WriteLine($"Monitor {i}");
                Console.WriteLine();
                Console.WriteLine($"DisplayAdapterId:         {monitor.DisplayAdapter.Id}");
                Console.WriteLine($"DisplayAdapterKey:        {monitor.DisplayAdapter.Key}");
                Console.WriteLine($"DisplayAdapterName:       {monitor.DisplayAdapter.Name}");
                Console.WriteLine($"DisplayAdapterStateFlags: {monitor.DisplayAdapter.StateFlags}");
                Console.WriteLine($"DisplayAdapterString:     {monitor.DisplayAdapter.String}");
                Console.WriteLine();
                Console.WriteLine($"DisplayDeviceId:          {monitor.Id}");
                Console.WriteLine($"DisplayDeviceKey:         {monitor.Key}");
                Console.WriteLine($"DisplayDeviceName:        {monitor.Name}");
                Console.WriteLine($"DisplayDeviceStateFlags:  {monitor.StateFlags}");
                Console.WriteLine($"DisplayDeviceString:      {monitor.String}");
                Console.WriteLine();
                Console.WriteLine($"Dpi:                      {monitor.Dpi}");
                Console.WriteLine();
                Console.WriteLine($"Frequency:                {monitor.Frequency}");
                Console.WriteLine();
                Console.WriteLine($"Handle:                   {monitor.Handle}");
                Console.WriteLine();
                Console.WriteLine($"IsPrimaryMonitor:         {monitor.IsPrimaryMonitor}");
                Console.WriteLine();
                Console.WriteLine($"Name:                     {monitor.Name}");
                Console.WriteLine();
                Console.WriteLine($"ScalingFactor:            {monitor.ScalingFactor}");
                Console.WriteLine();
                Console.WriteLine($"WidthCentimeters:         {monitor.Dimensions.Width}");
                Console.WriteLine($"HeightCentimeters:        {monitor.Dimensions.Height}");
                Console.WriteLine();
                Console.WriteLine($"WidthPhysicalPixels:      {monitor.Resolution.Width}");
                Console.WriteLine($"HeightPhysicalPixels:     {monitor.Resolution.Height}");
                Console.WriteLine();
                Console.WriteLine($"WidthScaledPixels:        {monitor.MonitorCoordinates.Width}");
                Console.WriteLine($"HeightScaledPixels:       {monitor.MonitorCoordinates.Height}");
                Console.WriteLine();
                Console.WriteLine($"WorkAreaWidth:            {monitor.WorkAreaCoordinates.Width}");
                Console.WriteLine($"WorkAreaHeight:           {monitor.WorkAreaCoordinates.Height}");
                Console.WriteLine();
                Console.Write("Edid: ");
                foreach (byte b in monitor.Edid)
                {
                    Console.Write(b + " ");
                }

                Console.WriteLine("\n");
            }
        }
    }
}
