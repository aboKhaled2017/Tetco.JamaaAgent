﻿using Application.Common.Interfaces;
using Domain.Common.Patterns;
using Domain.Common.Settings;

namespace Application.Agent.Queries.Students.GetStudentMetaData
{
    public sealed record GetStudentsMetaDataRes(string InstituteCode, string AgentVersion, IEnumerable<ViewsMetaData> ViewsMetaData);
    public sealed class GetAgentMetaDataQuery : IRequest<Result<GetStudentsMetaDataRes>>
    {
        public string SchemaName { get; set; }
        public List<string> Views { get; set; }
        //public int ProviderId { get; set; }
        //public string ConnectionStr { get; set; }
    }
    public sealed class GetStudentsMetaDataQueryHandler : IRequestHandler<GetAgentMetaDataQuery, Result<GetStudentsMetaDataRes>>
    {
        private readonly IStudentQuery _db;
        private readonly GeneralSetting _generalSetting;

        public GetStudentsMetaDataQueryHandler(IStudentQuery db, GeneralSetting generalSetting)
        {
            _db = db;
            _generalSetting = generalSetting;
        }
        public async Task<Result<GetStudentsMetaDataRes>> Handle(GetAgentMetaDataQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _db.GetColumnInformation(request.SchemaName, request.Views);
                return Result<GetStudentsMetaDataRes>.Success("data retreived successfully")
                    .WithData(new(_generalSetting.InstituteCode, _generalSetting.AgentVersion, result));
            }
            catch (Exception ex)
            {
                return Result<GetStudentsMetaDataRes>.Failure("500", ex.Message).WithData(new GetStudentsMetaDataRes(string.Empty,string.Empty,new List<ViewsMetaData>()));
            }

        }

    }

    public record ViewsMetaData(string ViewName, dynamic ColumnInformation, long TotalCount, DateTime? LastBatchUpdate);

}
