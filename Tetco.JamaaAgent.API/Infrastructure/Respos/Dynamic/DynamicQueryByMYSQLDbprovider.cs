using Application.Common.Interfaces;
using Application.NaqelAgent.Queries.Students.GetDynamicQueryData;
using Dapper;
using Domain.Common.Settings;
using Domain.Enums;
using Infrastructure.Utilities;
using MySql.Data.MySqlClient;
using System.Text;

namespace Infrastructure.Respos.Dynamic
{
    internal sealed class DynamicQueryByMYSQLDbprovider : IDynamicQuery
    {
        private readonly GeneralSetting _generalSetting;

        public DynamicQueryByMYSQLDbprovider(GeneralSetting generalSetting)
        {
            _generalSetting = generalSetting ?? throw new ArgumentNullException(nameof(generalSetting));
        }


        public async Task<IEnumerable<ViewDynamicData>> GetDynamicInformation(string query, IEnumerable<Paramter> paramters, int noOfQueries, string definedConnectionStr)
        {
            return await GetDymaicData(query, paramters, noOfQueries, definedConnectionStr);
        }


        #region Helper

        private async Task<IEnumerable<ViewDynamicData>> GetDymaicData(string query, IEnumerable<Paramter> paramters, int noOfQueries, string definedConnectionStr)
        {
            var result = new List<ViewDynamicData>();
            try
            {
                using (var connection = new MySqlConnection(definedConnectionStr))
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
            catch (MySqlException sqlEx)
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
