using Application.Common.Interfaces;
using Application.NaqelAgent.Queries.Students.GetDynamicQueryData;
using Domain.Common.Settings;
using Domain.Enums;

namespace Infrastructure.Respos
{
    internal partial class DefineProviderManager : IDefineProviderManager
    {
        private readonly GeneralSetting _generalSetting;
        public ICreateDatabaseFactory _databaseFactory { get; set; }

        public DefineProviderManager(ICreateDatabaseFactory databaseFactory, GeneralSetting generalSetting)
        {
            _databaseFactory = databaseFactory ?? throw new ArgumentNullException(nameof(databaseFactory));
            _generalSetting = generalSetting ?? throw new ArgumentNullException(nameof(generalSetting));
        }

        public async Task<IEnumerable<ViewDynamicData>> GetDynamicInformation(string queryStr, IEnumerable<Paramter> parameters, int noOfQueries, DBProvider provider,SchemaType schemaType)
        {
            var defienConnectionStr = _databaseFactory.CreateConnectionStringProvider(schemaType, _generalSetting).GetConnectionString(provider);
            var result = await _databaseFactory.CreateProviderFactory(provider).GetDynamicInformation(queryStr, parameters, noOfQueries, defienConnectionStr);
            return result;
        }


    }
}
