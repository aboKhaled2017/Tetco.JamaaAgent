using Application.NaqelAgent.Models;
using Domain.Common.Patterns;

namespace Application.NaqelAgent.Queries.Students.GetStudentMetaData
{
    public sealed record GetStudentsMetaDataRes(long totalItems,DateTime LastBatchUpdate);
    public sealed class GetStudentsMetaDataQuery : IRequest<Result<GetStudentsMetaDataRes>>
    {
    }
    public sealed class GetStudentsMetaDataQueryHandler : IRequestHandler<GetStudentsMetaDataQuery, Result<GetStudentsMetaDataRes>>
    {

        public async Task<Result<GetStudentsMetaDataRes>> Handle(GetStudentsMetaDataQuery request, CancellationToken cancellationToken)
        {
            await Task.Delay(1);
            long totalRecords = 1000;
            return Result<GetStudentsMetaDataRes>.Success("data retreived successfully")
                .WithData(new(totalRecords , DateTime.Now));

        }

    }


}
