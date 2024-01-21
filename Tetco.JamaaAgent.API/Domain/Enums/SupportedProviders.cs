
namespace Domain.Enums;

public sealed record SupportedProviders : SmartEnum<SupportedProviders>
{
    public SupportedProviders(string key, int value) : base(key, value)
    {
    }

    public static MigrationType SQL = new("SQL", 1);
    public static MigrationType MYSQL = new("MYSQL", 2);
    public static MigrationType Oracle = new("Oracle", 3);
}
