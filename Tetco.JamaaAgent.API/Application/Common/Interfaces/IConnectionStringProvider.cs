using Domain.Enums;

public interface IConnectionStringProvider
{
    string GetConnectionString(DBProvider databaseProvider);
}