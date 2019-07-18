using EDIDParser;
using EDIDParser.Descriptors;
using EDIDParser.Enums;
using EDIDParser.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonitorDetails.Test
{
    class Program
    {
        static void Main()
        {
            var monitorDetailsReader = new Reader();
            var monitorDetails = monitorDetailsReader.GetMonitorDetails();

            var i = 0;
            var descriptorNo = 0;
            var extensionNo = 0;

            foreach (var monitor in monitorDetails)
            {
                ++i;
                Console.WriteLine("***********************");
                Console.WriteLine($"****** Monitor {i} ******");
                Console.WriteLine("***********************");
                Console.WriteLine();
                Console.WriteLine($"Display adapter ID:                                  {monitor.DisplayAdapter.Id}");
                Console.WriteLine($"Display adapter key:                                 {monitor.DisplayAdapter.Key}");
                Console.WriteLine($"Display adapter name:                                {monitor.DisplayAdapter.Name}");
                Console.WriteLine($"Display adapter state flags:                         {monitor.DisplayAdapter.State}");
                Console.WriteLine($"Display adapter description:                         {monitor.DisplayAdapter.Description}");
                Console.WriteLine();
                Console.WriteLine($"ID:                                                  {monitor.Id}");
                Console.WriteLine($"Key:                                                 {monitor.Key}");
                Console.WriteLine($"Name:                                                {monitor.Name}");
                Console.WriteLine($"State flags:                                         {monitor.State}");
                Console.WriteLine($"Description:                                         {monitor.Description}");
                Console.WriteLine();
                Console.WriteLine($"DPI:                                                 {monitor.Dpi}");
                Console.WriteLine();
                Console.WriteLine($"Frequency:                                           {monitor.Frequency}");
                Console.WriteLine();
                Console.WriteLine($"Handle:                                              {monitor.Handle}");
                Console.WriteLine();
                Console.WriteLine($"Is primary monitor?                                  {monitor.IsPrimaryMonitor}");
                Console.WriteLine();
                Console.WriteLine($"Scaling factor:                                      {monitor.ScalingFactor}");
                Console.WriteLine();
                Console.WriteLine($"Width (cm.):                                         {monitor.Dimensions.Width}");
                Console.WriteLine($"Height (cm.):                                        {monitor.Dimensions.Height}");
                Console.WriteLine();
                Console.WriteLine($"Width (pixels):                                      {monitor.Resolution.Width}");
                Console.WriteLine($"Height (pixels):                                     {monitor.Resolution.Height}");
                Console.WriteLine();
                Console.WriteLine($"Width (scaled pixels):                               {monitor.MonitorCoordinates.Width}");
                Console.WriteLine($"Height (scaled pixels):                              {monitor.MonitorCoordinates.Height}");
                Console.WriteLine();
                Console.WriteLine($"Work area width (scaled pixels):                     {monitor.WorkAreaCoordinates.Width}");
                Console.WriteLine($"Work area height (scaled pixels):                    {monitor.WorkAreaCoordinates.Height}");
                Console.WriteLine();
                foreach (var descriptor in monitor.Edid?.Descriptors ?? new List<EDIDDescriptor>())
                {
                    if (!descriptor.IsValid)
                    {
                        continue;
                    }

                    Console.WriteLine($"Descriptor {++descriptorNo}");
                    PrintDescriptor(descriptor, monitor.Edid.DisplayParameters.IsDigital);
                    Console.WriteLine();
                }
                PrintDisplayParameters(monitor.Edid?.DisplayParameters);
                Console.WriteLine($"EDID version:                                        {monitor.Edid?.EDIDVersion}");
                try
                {
                    Console.WriteLine($"Manufacture date:                                    {monitor.Edid?.ManufactureDate}");
                }
                catch (Exception)
                {
                }
                foreach (var extension in monitor.Edid?.Extensions ?? new List<EDIDExtension>())
                {
                    if (!extension.IsValid)
                    {
                        continue;
                    }

                    Console.WriteLine($"Extension {++extensionNo}");
                    PrintExtension(extension);
                    Console.WriteLine();
                }
                Console.WriteLine($"Manufacturer code:                                   {monitor.Edid?.ManufacturerCode}");
                Console.WriteLine($"Manufacturer ID:                                     {monitor.Edid?.ManufacturerId}");
                Console.WriteLine($"Product code:                                        {monitor.Edid?.ProductCode}");
                Console.WriteLine($"Product year:                                        {monitor.Edid?.ProductYear}");
                Console.WriteLine($"Serial number:                                       {monitor.Edid?.SerialNumber}");
                foreach (var timing in monitor.Edid?.Timings ?? new List<ITiming>())
                {
                    Console.WriteLine($"Timing:                                    {timing}");
                }

                Console.WriteLine();
            }
        }

        static void PrintDescriptor(EDIDDescriptor descriptor, bool isDigital)
        {
            if (descriptor is AdditionalStandardTimingDescriptor astDescriptor)
            {
                PrintAdditionalStandardTimingDescriptor(astDescriptor);
            }
            else if (descriptor is StringDescriptor stringDescriptor)
            {
                PrintStringDescriptor(stringDescriptor);
            }
            else if (descriptor is AdditionalWhitePointDescriptor awpDescriptor)
            {
                PrintAdditionalWhitePointDescriptor(awpDescriptor);
            }
            else if (descriptor is DetailedTimingDescriptor dtDescriptor)
            {
                PrintDetailedTimingDescriptor(dtDescriptor, isDigital);
            }
            else if (descriptor is ManufacturerDescriptor mDescriptor)
            {
                PrintManufacturerDescriptor(mDescriptor);
            }
            else if (descriptor is MonitorRangeLimitsDescriptor mrlDescriptor)
            {
                PrintMonitorRangeLimitsDescriptor(mrlDescriptor);
            }
        }

        static void PrintAdditionalStandardTimingDescriptor(AdditionalStandardTimingDescriptor descriptor)
        {
            var timings = descriptor.Timings.ToList() ?? new List<StandardTiming>();
            if (!timings.Any())
            {
                return;
            }

            Console.WriteLine($"Timings:                                    {timings[0]}");

            for (var i = 1; i < timings.Count; ++i)
            {
                Console.Write($"                                           {timings[i]}");
            }
            Console.WriteLine();
        }

        static void PrintAdditionalWhitePointDescriptor(AdditionalWhitePointDescriptor descriptor)
        {
            while (descriptor != null)
            {
                Console.WriteLine($"Gamma:                                               {descriptor.Gamma}");
                Console.WriteLine($"Index:                                               {descriptor.Index}");
                Console.WriteLine($"IsUsed:                                              {descriptor.IsUsed}");
                Console.WriteLine($"White point:                                         ({descriptor.WhitePointX}, {descriptor.WhitePointY})");

                descriptor = descriptor.NextDescriptor;
            }
        }

        static void PrintDetailedTimingDescriptor(DetailedTimingDescriptor descriptor, bool isDigital)
        {
            Console.WriteLine($"Horizontal active pixels:                            {descriptor.HorizontalActivePixels}");
            Console.WriteLine($"Vertical active pixels:                              {descriptor.VerticalActivePixels}");
            Console.WriteLine($"Horizontal blanking pixels:                          {descriptor.HorizontalBlankingPixels}");
            Console.WriteLine($"Vertical blanking pixels:                            {descriptor.VerticalBlankingPixels}");
            Console.WriteLine($"Horizontal border pixels:                            {descriptor.HorizontalBorderPixels}");
            Console.WriteLine($"Vertical border pixels:                              {descriptor.VerticalBorderPixels}");
            Console.WriteLine($"Horizontal display size:                             {descriptor.HorizontalDisplaySize}");
            Console.WriteLine($"Vertical display size:                               {descriptor.VerticalDisplaySize}");
            Console.WriteLine($"Horizontal sync offset:                              {descriptor.HorizontalSyncOffset}");
            Console.WriteLine($"Vertical sync offset:                                {descriptor.VerticalSyncOffset}");

            if (isDigital)
            {
                Console.WriteLine($"Horizontal sync polarity:                            {descriptor.HorizontalSyncPolarity}");
                Console.WriteLine($"Vertical sync polarity:                              {descriptor.VerticalSyncPolarity}");
            }

            Console.WriteLine($"Horizontal sync pulse:                               {descriptor.HorizontalSyncPulse}");
            Console.WriteLine($"Vertical sync pulse:                                 {descriptor.VerticalSyncPulse}");
            Console.WriteLine($"Interlaced?                                          {descriptor.IsInterlaced}");

            if (!isDigital)
            {
                Console.WriteLine($"Syncs on all RGB lines?                              {descriptor.IsSyncOnAllRGBLines}");
                Console.WriteLine($"Vertical sync serrated?                              {descriptor.IsVerticalSyncSerrated}");
            }

            Console.WriteLine($"Pixel clock:                                         {descriptor.PixelClock}");
            Console.WriteLine($"Stereo mode:                                         {descriptor.StereoMode}");
            Console.WriteLine($"Sync type:                                           {descriptor.SyncType}");
        }

        static void PrintManufacturerDescriptor(ManufacturerDescriptor descriptor)
        {
            Console.WriteLine($"Descriptor code:                                     {descriptor.DescriptorCode}");
            Console.Write($"Data:                                                ");

            foreach (var b in descriptor.Data)
            {
                Console.Write($"{b} ");
            }
            Console.WriteLine();
        }

        static void PrintMonitorRangeLimitsDescriptor(MonitorRangeLimitsDescriptor descriptor)
        {
            if (descriptor.IsSecondaryGTFSupported)
            {
                Console.WriteLine($"GTF C:                                               {descriptor.GTFC}");
                Console.WriteLine($"GTF J:                                               {descriptor.GTFJ}");
                Console.WriteLine($"GTF K:                                               {descriptor.GTFK}");
                Console.WriteLine($"GTF M:                                               {descriptor.GTFM}");
                Console.WriteLine($"Secondary curve start frequency:                     {descriptor.SecondaryCurveStartFrequency}");
            }
            Console.WriteLine($"Secondary GTF supported?                             {descriptor.IsSecondaryGTFSupported}");
            Console.WriteLine($"Maximum horizontal field rate:                       {descriptor.MaximumHorizontalFieldRate}");
            Console.WriteLine($"Maximum vertical field rate:                         {descriptor.MaximumVerticalFieldRate}");
            Console.WriteLine($"Maximum pixel clock rate:                            {descriptor.MaximumPixelClockRate}");
            Console.WriteLine($"Minimum horizontal field rate:                       {descriptor.MinimumHorizontalFieldRate}");
            Console.WriteLine($"Minimum vertical field rate:                         {descriptor.MinimumVerticalFieldRate}");
        }

        static void PrintStringDescriptor(StringDescriptor descriptor)
        {
            Console.WriteLine($"Type:                                                {descriptor.Type}");
            Console.WriteLine($"Value:                                               {descriptor.Value}");
        }

        static void PrintDisplayParameters(DisplayParameters displayParams)
        {
            if (displayParams.IsDigital)
            {
                Console.WriteLine($"Digital display type:                                {displayParams.DigitalDisplayType}");
                Console.WriteLine($"VESA DFP 1.x TMDS CRGB compatible?                   {displayParams.IsDFPTMDSCompatible}");
            }
            else
            {
                Console.WriteLine($"Analog display type:                                 {displayParams.AnalogDisplayType}");
                Console.WriteLine($"Blank-to-black expected?                             {displayParams.IsBlankToBlackExpected}");
                Console.WriteLine($"Composite sync supported?                            {displayParams.IsCompositeSyncSupported}");
                Console.WriteLine($"Separate sync supported?                             {displayParams.IsSeparateSyncSupported}");
                Console.WriteLine($"Sync on green supported?                             {displayParams.IsSyncOnGreenSupported}");
                Console.WriteLine($"VSync serrated on composite?                         {displayParams.IsVSyncSerratedOnComposite}");
                Console.WriteLine($"Video white level:                                   {displayParams.VideoWhiteLevel}");
            }
            Console.WriteLine();
            PrintChromaticityCoordinates(displayParams.ChromaticityCoordinates);
            Console.WriteLine();
            try
            {
                Console.WriteLine($"Gamma:                                               {displayParams.DisplayGamma}");
            }
            catch (Exception)
            {
            }
            if (!displayParams.IsProjector)
            {
                Console.WriteLine($"Display size (in.):                                  {displayParams.DisplaySizeInInch}");
                Console.WriteLine($"Physical width (cm.):                                {displayParams.PhysicalWidth}");
                Console.WriteLine($"Physical height (cm.):                               {displayParams.PhysicalHeight}");
            }
            Console.WriteLine($"DPMS active-off supported?                           {displayParams.IsActiveOffSupported}");
            Console.WriteLine($"Default GTF supported?                               {displayParams.IsDefaultGTFSupported}");
            Console.WriteLine($"Digital display?                                     {displayParams.IsDigital}");
            Console.WriteLine($"Preferred timing mode available?                     {displayParams.IsPreferredTimingModeAvailable}");
            Console.WriteLine($"Preferred timing mode includes native info?          {displayParams.IsPreferredTimingModeAvailable}");
            Console.WriteLine($"Projector display?                                   {displayParams.IsProjector}");
            Console.WriteLine($"Standard sRBG color space?                           {displayParams.IsStandardSRGBColorSpace}");
            Console.WriteLine($"DPMS standby supported                               {displayParams.IsStandbySupported}");
            Console.WriteLine($"DPMS suspend supported?                              {displayParams.IsSuspendSupported}");
        }

        static void PrintChromaticityCoordinates(ChromaticityCoordinates coordinates)
        {
            Console.WriteLine("CIE chromaticity coordinates");
            Console.WriteLine($"Red:                                                 ({coordinates.RedX}, {coordinates.RedY})");
            Console.WriteLine($"Green:                                               ({coordinates.GreenX}, {coordinates.GreenY})");
            Console.WriteLine($"Blue:                                                ({coordinates.BlueX}, {coordinates.BlueY})");
            Console.WriteLine($"White:                                               ({coordinates.WhiteX}, {coordinates.WhiteY})");
        }

        static void PrintExtension(EDIDExtension extension)
        {
            Console.WriteLine($"Type:                                      {extension.Type}");
            if (extension is BlockMapExtension blockMapExtension)
            {
                PrintBlockMapExtension(blockMapExtension);
            }
            else if (extension is UnknownExtension unknownExtension)
            {
                PrintUnknownExtension(unknownExtension);
            }
        }

        static void PrintBlockMapExtension(BlockMapExtension extension)
        {
            var types = extension.ExtensionTypes.ToList() ?? new List<ExtensionType>();
            if (!types.Any())
            {
                return;
            }

            Console.Write($"Extension type:                            {types[0]}");

            for (var i = 1; i < types.Count; ++i)
            {
                Console.Write($", {types[i]}");
            }
            Console.WriteLine();
        }

        static void PrintUnknownExtension(UnknownExtension extension)
        {
            var data = extension.Data ?? new byte[0];
            if (data.Length == 0)
            {
                return;
            }

            Console.Write($"Data:                                                    {data[0]}");

            for (var i = 1; i < data.Length; ++i)
            {
                Console.Write($", {data[i]}");
            }
            Console.WriteLine();
        }
    }
}
