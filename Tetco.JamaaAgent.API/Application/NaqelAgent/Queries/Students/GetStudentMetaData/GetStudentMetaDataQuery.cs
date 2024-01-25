using Application.Common.Interfaces;
using Domain.Common.Patterns;
using Domain.Common.Settings;

namespace Application.NaqelAgent.Queries.Students.GetStudentMetaData
{
    public sealed record GetStudentsMetaDataRes(string InstituteCode,string AgentVersion, IEnumerable<ViewsMetaData> ViewsMetaData);
    public sealed class GetStudentsMetaDataQuery : IRequest<Result<GetStudentsMetaDataRes>>
    {
        public string SchemaName { get; set; }
        public List<string> Views { get; set; }
        //public int ProviderId { get; set; }
        //public string ConnectionStr { get; set; }
    }
    public sealed class GetStudentsMetaDataQueryHandler : IRequestHandler<GetStudentsMetaDataQuery, Result<GetStudentsMetaDataRes>>
    {
        private readonly IStudentQuery _db;
        private readonly GeneralSetting _generalSetting;

        public GetStudentsMetaDataQueryHandler(IStudentQuery db , GeneralSetting generalSetting)
        {
            _db = db;
            _generalSetting = generalSetting;
        }
        public async Task<Result<GetStudentsMetaDataRes>> Handle(GetStudentsMetaDataQuery request, CancellationToken cancellationToken)
        {
            var result = await _db.GetColumnInformation(request.SchemaName,request.Views);
            return Result<GetStudentsMetaDataRes>.Success("data retreived successfully")
                .WithData(new(_generalSetting.InstituteCode,_generalSetting.AgentVersion,result));

        }

    }

    public record ViewsMetaData(string ViewName,dynamic ColumnInformation, long TotalCount, DateTime? LastBatchUpdate);

}
