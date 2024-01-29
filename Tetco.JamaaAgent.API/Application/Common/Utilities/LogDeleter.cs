namespace Application.Common.Utilities
{
    public class LogDeleter
    {

        public void DeleteLog(DateOnly date)
        {
            string logFileName = $"log-{date:yyyyMMdd}.json";
            string logDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "logs");
            string logFilePath = Path.Combine(logDirectoryPath, logFileName);

            if (!File.Exists(logFilePath))
            {
                throw new FileNotFoundException($"Log file not found for {date:yyyy-MM-dd}", logFileName);
            }

            File.Delete(logFilePath);
        }
    }
}
