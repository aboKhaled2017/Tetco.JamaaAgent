using Application.AgentLogs.Queries.GetAgentLogsFiles;
using Application.Common.Interfaces;
using Domain.Common.Constants;
using Domain.Common.Patterns;
using Domain.Enums;
using MySqlX.XDevAPI.Common;

namespace Application.Agent.Queries.GetDynamicQueryData
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
            try
            {
                var result = await _defineProvider.GetDynamicInformation(request.QueryStr, request.Parameters, request.NoOfQueries, request.Provider, request.SchemaType);
                return Result<GetDynamicQueryDataRes>.Success("data retreived successfully")
                    .WithData(new(result));
            }
            catch (Exception ex)
            {
                return Result<GetDynamicQueryDataRes>.Failure("500", ex.Message).WithData(new GetDynamicQueryDataRes(new List<ViewDynamicData>()));
            }
          

        }


    }

    public record ViewDynamicData(string QueryName, dynamic Data, int TotalCount);

    public record Paramter(string ParamterName, string Value);

}
