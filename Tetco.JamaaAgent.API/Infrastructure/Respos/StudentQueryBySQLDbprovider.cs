using Application.Common.Interfaces;
using Application.NaqelAgent.Models;
using Domain.Common.Settings;
using MediatR;


namespace Infrastructure.Respos
{
    internal sealed class StudentQueryBySQLDbprovider : IStudentQuery
    {
        //private readonly GeneralSetting _generalSetting;

        public StudentQueryBySQLDbprovider(/*GeneralSetting generalSetting*/)
        {
            //_generalSetting = generalSetting;
        }

        public async Task<(long TotalCount, DateTime LastBatchUpdate)> GetTotalCount()
        {
            await Task.Delay(1);
            long totalCount = 1000;
            return (totalCount,DateTime.Now);
        }

        public async Task<IEnumerable<Student>> GetAllAsync(int pageSize , int pageNumber, DateTime LastBatchUpdate)
        {
            await Task.Delay(1);
            return new DataGenerator().GenerateStudents(1000)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
                .ToList();
        }
    }
}
