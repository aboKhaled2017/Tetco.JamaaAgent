using Application.Common.Interfaces;
using Domain.Common.Patterns;

namespace Application.NaqelAgent.Queries.Students.GetStudentMetaData
{
    public sealed record GetStudentsMetaDataRes(IEnumerable<ViewsMetaData> ViewsMetaData);
    public sealed class GetStudentsMetaDataQuery : IRequest<Result<GetStudentsMetaDataRes>>
    {
        public List<string> Views { get; set; }
        public int ProviderId { get; set; }
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
            var result = await _db.GetColumnInformation(request.Views);
            return Result<GetStudentsMetaDataRes>.Success("data retreived successfully")
                .WithData(new(result));

        }

    }

    public record ViewsMetaData(dynamic ColumnInformation, long TotalCount, DateTime? LastBatchUpdate,string ViewName);

}
