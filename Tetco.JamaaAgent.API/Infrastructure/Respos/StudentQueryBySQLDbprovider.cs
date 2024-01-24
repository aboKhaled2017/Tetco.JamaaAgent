using Application.Common.Interfaces;
using Application.NaqelAgent.Queries.Students.GetPage;
using Application.NaqelAgent.Queries.Students.GetStudentMetaData;
using Dapper;
using Domain.Common.Settings;
using System.Data.SqlClient;


namespace Infrastructure.Respos
{
    internal sealed class StudentQueryBySQLDbprovider : IStudentQuery
    {
        private readonly GeneralSetting _generalSetting;
        public StudentQueryBySQLDbprovider(GeneralSetting generalSetting)
        {
            _generalSetting = generalSetting;
        }

        public async Task<IEnumerable<ViewDetail>> GetAllAsync(int pageSize, int pageNumber, DateTime? LastBatchUpdate, string masterViewName, List<string> relatedViews, string associationColumnName, string columnNameFilter)
        {
            return await GetDynamicDataFromViewAsync(pageSize, pageNumber, LastBatchUpdate, masterViewName, relatedViews, associationColumnName, columnNameFilter);
        }

        public async Task<IEnumerable<ViewsMetaData>> GetColumnInformation(List<string> views)
        {
            return await GetColumnInformationAsync(views);
        }

        public async Task<IEnumerable<ViewsMetaData>> GetColumnInformationAsync(List<string> views)
        {
            var result = new List<ViewsMetaData>();

            try
            {
                using (var connection = new SqlConnection(_generalSetting.ConnectionStr))
                {
                    await connection.OpenAsync();

                    var multipleQueries = "";

                    foreach (var viewName in views)
                    {
                        multipleQueries += $@"
                    SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH
                    FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = '{viewName}';";
                    }

                    var datares = await connection.QueryMultipleAsync(multipleQueries, commandTimeout: 100000);

                    foreach (var viewName in views)
                    {
                        var data = datares.Read<dynamic>().ToList();
                        var viewDetails = new ViewsMetaData(data, 1000, DateTime.Now, viewName);
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


        private async Task<IEnumerable<ViewDetail>> GetDynamicDataFromViewAsync(int pageSize, int pageNumber, DateTime? lastBatchUpdate, string masterViewName, List<string> relatedViews, string associationColumnName, string columnNameFilter)
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

                    var masterQuery = $@"SELECT * FROM {masterViewName}  ORDER BY {associationColumnName} OFFSET {pageSize * (pageNumber - 1)} ROWS FETCH NEXT {pageSize} ROWS ONLY;";
                    var masterViewData = await connection.QueryAsync<dynamic>(masterQuery, commandTimeout: 100000);

                    var values = masterViewData
                                .Cast<IDictionary<string, object>>()  // Explicitly cast each dynamic object to IDictionary<string, object>
                                .Select(c => 
                                     (c[associationColumnName]).ToString()
                                )
                                .ToList();

                    if (values?.Any() ?? false)
                    {
                        var multipleQueries = string.Empty;
                        foreach (var viewName in relatedViews)
                        {
                            multipleQueries += $@"SELECT * FROM {viewName} where {associationColumnName} in @FilteredValues;";
                        }
                        var resultForRelatedViews = await connection.QueryMultipleAsync(multipleQueries, new { FilteredValues = values }, commandTimeout: 100000);
                        foreach (var viewName in relatedViews)
                        {
                            var data = resultForRelatedViews.Read<dynamic>().ToList();
                            var viewDetails = new ViewDetail(data, viewName);
                            result.Add(viewDetails);
                        }
                        result.Add(new ViewDetail(masterViewData, masterViewName));
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

    }
}

