using Dapper;
using Domain.Common.Settings;
using System.Text;
using Oracle.ManagedDataAccess.Client;
using Application.Common.Interfaces;
using Application.NaqelAgent.Queries.Students.GetPage;
using Domain.Enums;
using Infrastructure.Utilities;
using Application.NaqelAgent.Queries.Agent.GetDynamicQueryData;

namespace Infrastructure.Respos.Dynamic
{
    internal sealed class DynamicQueryByORACLEDbprovider : IDynamicQuery
    {
        private readonly GeneralSetting _generalSetting;

        public DynamicQueryByORACLEDbprovider(GeneralSetting generalSetting)
        {
            _generalSetting = generalSetting ?? throw new ArgumentNullException(nameof(generalSetting));
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
                throw new Exception(sqlEx.Message, sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return result;
        }

        #endregion
    }
}
