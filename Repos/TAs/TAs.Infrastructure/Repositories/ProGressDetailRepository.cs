using Microsoft.EntityFrameworkCore;
using TAs.Domain.Entities;
using TAs.Domain.Repositories;
using TAs.Infrastructure.Persistence;
using TAs.Infrastructure.Persistence.GenericRepo;

namespace TAs.Infrastructure.Repositories
{
    public class ProGressDetailRepository : GenericRepository<ProgressDetail>, IProGressDetailRepository
    {
        private readonly TAsDbContext dbContext;
        public ProGressDetailRepository(TAsDbContext context) : base(context)
        {
            dbContext = context;
        }

        public async Task<ProgressDetail?> GetByProgressAndSentence(Guid progressId, int SentenceIndex)
        {
            return await dbContext
            .ProgressDetails
            .FirstOrDefaultAsync(pd => pd.ProgressId == progressId &&
            pd.SentenceIndex == SentenceIndex);
        }

        public async Task<int> CountCompleted(Guid? progressId)
        {
            return await dbContext
            .ProgressDetails
            .CountAsync(pd => pd.ProgressId == progressId && pd.IsCompleted);
        }
    }
}