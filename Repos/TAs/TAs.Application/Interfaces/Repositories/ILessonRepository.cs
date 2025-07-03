using TAs.Domain.Entities;
using TAs.Domain.IGenericRepo;

namespace TAs.Domain.Repositories
{
    public interface ILessonRepository : IGenericRepository<Lesson>
    {
        Task<Lesson?> GetLessonsById(Guid lessonId);
    }
}