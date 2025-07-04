using TAs.Application.Interfaces.Repositories;

namespace TAs.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ISkillRepository SkillRepository { get; }
        IAchievementRepository AchievementRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ILessonRepository LessonRepository { get; }
        IProgressRepository ProgressRepository { get; }
        IUserRepository UserRepository { get; }
        IUserAchievementRepository UserAchievementRepository { get; }
        Task SaveChangesAsync();
    }

}