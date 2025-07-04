using TAs.Application.Interfaces;
using TAs.Application.Interfaces.Repositories;
using TAs.Infrastructure.Persistence;
using TAs.Infrastructure.Repositories;

namespace TAs.Infrastructure.UOW
{
    public class UnitOfWork(TAsDbContext context) : IUnitOfWork
    {
        private ISkillRepository _skillRepository = null!;
        public ISkillRepository SkillRepository
        {
            get
            {
                if (_skillRepository is null)
                {
                    _skillRepository = new SkillRepository(context);
                }
                return _skillRepository;
            }
        }

        public IAchievementRepository _achievementRepository = null!;
        public IAchievementRepository AchievementRepository
        {
            get
            {
                if (_achievementRepository is null)
                {
                    _achievementRepository = new AchievementRepository(context);
                }
                return _achievementRepository;
            }
        }

        public ICategoryRepository _categoryRepository = null!;
        public ICategoryRepository CategoryRepository
        {
            get
            {
                if (_categoryRepository is null)
                {
                    _categoryRepository = new CategoryRepository(context);
                }
                return _categoryRepository;
            }
        }
        public ILessonRepository _lessonRepository = null!;
        public ILessonRepository LessonRepository
        {
            get
            {
                if (_lessonRepository is null)
                {
                    _lessonRepository = new LessonRepository(context);
                }
                return _lessonRepository;
            }
        }
        public IProgressRepository _progressRepository = null!;
        public IProgressRepository ProgressRepository
        {
            get
            {
                if (_progressRepository is null)
                {
                    _progressRepository = new ProgressRepository(context);
                }
                return _progressRepository;
            }
        }

        public IUserRepository _userRepository = null!;
        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository is null)
                {
                    _userRepository = new UserRepository(context);
                }
                return _userRepository;
            }
        }

        public IUserAchievementRepository _userAchievementRepository = null!;
        public IUserAchievementRepository UserAchievementRepository
        {
            get
            {
                if (_userAchievementRepository is null)
                {
                    _userAchievementRepository = new UserAchievementRepository(context);
                }
                return _userAchievementRepository;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
                GC.SuppressFinalize(this);
            }
            ;
        }
    }
}