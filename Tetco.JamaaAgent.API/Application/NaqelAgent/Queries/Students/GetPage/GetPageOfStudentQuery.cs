using Application.Common.Interfaces;

using Domain.Common.Patterns;

namespace Application.NaqelAgent.Queries.Students.GetPage
{
    public sealed record GetPageOfStudentRes(string instituteCode, IEnumerable<Dictionary<string, object>> students);
    public sealed class GetPageOfStudentQuery : IRequest<Result<GetPageOfStudentRes>>
    {
        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 100;

        public DateTime? LastBatchUpdate { get; set; }
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
            var students = await _db.GetAllAsync(request.pageSize,request.pageNumber,request.LastBatchUpdate);
            return Result<GetPageOfStudentRes>.Success("data retreived successfully")
                .WithData(new("1234", students));

        }

    }


}
