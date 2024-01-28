using Domain.Common.Settings;
using Domain.Enums;

namespace Application.Common.Interfaces
{
    public interface ICreateDatabaseFactory
    {
       IDynamicQuery CreateProviderFactory(DBProvider dBProvider);
        IConnectionStringProvider CreateConnectionStringProvider(SchemaType schemaType, GeneralSetting generalSetting);
    }
}
