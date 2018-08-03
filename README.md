# Monitor Details Reader

Retrieves current settings and useful information about the monitors connected to the host computer, including pixel dimensions, physical measurements, EDID, monitor handles, and virtual screen coordinates.

Credit to Ofek Shilon for his [blog posts](https://ofekshilon.com/2014/06/19/reading-specific-monitor-dimensions/) on how to get the physical dimensions of a specific monitor using Visual C++. This is basically a C# port of his code using PInvoke, with some extra functionality added to get DPI scaling factors and physical pixel dimensions.

Targets .NET Framework 2.0 for maximum compatibility.

## Usage

Create an instance of MonitorDetailsReader and call the **GetMonitorDetails** method, like so:
```
IMonitorDetailsReader mdReader = new MonitorDetailsReader();

// This is like the only public API so far.
ReadOnlyCollection<IMonitorDetails> monitorDetailsList = mdReader.GetMonitorDetails();
```

## Remarks

A test console program (MonitorDetailsReader.Test) is included that when run prints out most of the useful information for each monitor.

Monitors vary far and wide, so your mileage may vary with this reader. If you have any improvements you'd like to share, feel free to contact me or submit a pull request!