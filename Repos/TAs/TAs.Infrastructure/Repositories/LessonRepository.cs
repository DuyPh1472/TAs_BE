using Microsoft.EntityFrameworkCore;
using TAs.Application.Interfaces.Repositories;
using TAs.Domain.Entities;
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

        public async Task<List<Lesson>> GetLessonsByCategoryTitle(string title)
        {
            return await dbContext
            .Lessons
            .Where(l => l.Category.Title == title).ToListAsync();
        }

        public async Task<Lesson?> GetLessonsById(Guid lessonId)
        {
            return await dbContext.Lessons
            .Include(l => l.DictationSentences)
            .FirstOrDefaultAsync(l => l.Id == lessonId);
        }

        public async Task<Lesson?> GetByIdAsync(Guid? lessonId)
        {
            return await dbContext.Lessons
            .Include(l => l.DictationSentences)
            .Include(l => l.Category)
            .FirstOrDefaultAsync(l => l.Id == lessonId);
        }

   
    }
}