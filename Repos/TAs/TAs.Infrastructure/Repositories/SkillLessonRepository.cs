using TAs.Domain.Entities;
using TAs.Domain.Repositories;
using TAs.Infrastructure.Persistence;
using TAs.Infrastructure.Persistence.GenericRepo;

namespace TAs.Infrastructure.Repositories
{
    public class SkillLessonRepository : GenericRepository<SkillLesson>, ISkillLessonRepository
    {
        public SkillLessonRepository(TAsDbContext context) : base(context)
        {
        }
    }
}