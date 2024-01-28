using Application.Common.Interfaces;
using Domain.Common.Settings;
using Domain.Enums;
using Infrastructure.Respos.Dynamic;
using static Infrastructure.Respos.DefineProviderManager;

namespace Infrastructure.Respos
{
    public class CreateDatabaseFactory : ICreateDatabaseFactory
    {
        private readonly GeneralSetting _generalSetting;

        public CreateDatabaseFactory(GeneralSetting generalSetting)
        {
            _generalSetting = generalSetting ?? throw new ArgumentNullException(nameof(generalSetting));
        }

        public IDynamicQuery CreateProviderFactory(DBProvider databaseType)
        {
            return databaseType switch
            {
                DBProvider.SQL => new DynamicQueryBySQLDbprovider(_generalSetting),
                DBProvider.MySQL => new DynamicQueryByMYSQLDbprovider(_generalSetting),
                DBProvider.Oracle => new DynamicQueryByORACLEDbprovider(_generalSetting),
                _ => throw new NotSupportedException($"Database type {databaseType} is not supported."),
            };
        }

        public IConnectionStringProvider CreateConnectionStringProvider(SchemaType schemaType, GeneralSetting generalSetting)
        {
            return schemaType switch
            {
                SchemaType.Student => new StudentConnectionStringProvider(generalSetting),
                SchemaType.Employee => new EmployeeConnectionStringProvider(generalSetting),
                _ => throw new NotSupportedException($"Schema type {schemaType} is not supported."),
            };
        }

    }
}
