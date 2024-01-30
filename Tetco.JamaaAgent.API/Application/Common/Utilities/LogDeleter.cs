namespace Application.Common.Utilities
{
    public class LogDeleter
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

        public void DeleteLogsInRange(DateOnly startDate, DateOnly endDate)
        {
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                string logFilePath = GetLogFilePath(date);

                try
                {
                    if (File.Exists(logFilePath))
                    {
                        File.Delete(logFilePath);
                        Console.WriteLine($"Deleted log file for {date:yyyy-MM-dd}");
                    }
                    else
                    {
                        Console.WriteLine($"Log file not found for {date:yyyy-MM-dd}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting log file for {date:yyyy-MM-dd}: {ex.Message}");
                }
            }
        }

        private string GetLogFilePath(DateOnly date)
        {
            string logFileName = $"log-{date:yyyyMMdd}.json";
            return Path.Combine(Directory.GetCurrentDirectory(), "logs", logFileName);
        }

    }
}
