using Application.Common.Interfaces;
using Domain.Common.Settings;
using Domain.Enums;
using Infrastructure.Respos.Dynamic;
using Microsoft.Extensions.Logging;
using static Infrastructure.Respos.DefineProviderManager;

namespace Infrastructure.Respos
{
    public class CreateDatabaseFactory : ICreateDatabaseFactory
    {
        private readonly GeneralSetting _generalSetting;
        private readonly ILogger<DynamicQueryBySQLDbprovider> _dynamicQueryBySQLDbproviderlogger;
        private readonly ILogger<DynamicQueryByMYSQLDbprovider> _dynamicQueryByMYSQLDbprovider;
        private readonly ILogger<DynamicQueryByORACLEDbprovider> _dynamicQueryByORACLEDbprovider;

        public CreateDatabaseFactory(GeneralSetting generalSetting, 
            ILogger<DynamicQueryBySQLDbprovider> dynamicQueryBySQLDbproviderlogger,
            ILogger<DynamicQueryByMYSQLDbprovider> dynamicQueryByMYSQLDbprovider,
            ILogger<DynamicQueryByORACLEDbprovider> dynamicQueryByORACLEDbprovider
            )
        {
            _generalSetting = generalSetting ?? throw new ArgumentNullException(nameof(generalSetting));
            _dynamicQueryBySQLDbproviderlogger = dynamicQueryBySQLDbproviderlogger ?? throw new ArgumentNullException(nameof(_dynamicQueryBySQLDbproviderlogger));
            _dynamicQueryByMYSQLDbprovider = dynamicQueryByMYSQLDbprovider ?? throw new ArgumentNullException(nameof(_dynamicQueryByMYSQLDbprovider));
            _dynamicQueryByORACLEDbprovider = dynamicQueryByORACLEDbprovider ?? throw new ArgumentNullException(nameof(_dynamicQueryByORACLEDbprovider));
        }

        public IDynamicQuery CreateProviderFactory(DBProvider databaseType)
        {
            return databaseType switch
            {
                DBProvider.SQL => new DynamicQueryBySQLDbprovider(_generalSetting, _dynamicQueryBySQLDbproviderlogger),
                DBProvider.MySQL => new DynamicQueryByMYSQLDbprovider(_generalSetting, _dynamicQueryByMYSQLDbprovider),
                DBProvider.Oracle => new DynamicQueryByORACLEDbprovider(_generalSetting, _dynamicQueryByORACLEDbprovider),
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
