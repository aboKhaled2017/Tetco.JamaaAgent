﻿using Application.Agent.Queries.Students.GetPage;
using Application.Agent.Queries.Students.GetStudentMetaData;
using Application.Common.Interfaces;
using Dapper;
using Domain.Common.Settings;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System.Text;

namespace Infrastructure.Respos.Students
{
    internal sealed class StudentQueryByMySQLDbProvider : IStudentQuery
    {
        private readonly GeneralSetting _generalSetting;
        private readonly ILogger<StudentQueryByMySQLDbProvider> _logger;

        public StudentQueryByMySQLDbProvider(GeneralSetting generalSetting, ILogger<StudentQueryByMySQLDbProvider> logger)
        {
            _generalSetting = generalSetting ?? throw new ArgumentNullException(nameof(generalSetting));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<ViewDetail>> GetAllAsync(int pageSize, int pageNumber, string schemaName, string masterViewName, List<string> relatedViews, string associationColumnName, string columnNameFilter, string from, string to)
        {
            return await GetDynamicDataFromViewAsync(pageSize, pageNumber, schemaName, masterViewName, relatedViews, associationColumnName, columnNameFilter, from, to);
        }

        public async Task<IEnumerable<ViewsMetaData>> GetColumnInformation(string schemaName, List<string> views)
        {
            return await GetColumnInformationAsync(schemaName, views);
        }

        #region Helper

        private async Task<IEnumerable<ViewsMetaData>> GetColumnInformationAsync(string schemaName, List<string> views)
        {
            var result = new List<ViewsMetaData>();

            try
            {
                using (var connection = new MySqlConnection(_generalSetting.StudentConnection.MySQLConnectionStr))
                {
                    await connection.OpenAsync();
                    var multipleQueries = new StringBuilder();
                    var parameters = new DynamicParameters();
                    parameters.Add("SchemaName", schemaName);

                    foreach (var viewName in views)
                    {
                        multipleQueries.Append($@"SELECT count(*) from `{schemaName}`.`{viewName}`;

                                          SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH
                                          FROM INFORMATION_SCHEMA.COLUMNS
                                          WHERE TABLE_SCHEMA = @SchemaName AND TABLE_NAME = '{viewName}';");
                        parameters.Add($"ViewName", viewName); // Avoid using indexed parameters for simplicity
                    }

                    var datares = await connection.QueryMultipleAsync(multipleQueries.ToString(), parameters, commandTimeout: _generalSetting.TimeOut);

                    foreach (var viewName in views)
                    {
                        var count = datares.Read<long>().FirstOrDefault();
                        var data = datares.Read<dynamic>().ToList();
                        var viewDetails = new ViewsMetaData($"{schemaName}.{viewName}", data, count, DateTime.Now);
                        result.Add(viewDetails);
                    }
                }
            }
            catch (MySqlException mySqlEx)
            {
                _logger.LogError(mySqlEx.Message, mySqlEx);
                throw new Exception(mySqlEx.Message, mySqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
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
                using (var connection = new MySqlConnection(_generalSetting.StudentConnection.MySQLConnectionStr))
                {
                    await connection.OpenAsync();
                    var offset = (pageNumber - 1) * pageSize;
                    var masterQuery = $@"SELECT * FROM `{schemaName}`.`{masterViewName}` ORDER BY `{associationColumnName}` LIMIT {offset}, {pageSize};";
                    var masterViewData = await connection.QueryAsync<dynamic>(masterQuery, commandTimeout: _generalSetting.TimeOut);

                    var values = masterViewData
                                .Cast<IDictionary<string, object>>()
                                .Select(c => c[associationColumnName].ToString())
                                .ToList();

                    var multipleQueries = new StringBuilder();
                    var parameters = new DynamicParameters();
                    parameters.Add("FilteredValues", values);

                    foreach (var viewName in relatedViews)
                    {
                        multipleQueries.Append($"SELECT * FROM `{schemaName}`.`{viewName}` WHERE `{associationColumnName}` in @FilteredValues;");
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
            catch (MySqlException mySqlEx)
            {
                _logger.LogError(mySqlEx.Message, mySqlEx);
                throw new Exception(mySqlEx.Message, mySqlEx);
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
