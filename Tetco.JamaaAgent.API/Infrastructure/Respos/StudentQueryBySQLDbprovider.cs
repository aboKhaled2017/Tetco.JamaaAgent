using Application.Common.Interfaces;
using Domain.Common.Settings;
using System.Data.SqlClient;


namespace Infrastructure.Respos
{
    internal sealed class StudentQueryBySQLDbprovider : IStudentQuery
    {
        private readonly GeneralSetting _generalSetting;
        //public const string connectionStr = "";
        public StudentQueryBySQLDbprovider(GeneralSetting generalSetting)
        {
            _generalSetting = generalSetting;
        }

        public async Task<(IEnumerable<Dictionary<string, object>> ColumnInformation, long totalCount, DateTime? lastBatchUpdate)> GetColumnInformation()
        {
            return await GetColumnInformationAsync();
        }

        public async Task<IEnumerable<Dictionary<string, object>>> GetAllAsync(int pageSize , int pageNumber, DateTime?  LastBatchUpdate)
        {
            return await GetDynamicDataFromViewAsync(pageSize, pageNumber, LastBatchUpdate);
            //return new DataGenerator().GenerateStudents(1000)
            //.Skip((pageNumber - 1) * pageSize)
            //.Take(pageSize)
            //    .ToList();
        }


        private async Task<(IEnumerable<Dictionary<string, object>> ColumnInformation, long totalCount, DateTime? lastBatchUpdate)> GetColumnInformationAsync()
        {
            using var connection = new SqlConnection(_generalSetting.ConnectionStr);
            await connection.OpenAsync();

            using var command = new SqlCommand($"SELECT COLUMN_NAME,DATA_TYPE,CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'StudentDetailsView';", connection);
            using var reader = await command.ExecuteReaderAsync();

            var result = new List<Dictionary<string, object>>();

            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var columnName = reader.GetName(i);
                    var columnValue = reader.GetValue(i);

                    row.Add(columnName, columnValue);
                }

                result.Add(row);
            }

            return (result, 1000, DateTime.Now);
        }


        private async Task<IEnumerable<Dictionary<string, object>>> GetDynamicDataFromViewAsync(int pageSize , int pageNumber, DateTime? LastBatchUpdate)
        {
            if (pageNumber < 1)
                pageNumber = 1;

            using var connection = new SqlConnection(_generalSetting.ConnectionStr);
            await connection.OpenAsync();

            var query = $"SELECT * FROM dbo.StudentDetailsView ORDER BY StudentUniqueId OFFSET {pageSize * (pageNumber - 1)} ROWS FETCH NEXT {pageSize} ROWS ONLY";
            using var command = new SqlCommand(query, connection);
            using var reader = await command.ExecuteReaderAsync();

            var result = new List<Dictionary<string, object>>();

            while (await reader.ReadAsync())
            {
                var row = new Dictionary<string, object>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var columnName = reader.GetName(i);
                    var columnValue = reader.GetValue(i);

                    row.Add(columnName, columnValue);
                }

                result.Add(row);
            }

            return result;
        }
    }
}
