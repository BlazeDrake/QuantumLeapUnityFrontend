public record StandardSystemBaseState
{
    public int CurrentPower { get; set; }
    public int RequiredPower { get; set; }
    public bool Disabled { get; set; }
    public bool Damaged { get; set; }

    public const string InsufficientPowerError = "insufficient power";
    public const string DisabledError = "system disabled";
    public const string DamagedError = "system damaged";
}
