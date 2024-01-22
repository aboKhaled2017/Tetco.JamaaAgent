using Application.NaqelAgent.Models;

namespace Application.Common.Interfaces
{
    public interface IStudentQuery
    {
        Task<IEnumerable<Student>> GetAllAsync(int pageSize,int pageNumber, DateTime LastBatchUpdate);
        Task<long> GetTotalCount();
    }
}
