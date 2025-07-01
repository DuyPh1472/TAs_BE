using Microsoft.EntityFrameworkCore;
using TAs.Domain.Entities;
using TAs.Domain.Repositories;
using TAs.Infrastructure.Persistence;
using TAs.Infrastructure.Persistence.GenericRepo;

namespace TAs.Infrastructure.Repositories
{
   public class ProgressDetailRepository : GenericRepository<ProgressDetail>, IProGressDetailRepository                    
   {
       private readonly TAsDbContext dbContext;
        public ProgressDetailRepository(TAsDbContext context) : base(context)
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

        public async Task<List<ProgressDetail>> GetByProgressIdAsync(Guid progressId)
        {
            return await dbContext.ProgressDetails.Where(pd => pd.ProgressId == progressId).ToListAsync();
        }

        public async Task DeleteByProgressIdAsync(Guid progressId)
        {
            var details = dbContext.ProgressDetails.Where(pd => pd.ProgressId == progressId);
            dbContext.ProgressDetails.RemoveRange(details);
            await dbContext.SaveChangesAsync();
        }
    }
}