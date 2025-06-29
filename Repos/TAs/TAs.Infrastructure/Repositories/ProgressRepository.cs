using TAs.Domain.Entities;
using TAs.Domain.Repositories;
using TAs.Infrastructure.Persistence;
using TAs.Infrastructure.Persistence.GenericRepo;

namespace TAs.Infrastructure.Repositories
{
    public class ProgressRepository : GenericRepository<Progress>, IProgressRepository
    {
        public ProgressRepository(TAsDbContext context) : base(context)
        {
        }
    }
}