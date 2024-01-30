namespace Application.Common.Utilities
{
    public partial class LogDeleter
    {
        public record DeletedFilesStatus
        {
            public DateOnly Date { get; }
            public bool IsDeleted { get; }
            public string Message { get; }

            public DeletedFilesStatus(DateOnly date, bool isDeleted, string message)
            {
                Date = date;
                IsDeleted = isDeleted;
                Message = message;
            }
        }
    }
}
