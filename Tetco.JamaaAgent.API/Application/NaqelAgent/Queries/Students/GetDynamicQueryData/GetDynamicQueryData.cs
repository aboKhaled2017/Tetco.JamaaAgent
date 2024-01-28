using Application.Common.Interfaces;
using Domain.Common.Patterns;

namespace Application.NaqelAgent.Queries.Students.GetStudentMetaData
{
    public sealed record GetDynamicQueryDataRes(IEnumerable<ViewDynamicData> ViewsMetaData);
    public sealed class GetDynamicQueryDataReq : IRequest<Result<GetDynamicQueryDataRes>>
    {
        public string QueryStr { get; set; }
        public int NoOfQueries { get; set; }
        public IEnumerable<Paramter> Parameters { get; set; }

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
            var result = await _db.GetDynamicInformation(request.QueryStr,request.Parameters,request.NoOfQueries);
            return Result<GetDynamicQueryDataRes>.Success("data retreived successfully")
                .WithData(new(result));

        }

    }

    public record ViewDynamicData(string QueryName,dynamic Data, int TotalCount);

    public record Paramter(string ParamterName,string Value);

}
