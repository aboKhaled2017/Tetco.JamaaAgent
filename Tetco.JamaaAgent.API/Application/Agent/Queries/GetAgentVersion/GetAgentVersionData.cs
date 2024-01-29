using Domain.Common.Patterns;
using Domain.Common.Settings;

namespace Application.Agent.Queries.GetAgentVersion
{
    public sealed record GetAgentVersionRes(string Version);
    public sealed class GetAgentVersionReq : IRequest<Result<GetAgentVersionRes>>
    {

    }
    public sealed class GetAgentVersionHandler : IRequestHandler<GetAgentVersionReq, Result<GetAgentVersionRes>>
    {
        public GeneralSetting _generalSetting { get; set; }
        public GetAgentVersionHandler(GeneralSetting generalSetting)
        {
            _generalSetting = generalSetting;
        }

        public async Task<Result<GetAgentVersionRes>> Handle(GetAgentVersionReq request, CancellationToken cancellationToken)
        {
            Task.CompletedTask.Wait();
            var result = _generalSetting.AgentVersion;
            return Result<GetAgentVersionRes>.Success("data retreived successfully")
               .WithData(new(result));
        }


    }

}
