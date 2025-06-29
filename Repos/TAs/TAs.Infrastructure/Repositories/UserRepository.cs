using TAs.Domain.Entities;
using TAs.Domain.Repositories;
using TAs.Infrastructure.Persistence;
using TAs.Infrastructure.Persistence.GenericRepo;

namespace TAs.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(TAsDbContext context) : base(context)
        {
        }
    }
}