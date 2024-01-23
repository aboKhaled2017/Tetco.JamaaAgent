using Application.NaqelAgent.Models;

namespace Application.Common.Interfaces
{
    public interface IStudentQuery
    {
        Task<IEnumerable<Dictionary<string, object>>> GetAllAsync(int pageSize,int pageNumber, DateTime? LastBatchUpdate);
        Task<(IEnumerable<Dictionary<string, object>> ColumnInformation, long totalCount, DateTime? lastBatchUpdate)> GetColumnInformation();
    }
}
