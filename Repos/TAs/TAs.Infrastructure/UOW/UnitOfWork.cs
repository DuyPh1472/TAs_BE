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

        public IGameRoomRepository _gameRoomRepository = null!;
        public IGameRoomRepository GameRoomRepository
        {
            get
            {
                if (_gameRoomRepository is null)
                {
                    _gameRoomRepository = new GameRoomRepository(context);
                }
                return _gameRoomRepository;
            }
        }


        public IPlayerInRoomRepository _playerInRoomRepository = null!;
        public IPlayerInRoomRepository PlayerInRoomRepository
        {
            get
            {
                if (_playerInRoomRepository is null)
                {
                    _playerInRoomRepository = new PlayerInRoomRepository(context);
                }
                return _playerInRoomRepository;
            }
        }


        public IPlayerScoreRepository _playerScoreRepository = null!;
        public IPlayerScoreRepository PlayerScoreRepository
        {
            get
            {
                if (_playerScoreRepository is null)
                {
                    _playerScoreRepository = new PlayerScoreRepository(context);
                }
                return _playerScoreRepository;
            }
        }

        public IGameSessionRepository _gameSessionRepository = null!;
        public IGameSessionRepository GameSessionRepository
        {
            get
            {
                if (_gameSessionRepository is null)
                {
                    _gameSessionRepository = new GameSessionRepository(context);
                }
                return _gameSessionRepository;
            }
        }

        public IChatMessageRepository _chatMessageRepository = null!;
        public IChatMessageRepository ChatMessageRepository
        {
            get
            {
                if (_chatMessageRepository is null)
                {
                    _chatMessageRepository = new ChatMessageRepository(context);
                }
                return _chatMessageRepository;
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