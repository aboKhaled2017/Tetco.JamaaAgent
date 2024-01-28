using Application.NaqelAgent.Queries.Students.GetDynamicQueryData;
using Domain.Enums;

namespace Application.Common.Interfaces
{
    public interface IDefineProviderManager
    {
        Task<IEnumerable<ViewDynamicData>> GetDynamicInformation(string QueryStr, IEnumerable<Paramter> parameters, int NoOfQueries,DBProvider Provider, SchemaType schemaType);
    }
}
