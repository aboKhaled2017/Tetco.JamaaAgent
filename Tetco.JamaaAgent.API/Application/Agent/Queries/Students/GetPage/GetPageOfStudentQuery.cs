using Application.Common.Interfaces;
using Domain.Common.Patterns;

namespace Application.Agent.Queries.Students.GetPage
{
    public sealed record GetPageOfStudentRes(IEnumerable<ViewDetail> ViewDetails);
    public sealed class GetPageOfStudentQuery : IRequest<Result<GetPageOfStudentRes>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 100;
        public string SchemaName { get; set; }
        public string MasterView { get; set; }
        public List<string> RelatedViews { get; set; }
        public string AssociationColumnName { get; set; }
        public string ColumnNameFilter { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        //public int ProviderId { get; set; }
        //public string ConnectionStr { get; set; }

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
            var students = await _db.GetAllAsync(request.PageSize, request.PageNumber, request.SchemaName, request.MasterView, request.RelatedViews, request.AssociationColumnName, request.ColumnNameFilter, request.From, request.To);
            return Result<GetPageOfStudentRes>.Success("data retreived successfully")
                .WithData(new(students));
        }

    }
    public record ViewDetail(dynamic ViewData, string ViewName);

}
