using Application.NaqelAgent.Queries.Students.GetPage;
using Application.NaqelAgent.Queries.Students.GetStudentMetaData;

namespace Application.Common.Interfaces
{
    public interface IStudentQuery
    {
        Task<IEnumerable<ViewDetail>> GetAllAsync(int pageSize,int pageNumber, DateTime? lastBatchUpdate, string masterViewName , List<string> RelatedViews ,string associationColumnName, string columnNameFilter);
        Task<IEnumerable<ViewsMetaData>> GetColumnInformation(List<string> views);
    }
}
