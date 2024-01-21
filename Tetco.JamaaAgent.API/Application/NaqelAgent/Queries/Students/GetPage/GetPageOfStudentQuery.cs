using Application.NaqelAgent.Models;

using Domain.Common.Patterns;

namespace Application.NaqelAgent.Queries.Students.GetPage
{
    public sealed record GetPageOfStudentRes(string instituteCode, List<Student> students);
    public sealed class GetPageOfStudentQuery : IRequest<Result<GetPageOfStudentRes>>
    {
        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 100;
    }
    public sealed class GetPageOfStudentQueryHandler : IRequestHandler<GetPageOfStudentQuery, Result<GetPageOfStudentRes>>
    {
        //private readonly INaqelAgentContext _db;

        //public GetPageOfStudentQueryHandler(INaqelAgentContext db)
        //{
        //    _db = db;
        //}

        public async Task<Result<GetPageOfStudentRes>> Handle(GetPageOfStudentQuery request, CancellationToken cancellationToken)
        {
            await Task.Delay(1);

            var totalRecords = 1000;
            //int totalPages = (int)Math.Ceiling((double)totalRecords / request.pageSize);

            var students = new DataGenerator().GenerateStudents(totalRecords)
                .Skip((request.pageNumber - 1) * request.pageSize)
                .Take(request.pageSize)
                .ToList();


            return Result<GetPageOfStudentRes>.Success("data retreived successfully")
                .WithData(new("1234", students));

        }

    }


}
