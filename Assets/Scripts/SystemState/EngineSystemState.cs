using System;

public record EnginesState : StandardSystemBaseState
{
    public int CurrentSpeed { get; set; }
    public EngineSpeedConfig SpeedConfig { get; set; }
    public int CurrentHeat { get; set; }
    public EngineHeatConfig HeatConfig { get; set; }
    public SpeedPowerRequirement[] SpeedPowerRequirements { get; set; } = Array.Empty<SpeedPowerRequirement>();
}

public record EngineSpeedConfig
{
    public int MaxSpeed { get; set; }
    public int CruisingSpeed { get; set; }
}

public record EngineHeatConfig
{
    public int PoweredHeat { get; set; }
    public int CruisingHeat { get; set; }
    public int MaxHeat { get; set; }
    public int MinutesAtMaxSpeed { get; set; }
    public int MinutesToCoolDown { get; set; }
}

public record SpeedPowerRequirement
{
    public int Speed { get; set; }
    public int PowerNeeded { get; set; }
}