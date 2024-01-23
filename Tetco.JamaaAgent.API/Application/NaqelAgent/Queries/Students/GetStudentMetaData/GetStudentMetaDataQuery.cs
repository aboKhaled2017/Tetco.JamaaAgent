using Application.Common.Interfaces;
using Domain.Common.Patterns;

namespace Application.NaqelAgent.Queries.Students.GetStudentMetaData
{
    public sealed record GetStudentsMetaDataRes(IEnumerable<Dictionary<string, object>> ColumnInformation, long totalCount, DateTime? lastBatchUpdate);
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
            var result = await _db.GetColumnInformation();
            return Result<GetStudentsMetaDataRes>.Success("data retreived successfully")
                .WithData(new(result.ColumnInformation,result.totalCount,result.lastBatchUpdate));

        }

    }


}
