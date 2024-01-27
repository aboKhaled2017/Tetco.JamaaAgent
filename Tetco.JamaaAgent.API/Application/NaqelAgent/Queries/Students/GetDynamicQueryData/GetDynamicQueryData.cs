using Application.Common.Interfaces;
using Domain.Common.Patterns;
using Domain.Common.Settings;

namespace Application.NaqelAgent.Queries.Students.GetStudentMetaData
{
    public sealed record GetDynamicQueryDataRes(IEnumerable<ViewDynamicData> ViewsMetaData);
    public sealed class GetDynamicQueryDataReq : IRequest<Result<GetDynamicQueryDataRes>>
    {
        public string Query { get; set; }
        public int NoOfQueries { get; set; }
        public Dictionary<string,string> Parameters { get; set; }

    }
    public sealed class GetDynamicQueryDataHandler : IRequestHandler<GetDynamicQueryDataReq, Result<GetDynamicQueryDataRes>>
    {
        private readonly IStudentQuery _db;

        public GetDynamicQueryDataHandler(IStudentQuery db)
        {
            _db = db;
        }
        public async Task<Result<GetDynamicQueryDataRes>> Handle(GetDynamicQueryDataReq request, CancellationToken cancellationToken)
        {
            var result = await _db.GetDynamicInformation(request.Query,request.Parameters,request.NoOfQueries);
            return Result<GetDynamicQueryDataRes>.Success("data retreived successfully")
                .WithData(new(result));

        }

    }

    public record ViewDynamicData(string QueryName,dynamic Data);

}
