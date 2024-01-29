using Application.Agent.Queries.GetDynamicQueryData;
using Application.Common.Interfaces;
using Dapper;
using Domain.Common.Settings;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using System.Text;

namespace Infrastructure.Respos.Dynamic
{
    public sealed class DynamicQueryByORACLEDbprovider : IDynamicQuery
    {
        private readonly GeneralSetting _generalSetting;
        private readonly ILogger<DynamicQueryByORACLEDbprovider> _logger;

        public DynamicQueryByORACLEDbprovider(GeneralSetting generalSetting,ILogger<DynamicQueryByORACLEDbprovider> logger)
        {
            _generalSetting = generalSetting ?? throw new ArgumentNullException(nameof(generalSetting));
            _logger = logger ?? throw new ArgumentNullException( nameof(logger));
        }

        public async Task<IEnumerable<ViewDynamicData>> GetDynamicInformation(string query, IEnumerable<Paramter> paramters, int noOfQueries,string definedConnectionStr)
        {
            return await GetDymaicData(query, paramters, noOfQueries, definedConnectionStr);
        }

        #region Helper
        private async Task<IEnumerable<ViewDynamicData>> GetDymaicData(string query, IEnumerable<Paramter> paramters, int noOfQueries, string definedConnectionStr)
        {
            var result = new List<ViewDynamicData>();
            try
            {
                using (var connection = new OracleConnection(definedConnectionStr))
                {
                    await connection.OpenAsync();
                    var multipleQueries = new StringBuilder(query);
                    var parameters = new DynamicParameters();
                    foreach (var paramter in paramters)
                        parameters.Add(paramter.ParamterName.ToString(), paramter.Value);

                    var datares = await connection.QueryMultipleAsync(multipleQueries.ToString(), parameters, commandTimeout: _generalSetting.TimeOut);

                    for (int i = 1; i < noOfQueries + 1; i++)
                    {
                        var data = datares.Read<dynamic>().ToList();
                        var viewDetails = new ViewDynamicData($"Result of Query Number {i}", data, data.Count);
                        result.Add(viewDetails);
                    }
                }
            }
            catch (OracleException sqlEx)
            {
                _logger.LogError(sqlEx.Message, sqlEx);
                throw new Exception(sqlEx.Message, sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new Exception(ex.Message, ex);
            }
            return result;
        }

        #endregion
    }
}
