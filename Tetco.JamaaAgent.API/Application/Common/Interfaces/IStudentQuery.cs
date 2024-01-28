using Application.NaqelAgent.Queries.Students.GetPage;
using Application.NaqelAgent.Queries.Students.GetStudentMetaData;

namespace Application.Common.Interfaces
{
    public interface IStudentQuery
    {
        Task<IEnumerable<ViewDetail>> GetAllAsync(int pageSize, int pageNumber, string schemaName, string masterViewName, List<string> relatedViews, string associationColumnName, string columnNameFilter, string from, string to);
        Task<IEnumerable<ViewsMetaData>> GetColumnInformation(string schemaName,List<string> views);
        Task<IEnumerable<ViewDynamicData>> GetDynamicInformation(string query, IEnumerable<Paramter> paramters,int noOfQueries);
    }
}
