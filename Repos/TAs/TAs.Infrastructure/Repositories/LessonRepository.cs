using Microsoft.EntityFrameworkCore;
using TAs.Domain.Entities;
using TAs.Domain.Repositories;
using TAs.Infrastructure.Persistence;
using TAs.Infrastructure.Persistence.GenericRepo;

namespace TAs.Infrastructure.Repositories
{
    public class LessonRepository : GenericRepository<Lesson>, ILessonRepository
    {
        private readonly TAsDbContext dbContext;
        public LessonRepository(TAsDbContext context) : base(context)
        {
            dbContext = context;
        }


        public async Task<Lesson?> GetLessonsById(Guid lessonId)
        {
            return await dbContext.Lessons
            .Include(l => l.DictationSentences)
            .FirstOrDefaultAsync(l => l.Id == lessonId);
        }
    }
}