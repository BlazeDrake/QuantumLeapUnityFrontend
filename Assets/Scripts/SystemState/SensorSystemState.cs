using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public record SensorsState : StandardSystemBaseState
{
    public List<SensorScan> ActiveScans { get; set; }
    public SensorScan LastUpdatedScan { get; set; }

    public List<SensorContact> Contacts { get; set; }
}

[Serializable]
public record SensorScan
{
    public string ScanId { get; set; } = "";
    public string State { get; set; } = "";
    public string ScanFor { get; set; } = "";
    public string Result { get; set; } = "";
    public DateTimeOffset LastUpdated { get; set; } = DateTimeOffset.UtcNow;
}

public static class SensorScanState
{
    public static string Active = "active";
    public static string Completed = "completed";
    public static string Canceled = "canceled";
}

[Serializable]
public record SensorContact
{
    public string ContactId { get; set; } = "";
    public string Name { get; set; } = "";
    public string Icon { get; set; } = "";
    public Point Position { get; set; }
    public Destination[] Destinations { get; set; } = Array.Empty<Destination>();
}

[Serializable]
public record Destination
{
    public Point Position { get; set; }
    public int RemainingMilliseconds { get; set; }
}
