using TAs.Domain.Entities;
using TAs.Domain.IGenericRepo;

namespace TAs.Domain.Repositories
{
    public interface IProgressRepository : IGenericRepository<Progress>
    {
        Task<Progress?> GetProgressByUserAndLesson(Guid userId, Guid LessonId);
        Task<IEnumerable<Progress>> GetAllProgressAsync();
        Task<Progress?> GetProgressByIdAndSentenceIndex(Guid progressId, int sentenceIndex);
    }
}