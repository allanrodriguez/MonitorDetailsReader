# Monitor Details Reader
[![Build status](https://ci.appveyor.com/api/projects/status/g2r0vk9omnw55h5t/branch/master?svg=true)](https://ci.appveyor.com/project/allanrodriguez/monitordetailsreader/branch/master) [![Coverage Status](https://coveralls.io/repos/github/allanrodriguez/MonitorDetailsReader/badge.svg?branch=master)](https://coveralls.io/github/allanrodriguez/MonitorDetailsReader?branch=master)

Retrieves current settings and useful information about the monitors connected to the host computer, including pixel dimensions, physical measurements, EDID, monitor handles, and virtual screen coordinates.

Credit to Ofek Shilon for his [blog posts](https://ofekshilon.com/2014/06/19/reading-specific-monitor-dimensions/) on how to get the physical dimensions of a specific monitor using Visual C++. This is basically a C# port of his code using PInvoke, with some extra functionality added to get DPI scaling factors, physical pixel dimensions, and EDID properties.

Targets .NET Framework 4.6.1.

## Usage

Create an instance of ```Reader``` and call the ```GetMonitorDetails``` method:
```
using MonitorDetails;
using MonitorDetails.Models;
...
IReader reader = new Reader();

IEnumerable<Monitor> monitors = reader.GetMonitorDetails();
...
```

## Remarks

A test console program (```MonitorDetailsReader.Test```) is included that when run prints out most of the useful information for each monitor.

Monitors vary far and wide, so your mileage may vary with this reader. If you have any improvements you'd like to share, feel free to submit a pull request!

Uses [EDIDParser](https://github.com/falahati/EDIDParser) to make sense of monitors' EDID.
