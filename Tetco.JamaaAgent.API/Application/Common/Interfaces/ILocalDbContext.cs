using Application.NaqelAgent.Models;

namespace Application.Common.Interfaces
{
    public interface ILocalDbContext
    {
        DbSet<Student> Students { get; set; }
        Task SaveAsync(CancellationToken cancellationToken = default);
    }
}
