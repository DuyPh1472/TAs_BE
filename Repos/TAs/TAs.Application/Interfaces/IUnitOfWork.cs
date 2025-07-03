using TAs.Application.Interfaces.Repositories;

namespace TAs.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ISkillRepository SkillRepository { get; }
        IAchievementRepository AchievementRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ICategoryLessonRepository CategoryLessonRepository { get; }
        ILessonRepository LessonRepository { get; }
        IProgressRepository ProgressRepository { get; }
        ISkillLessonRepository SkillLessonRepository { get; }
        IUserRepository UserRepository { get; }
        IUserAchievementRepository UserAchievementRepository { get; }
        Task SaveChangesAsync();
    }

}