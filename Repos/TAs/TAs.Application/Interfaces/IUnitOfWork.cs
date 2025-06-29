using TAs.Domain.Repositories;

namespace TAs.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ISkillRepository SkillRepository { get; }
        Task SaveChangesAsync();
    }

}