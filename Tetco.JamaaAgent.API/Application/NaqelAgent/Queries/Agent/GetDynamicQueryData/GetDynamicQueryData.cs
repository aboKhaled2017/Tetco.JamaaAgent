using Application.Common.Interfaces;
using Domain.Common.Patterns;
using Domain.Enums;

namespace Application.NaqelAgent.Queries.Agent.GetDynamicQueryData
{
    public sealed record GetDynamicQueryDataRes(IEnumerable<ViewDynamicData> ViewsMetaData);
    public sealed class GetDynamicQueryDataReq : IRequest<Result<GetDynamicQueryDataRes>>
    {
        public string QueryStr { get; set; }
        public int NoOfQueries { get; set; }
        public IEnumerable<Paramter> Parameters { get; set; }

        public DBProvider Provider { get; set; }
        public SchemaType SchemaType { get; set; }

    }
    public sealed class GetDynamicQueryDataHandler : IRequestHandler<GetDynamicQueryDataReq, Result<GetDynamicQueryDataRes>>
    {
        public IDefineProviderManager _defineProvider { get; set; }
        public GetDynamicQueryDataHandler(IDefineProviderManager defineProvider)
        {
            _defineProvider = defineProvider;
        }

        public async Task<Result<GetDynamicQueryDataRes>> Handle(GetDynamicQueryDataReq request, CancellationToken cancellationToken)
        {
            var result = await _defineProvider.GetDynamicInformation(request.QueryStr, request.Parameters, request.NoOfQueries, request.Provider, request.SchemaType);
            return Result<GetDynamicQueryDataRes>.Success("data retreived successfully")
                .WithData(new(result));

        }


    }

    public record ViewDynamicData(string QueryName, dynamic Data, int TotalCount);

    public record Paramter(string ParamterName, string Value);

}
