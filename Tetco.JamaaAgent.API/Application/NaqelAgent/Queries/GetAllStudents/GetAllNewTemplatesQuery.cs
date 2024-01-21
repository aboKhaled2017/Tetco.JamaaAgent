using Domain.Common.Patterns;

namespace Application.NaqelAgent.Queries.GetAllStudents
{
    public sealed record GetAllStudentsRes(int totalItems, int nextPage, int totalPages, bool hasNextPage, object records);
    public sealed class GetAllStudentsQuery : IRequest<Result<GetAllStudentsRes>>
    {
        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 100;
        public string InstituteCode { get; set; }
    }
    public sealed class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, Result<GetAllStudentsRes>>
    {
        //private readonly INaqelAgentContext _db;

        //public GetAllStudentsQueryHandler(INaqelAgentContext db)
        //{
        //    _db = db;
        //}

        public async Task<Result<GetAllStudentsRes>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
        {
            await Task.Delay(1);

            var totalRecords = 100000;
            int totalPages = (int)Math.Ceiling((double)totalRecords / request.pageSize);

            var students = new DataGenerator().GenerateStudents(totalRecords)
                .Skip((request.pageNumber - 1) * request.pageSize)
                .Take(request.pageSize)
                .ToList();

            var data = new { request.InstituteCode, Students = students, UniversityName="جامعة الملك سعود" };

            return Result<GetAllStudentsRes>.Success("data retreived successfully")
                .WithData(
                   new(totalRecords,
                       totalPages > request.pageNumber ? request.pageNumber + 1 : request.pageNumber,
                       totalPages,
                       request.pageNumber < totalPages,
                       data));

        }

    }


}
