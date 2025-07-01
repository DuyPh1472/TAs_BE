using Microsoft.EntityFrameworkCore;
using TAs.Domain.Entities;
using TAs.Domain.Repositories;
using TAs.Infrastructure.Persistence;
using TAs.Infrastructure.Persistence.GenericRepo;

namespace TAs.Infrastructure.Repositories
{
    public class ProgressRepository : GenericRepository<Progress>, IProgressRepository
    {
        private readonly TAsDbContext dbContext;
        public ProgressRepository(TAsDbContext context) : base(context)
        {
            dbContext = context;
        }

        public async Task<IEnumerable<Progress>> GetAllProgressAsync()
        {
            return await dbContext
            .Progresses
            .Include(p => p.ProgressDetails)
            .ToListAsync();
        }

        public async Task<Progress?> GetProgressByIdAndSentenceIndex(Guid progressId, int sentenceIndex)
        {
            return await dbContext
            .Progresses
            .Include(p => p.ProgressDetails)
            .FirstOrDefaultAsync(p => p.ProgressId == progressId &&
             p.ProgressDetails
            .Any(pd => pd.SentenceIndex == sentenceIndex));


        }

        public async Task<Progress?> GetProgressByUserAndLesson(Guid userId, Guid LessonId)
        {
            var progress = await dbContext
            .Progresses
            .FirstOrDefaultAsync(p => p.UserId == userId
            && p.LessonId == LessonId);
            return progress;
        }
    }
}