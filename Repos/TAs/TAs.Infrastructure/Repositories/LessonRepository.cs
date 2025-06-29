using TAs.Domain.Entities;
using TAs.Domain.Repositories;
using TAs.Infrastructure.Persistence;
using TAs.Infrastructure.Persistence.GenericRepo;

namespace TAs.Infrastructure.Repositories
{
    public class LessonRepository : GenericRepository<Lesson>, ILessonRepository
    {
        public LessonRepository(TAsDbContext context) : base(context)
        {
        }
    }
}