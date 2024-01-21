//using Application.Common.Interfaces;
//using Application.Common.Models;
//using Application.NaqelTemplates.Commands.PostTemplateBatchToJamaaHub;
//using Domain.Constants;
//using Newtonsoft.Json;
//using System.Net.Http.Json;

//namespace Application.NaqelTemplates.Commands.AuthorizeToJamaaHub
//{
//    public record LoginJApiResult(bool success, string token, string message);
//    public sealed record TokenResult(string token);
//    public sealed record AuthorizeToJamaaHubCommand(string email, string password) : IRequest<Result<TokenResult>> { }
//    public sealed class AuthorizeToJamaaHubCommandHandler : IRequestHandler<AuthorizeToJamaaHubCommand, Result<TokenResult>>
//    {
//        private readonly INaqelAgentContext _db;
//        private readonly HttpClient _jamaaAgentHubClient;
//        public AuthorizeToJamaaHubCommandHandler(INaqelAgentContext db, IHttpClientFactory clientFactory)
//        {
//            _db = db;
//            _jamaaAgentHubClient = clientFactory.CreateClient(HtttClientNames.JamaaAgentHubClientName);
//        }

//        public async Task<Result<TokenResult>> Handle(AuthorizeToJamaaHubCommand request, CancellationToken cancellationToken)
//        {
//            var reqBody = new
//            {
//                Email = request.email,
//                Password = request.password
//            };

//            var response = await _jamaaAgentHubClient.PostAsJsonAsync($"auth/agent/login", reqBody, cancellationToken);

//            if (!response.IsSuccessStatusCode)
//                return Result<TokenResult>.Failure(NaqelErrorCodes.IntegrationHubError, response.ReasonPhrase);

//            var authRes= JsonConvert.DeserializeObject<LoginJApiResult>(await response.Content.ReadAsStringAsync());
            
//            if(!authRes.success)
//                return Result<TokenResult>.Failure(NaqelErrorCodes.IntegrationHubError, authRes.message);

//            return Result<TokenResult>.Success("token retrived successfully")
//                .WithData(new(authRes.token));
//        }
//    }
//}
