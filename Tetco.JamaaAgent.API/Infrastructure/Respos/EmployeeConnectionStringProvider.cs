using Domain.Common.Settings;
using Domain.Enums;

namespace Infrastructure.Respos
{
    internal partial class DefineProviderManager
    {
        // Employee schema connection string provider
        public class EmployeeConnectionStringProvider : IConnectionStringProvider
        {
            private readonly GeneralSetting _generalSetting;

            public EmployeeConnectionStringProvider(GeneralSetting generalSetting)
            {
                _generalSetting = generalSetting ?? throw new ArgumentNullException(nameof(generalSetting));
            }

            public string GetConnectionString(DBProvider databaseProvider)
            {
                return databaseProvider switch
                {
                    DBProvider.SQL => _generalSetting.StudentConnection.SQLConnectionStr,
                    DBProvider.MySQL => _generalSetting.StudentConnection.MySQLConnectionStr,
                    DBProvider.Oracle => _generalSetting.StudentConnection.ORACLEConnectionStr,
                    _ => throw new NotSupportedException($"Database provider {databaseProvider} is not supported."),
                };
            }
        }


    }
}
