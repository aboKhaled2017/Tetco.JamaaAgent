﻿using Application.Common.Interfaces;
using Application.NaqelAgent.Queries.Agent.GetDynamicQueryData;
using Application.NaqelAgent.Queries.Students.GetPage;
using Application.NaqelAgent.Queries.Students.GetStudentMetaData;
using Dapper;
using Domain.Common.Settings;
using System.Data.SqlClient;
using System.Text;


namespace Infrastructure.Respos.Students
{
    internal sealed class StudentQueryBySQLDbprovider : IStudentQuery
    {
        private readonly GeneralSetting _generalSetting;
        public StudentQueryBySQLDbprovider(GeneralSetting generalSetting)
        {
            _generalSetting = generalSetting;
        }

        public async Task<IEnumerable<ViewDetail>> GetAllAsync(int pageSize, int pageNumber, string schemaName, string masterViewName, List<string> relatedViews, string associationColumnName, string columnNameFilter, string from, string to)
        {
            return await GetDynamicDataFromViewAsync(pageSize, pageNumber, schemaName, masterViewName, relatedViews, associationColumnName, columnNameFilter, from, to);
        }

        public async Task<IEnumerable<ViewsMetaData>> GetColumnInformation(string schemaName, List<string> views)
        {
            return await GetColumnInformationAsync(schemaName, views);
        }

        public async Task<IEnumerable<ViewDynamicData>> GetDynamicInformation(string query, IEnumerable<Paramter> paramters, int noOfQueries)
        {
            return await GetDymaicData(query, paramters, noOfQueries);
        }


        #region Helper

        private async Task<IEnumerable<ViewDynamicData>> GetDymaicData(string query, IEnumerable<Paramter> paramters, int noOfQueries)
        {
            var result = new List<ViewDynamicData>();
            try
            {
                using (var connection = new SqlConnection(_generalSetting.ConnectionStr))
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
            catch (SqlException sqlEx)
            {
                throw new Exception(sqlEx.Message, sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return result;
        }
        private async Task<IEnumerable<ViewsMetaData>> GetColumnInformationAsync(string schemaName, List<string> views)
        {
            var result = new List<ViewsMetaData>();

            try
            {
                using (var connection = new SqlConnection(_generalSetting.ConnectionStr))
                {
                    await connection.OpenAsync();
                    var multipleQueries = new StringBuilder();
                    var parameters = new DynamicParameters();
                    parameters.Add("SchemaName", schemaName);
                    var index = 0;
                    foreach (var viewName in views)
                    {

                        multipleQueries.Append($@"SELECT count(*) from [{schemaName}].[{viewName}];

                                          SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH
                                          FROM INFORMATION_SCHEMA.COLUMNS
                                          WHERE TABLE_SCHEMA = @SchemaName AND TABLE_NAME = @ViewName{index};");
                        parameters.Add($"ViewName{index}", viewName);
                        index++;
                    }

                    var datares = await connection.QueryMultipleAsync(multipleQueries.ToString(), parameters, commandTimeout: _generalSetting.TimeOut);

                    foreach (var viewName in views)
                    {
                        var count = datares.Read<long>().FirstOrDefault();
                        //long.TryParse(countRes.ToString(), out long count);
                        var data = datares.Read<dynamic>().ToList();
                        var viewDetails = new ViewsMetaData($"{schemaName}.{viewName}", data, count, DateTime.Now);
                        result.Add(viewDetails);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                throw new Exception(sqlEx.Message, sqlEx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            return result;
        }
        private async Task<IEnumerable<ViewDetail>> GetDynamicDataFromViewAsync(int pageSize, int pageNumber, string schemaName, string masterViewName, List<string> relatedViews, string associationColumnName, string columnNameFilter, string from, string to)
        {
            if (pageNumber < 1)
                pageNumber = 1;

            if (pageSize < 1)
                pageSize = 1;

            var result = new List<ViewDetail>();

            try
            {
                using (var connection = new SqlConnection(_generalSetting.ConnectionStr))
                {
                    await connection.OpenAsync();
                    var masterQuery = $@"SELECT * FROM [{schemaName}].[{masterViewName}] ORDER BY [{associationColumnName}] OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";
                    var masterViewData = await connection.QueryAsync<dynamic>(masterQuery, new { Offset = pageSize * (pageNumber - 1), PageSize = pageSize }, commandTimeout: _generalSetting.TimeOut);

                    var values = masterViewData
                                .Cast<IDictionary<string, object>>()  // Explicitly cast each dynamic object to IDictionary<string, object>
                                .Select(c =>
                                     c[associationColumnName].ToString())
                                .ToList();

                    var multipleQueries = new StringBuilder();
                    var parameters = new DynamicParameters();
                    parameters.Add("FilteredValues", values);

                    foreach (var viewName in relatedViews)
                    {
                        multipleQueries.Append($"SELECT * FROM [{schemaName}].[{viewName}] WHERE [{associationColumnName}] in @FilteredValues;");
                    }

                    var resultForRelatedViews = await connection.QueryMultipleAsync(multipleQueries.ToString(), parameters, commandTimeout: _generalSetting.TimeOut);

                    foreach (var viewName in relatedViews)
                    {
                        var data = resultForRelatedViews.Read<dynamic>().ToList();
                        var viewDetails = new ViewDetail(data, $"{schemaName}.{viewName}");
                        result.Add(viewDetails);
                    }
                }
            }
            catch (SqlException sqlEx)
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

