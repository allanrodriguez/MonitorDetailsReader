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
                Console.WriteLine($"DisplayAdapterId:         {monitor.DisplayAdapterId}");
                Console.WriteLine($"DisplayAdapterKey:        {monitor.DisplayAdapterKey}");
                Console.WriteLine($"DisplayAdapterName:       {monitor.DisplayAdapterName}");
                Console.WriteLine($"DisplayAdapterStateFlags: {monitor.DisplayAdapterStateFlags}");
                Console.WriteLine($"DisplayAdapterString:     {monitor.DisplayAdapterString}");
                Console.WriteLine();
                Console.WriteLine($"DisplayDeviceId:          {monitor.DisplayDeviceId}");
                Console.WriteLine($"DisplayDeviceKey:         {monitor.DisplayDeviceKey}");
                Console.WriteLine($"DisplayDeviceName:        {monitor.DisplayDeviceName}");
                Console.WriteLine($"DisplayDeviceStateFlags:  {monitor.DisplayDeviceStateFlags}");
                Console.WriteLine($"DisplayDeviceString:      {monitor.DisplayDeviceString}");
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
                Console.WriteLine($"WidthCentimeter:          {monitor.WidthCentimeters}");
                Console.WriteLine($"HeightCentimeters:        {monitor.HeightCentimeters}");
                Console.WriteLine();
                Console.WriteLine($"WidthPhysicalPixels:      {monitor.WidthPhysicalPixels}");
                Console.WriteLine($"HeightPhysicalPixels:     {monitor.HeightPhysicalPixels}");
                Console.WriteLine();
                Console.WriteLine($"WidthScaledPixels:        {monitor.WidthScaledPixels}");
                Console.WriteLine($"HeightScaledPixels:       {monitor.HeightScaledPixels}");
                Console.WriteLine();
                Console.WriteLine($"WorkAreaWidth:            {monitor.WorkAreaWidth}");
                Console.WriteLine($"WorkAreaHeight:           {monitor.WorkAreaHeight}");
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
