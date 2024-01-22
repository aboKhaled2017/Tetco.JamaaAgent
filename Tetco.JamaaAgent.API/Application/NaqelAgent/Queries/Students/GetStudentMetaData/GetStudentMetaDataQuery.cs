using Application.Common.Interfaces;
using Domain.Common.Patterns;

namespace Application.NaqelAgent.Queries.Students.GetStudentMetaData
{
    public sealed record GetStudentsMetaDataRes(long totalItems,DateTime LastBatchUpdate);
    public sealed class GetStudentsMetaDataQuery : IRequest<Result<GetStudentsMetaDataRes>>
    {
    }
    public sealed class GetStudentsMetaDataQueryHandler : IRequestHandler<GetStudentsMetaDataQuery, Result<GetStudentsMetaDataRes>>
    {
        private readonly IStudentQuery _db;

        public GetStudentsMetaDataQueryHandler(IStudentQuery db)
        {
            _db = db;
        }
        public async Task<Result<GetStudentsMetaDataRes>> Handle(GetStudentsMetaDataQuery request, CancellationToken cancellationToken)
        {
            await Task.Delay(1);
            var result = await _db.GetTotalCount();
            return Result<GetStudentsMetaDataRes>.Success("data retreived successfully")
                .WithData(new(result.TotalCount , result.LastBatchUpdate));

        }

    }


}
