using Domain.Common.Settings;
using Domain.Enums;

namespace Infrastructure.Respos
{
    internal partial class DefineProviderManager
    {
        // Student schema connection string provider
        public class StudentConnectionStringProvider : IConnectionStringProvider
        {
            private readonly GeneralSetting _generalSetting;

            public StudentConnectionStringProvider(GeneralSetting generalSetting)
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
