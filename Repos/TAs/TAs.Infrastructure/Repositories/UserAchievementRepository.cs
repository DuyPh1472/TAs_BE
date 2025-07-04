using TAs.Application.Interfaces.Repositories;
using TAs.Domain.Entities;
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