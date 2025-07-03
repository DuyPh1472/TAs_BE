using Microsoft.EntityFrameworkCore;
using TAs.Application.Interfaces.Repositories;
using TAs.Domain.Entities;
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

        public async Task<int> CountCompleted(Guid? progressId)
        {
            if (progressId.HasValue)
                return await dbContext.Progresses.CountAsync(p => p.Id == progressId && p.ProgressStatus);
            else
                return await dbContext.Progresses.CountAsync(p => p.ProgressStatus);
        }

        public async Task<IEnumerable<Progress>> GetAllProgressAsync()
        {
            return await dbContext
            .Progresses
            .ToListAsync();
        }

        public async Task<Progress?> GetProgressByIdAndSentenceIndex(Guid progressId, int sentenceIndex)
        {
            return await dbContext.Progresses.FirstOrDefaultAsync(p => p.Id == progressId);
        }

        public async Task<Progress?> GetProgressByIdAsync(Guid progressId)
        {
            return await dbContext
            .Progresses
             .FirstOrDefaultAsync(p => p.Id == progressId);
        }

        public async Task<Progress?> GetProgressByUserAndLesson(Guid userId, Guid LessonId)
        {
            var progress = await dbContext
            .Progresses
            .FirstOrDefaultAsync(p => p.UserId == userId
            && p.LessonId == LessonId);
            return progress;
        }

        public async Task<List<Progress>> GetProgressesByUser(Guid userId)
        {
            return await dbContext
            .Progresses
            .Where(p => p.UserId == userId).ToListAsync();
        }
    }
}