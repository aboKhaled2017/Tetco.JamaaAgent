using Application.Common.Interfaces;
using Application.NaqelAgent.Queries.Students.GetPage;
using Application.NaqelAgent.Queries.Students.GetStudentMetaData;
using Bogus;
using Dapper;
using Domain.Common.Settings;
using System.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace Infrastructure.Respos
{
    internal sealed class StudentQueryBySQLDbprovider : IStudentQuery
    {
        private readonly GeneralSetting _generalSetting;
        public StudentQueryBySQLDbprovider(GeneralSetting generalSetting)
        {
            _generalSetting = generalSetting;
        }

        public async Task<IEnumerable<ViewDetail>> GetAllAsync(int pageSize, int pageNumber, DateTime? LastBatchUpdate, string masterViewName, List<string> relatedViews, string associationColumnName,string columnNameFilter)
        {
            return await GetDynamicDataFromViewAsync(pageSize, pageNumber, LastBatchUpdate, masterViewName, relatedViews, associationColumnName,columnNameFilter);
        }
        

        //public async Task<IEnumerable<Dictionary<string, object>>> GetAllAsync(int pageSize , int pageNumber, DateTime?  LastBatchUpdate)
        //{
        //    return await GetDynamicDataFromViewAsync(pageSize, pageNumber, LastBatchUpdate);
        //}

        //private async Task<IEnumerable<Dictionary<string, object>>> GetDynamicDataFromViewAsync(int pageSize , int pageNumber, DateTime? LastBatchUpdate)
        //{
        //    if (pageNumber < 1)
        //        pageNumber = 1;

        //    if (pageSize < 1)
        //        pageSize = 1;


        //    var result = new List<Dictionary<string, object>>();

        //    using (var connection = new SqlConnection(_generalSetting.ConnectionStr))
        //    {
        //        await connection.OpenAsync();
        //        var query = $"SELECT * FROM dbo.StudentDetailsView ORDER BY StudentUniqueId OFFSET {pageSize * (pageNumber - 1)} ROWS FETCH NEXT {pageSize} ROWS ONLY";
        //        using var command = new SqlCommand(query, connection);
        //        using var reader = await command.ExecuteReaderAsync();
        //        while (await reader.ReadAsync())
        //        {
        //            var row = new Dictionary<string, object>();

        //            for (int i = 0; i < reader.FieldCount; i++)
        //            {
        //                var columnName = reader.GetName(i);
        //                var columnValue = reader.GetValue(i);

        //                row.Add(columnName, columnValue);
        //            }

        //            result.Add(row);
        //        }
        //    }
        //    return result;
        //}

        public async Task<IEnumerable<ViewsMetaData>> GetColumnInformation(List<string> views)
        {
            return await GetColumnInformationAsync(views);
        }

        private async Task<IEnumerable<ViewsMetaData>> GetColumnInformationAsync(List<string> Views)
        {
            var result = new List<ViewsMetaData>();

            using (var connection = new SqlConnection(_generalSetting.ConnectionStr))
            {

                await connection.OpenAsync();

                var multipleQueries = @"";

                foreach (var viewName in Views)
                {
                     multipleQueries += $@"
                    SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH
                    FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_NAME = '{viewName}';";
                }

               var datares= await connection.QueryMultipleAsync(multipleQueries, commandTimeout: 100000);

                foreach (var viewName in Views)
                {
                    var data = datares.Read<dynamic>().ToList();
                    var viewDetails = new ViewsMetaData(data, 1000 ,DateTime.Now ,viewName);
                    result.Add(viewDetails);
                }
            }

            return result;
        }



        private async Task<IEnumerable<ViewDetail>> GetDynamicDataFromViewAsync(int pageSize, int pageNumber, DateTime? lastBatchUpdate, string masterViewName, List<string> relatedViews, string associationColumnName,string columnNameFilter)
        {
            if (pageNumber < 1)
                pageNumber = 1;

            if (pageSize < 1)
                pageSize = 1;


            var result = new List<ViewDetail>();

            using (var connection = new SqlConnection(_generalSetting.ConnectionStr))
            {
                await connection.OpenAsync();

                //var masterQuery = $@"SELECT * FROM {masterViewName} where {filterColumn} > {lastBatchUpdate} ORDER BY {associationColumnName} OFFSET {pageSize * (pageNumber - 1)} ROWS FETCH NEXT {pageSize} ROWS ONLY;";
                var masterQuery = $@"SELECT * FROM {masterViewName}  ORDER BY {associationColumnName} OFFSET {pageSize * (pageNumber - 1)} ROWS FETCH NEXT {pageSize} ROWS ONLY;";
                var mastrViewData = await connection.QueryAsync<dynamic>(masterQuery, commandTimeout: 100000);

                var values = mastrViewData
                            .Cast<IDictionary<string, object>>()  // Explicitly cast each dynamic object to IDictionary<string, object>
                            .Select(c => new
                            {
                                ColumnValue = c[associationColumnName],
                            }).Select(x=> x.ColumnValue.ToString())
                            .ToList();

                if (values?.Any() ?? false)
                {
                    var multipleQueries = string.Empty;
                    foreach (var viewName in relatedViews)
                    {
                        multipleQueries += $@"SELECT * FROM {viewName} where {associationColumnName} in @FilteredValues;";
                    }                    
                    var resultforReletedViews = await connection.QueryMultipleAsync(multipleQueries, new { FilteredValues = values },commandTimeout:100000);
                    foreach (var viewName in relatedViews)
                    {
                        var data = resultforReletedViews.Read<dynamic>().ToList();
                        var viewDetails = new ViewDetail(data, viewName);
                        result.Add(viewDetails);
                    }
                    result.Add(new ViewDetail(mastrViewData, masterViewName));
                }

                
            }
            return result;
        }
    }
}
