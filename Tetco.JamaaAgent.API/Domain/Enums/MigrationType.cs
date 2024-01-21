namespace Domain.Enums;

public sealed record MigrationType : SmartEnum<MigrationType>
{
    public MigrationType(string key, int value) : base(key, value)
    {
    }

    public static MigrationType Full = new("Full_Migration", 1);
    public static MigrationType Sync = new("Sync_Data", 2);
}
