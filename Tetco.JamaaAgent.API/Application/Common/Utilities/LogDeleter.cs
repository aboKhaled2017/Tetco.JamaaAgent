namespace Application.Common.Utilities
{
    public partial class LogDeleter
    {

        public void DeleteLog(DateOnly date)
        {
            string logFilePath = GetLogFilePath(date);

            if (!File.Exists(logFilePath))
            {
                throw new FileNotFoundException($"Log file not found for {date:yyyy-MM-dd}", logFilePath);
            }

            File.Delete(logFilePath);
        }

        public List<DeletedFilesStatus> DeleteLogsInRange(DateOnly startDate, DateOnly endDate)
        {
            var deletedFiles = new List<DeletedFilesStatus>();
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                string logFilePath = GetLogFilePath(date);

                try
                {
                    if (File.Exists(logFilePath))
                    {
                        File.Delete(logFilePath);
                        deletedFiles.Add(new DeletedFilesStatus(date, true, string.Empty));
                    }
                    else
                    {
                        deletedFiles.Add(new DeletedFilesStatus(date, false, $"Log file not found for {date:yyyy-MM-dd}"));
                    }
                }
                catch (Exception ex)
                {
                    deletedFiles.Add(new DeletedFilesStatus(date, false, $"Error deleting log file for {date:yyyy-MM-dd}: {ex.Message}"));
                }
            }
            return deletedFiles;
        }

        private string GetLogFilePath(DateOnly date)
        {
            string logFileName = $"log-{date:yyyyMMdd}.json";
            return Path.Combine(Directory.GetCurrentDirectory(), "logs", logFileName);
        }
    }
}
