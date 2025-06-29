using TAs.Domain.Entities;
using TAs.Domain.Repositories;
using TAs.Infrastructure.Persistence;
using TAs.Infrastructure.Persistence.GenericRepo;

namespace TAs.Infrastructure.Repositories
{
    public class UserAchievementRepository : GenericRepository<UserAchievement>, IUserAchievementRepository
    {
        public UserAchievementRepository(TAsDbContext context) : base(context)
        {
        }
    }
}