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
                Console.WriteLine($"Display adapter ID:                 {monitor.DisplayAdapter.Id}");
                Console.WriteLine($"Display adapter key:                {monitor.DisplayAdapter.Key}");
                Console.WriteLine($"Display adapter name:               {monitor.DisplayAdapter.Name}");
                Console.WriteLine($"Display adapter state flags:        {monitor.DisplayAdapter.StateFlags}");
                Console.WriteLine($"Display adapter description string: {monitor.DisplayAdapter.String}");
                Console.WriteLine();
                Console.WriteLine($"ID:                                 {monitor.Id}");
                Console.WriteLine($"Key:                                {monitor.Key}");
                Console.WriteLine($"Name:                               {monitor.Name}");
                Console.WriteLine($"State flags:                        {monitor.StateFlags}");
                Console.WriteLine($"Description string:                 {monitor.String}");
                Console.WriteLine();
                Console.WriteLine($"DPI:                                {monitor.Dpi}");
                Console.WriteLine();
                Console.WriteLine($"Frequency:                          {monitor.Frequency}");
                Console.WriteLine();
                Console.WriteLine($"Handle:                             {monitor.Handle}");
                Console.WriteLine();
                Console.WriteLine($"IsPrimaryMonitor:                   {monitor.IsPrimaryMonitor}");
                Console.WriteLine();
                Console.WriteLine($"Name:                               {monitor.Name}");
                Console.WriteLine();
                Console.WriteLine($"ScalingFactor:                      {monitor.ScalingFactor}");
                Console.WriteLine();
                Console.WriteLine($"Width (cm.):                        {monitor.Dimensions.Width}");
                Console.WriteLine($"Height (cm.):                       {monitor.Dimensions.Height}");
                Console.WriteLine();
                Console.WriteLine($"Width (pixels):                     {monitor.Resolution.Width}");
                Console.WriteLine($"Height (pixels):                    {monitor.Resolution.Height}");
                Console.WriteLine();
                Console.WriteLine($"Width (scaled pixels):              {monitor.MonitorCoordinates.Width}");
                Console.WriteLine($"Height (scaled pixels):             {monitor.MonitorCoordinates.Height}");
                Console.WriteLine();
                Console.WriteLine($"Work area width (scaled pixels):    {monitor.WorkAreaCoordinates.Width}");
                Console.WriteLine($"Work area height (scaled pixels):   {monitor.WorkAreaCoordinates.Height}");
                Console.WriteLine();
                Console.Write("Edid:                               ");
                foreach (byte b in monitor.Edid)
                {
                    Console.Write(b + " ");
                }

                Console.WriteLine("\n");
            }
        }
    }
}
