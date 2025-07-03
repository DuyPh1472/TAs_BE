using TAs.Domain.Entities;
using TAs.Domain.IGenericRepo;

namespace TAs.Application.Interfaces.Repositories
{
    public interface IProgressRepository : IGenericRepository<Progress>
    {
        Task<Progress?> GetProgressByUserAndLesson(Guid userId, Guid LessonId);
        Task<IEnumerable<Progress>> GetAllProgressAsync();
        Task<Progress?> GetProgressByIdAndSentenceIndex(Guid progressId, int sentenceIndex);
        Task<List<Progress>> GetProgressesByUser(Guid userId);
        Task<int> CountCompleted(Guid? progressId);
        Task<Progress?> GetProgressByIdAsync(Guid progressId);
    }
}