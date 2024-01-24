using Application.Common.Interfaces;
using Domain.Common.Patterns;

namespace Application.NaqelAgent.Queries.Students.GetPage
{
    public sealed record GetPageOfStudentRes(string instituteCode, IEnumerable<ViewDetail> ViewDetails);
    public sealed class GetPageOfStudentQuery : IRequest<Result<GetPageOfStudentRes>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 100;
        public DateTime? LastBatchUpdate { get; set; }
        public string MasterView { get; set; }
        public List<string> RelatedViews { get; set; }
        public string AssociationColumnName { get; set; }
        public string ColumnNameFilter { get; set; }
        public int ProviderId { get; set; }

    }
    public sealed class GetPageOfStudentQueryHandler : IRequestHandler<GetPageOfStudentQuery, Result<GetPageOfStudentRes>>
    {
        private readonly IStudentQuery _db;

        public GetPageOfStudentQueryHandler(IStudentQuery db)
        {
            _db = db;
        }

        public async Task<Result<GetPageOfStudentRes>> Handle(GetPageOfStudentQuery request, CancellationToken cancellationToken)
        {
            var students = await _db.GetAllAsync(request.PageSize, request.PageNumber, request.LastBatchUpdate, request.MasterView, request.RelatedViews, request.AssociationColumnName,request.ColumnNameFilter);
            return Result<GetPageOfStudentRes>.Success("data retreived successfully")
                .WithData(new("1234", students));
        }

    }
    public record ViewDetail(dynamic ViewData, string ViewName);

}
