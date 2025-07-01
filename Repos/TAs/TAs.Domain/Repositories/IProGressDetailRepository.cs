using TAs.Domain.Entities;
using TAs.Domain.IGenericRepo;

namespace TAs.Domain.Repositories
{
    public interface IProGressDetailRepository : IGenericRepository<ProgressDetail>
    {
        Task<ProgressDetail?> GetByProgressAndSentence(Guid progressId, int SentenceIndex);
        Task<int> CountCompleted(Guid? progressId);
        Task<List<ProgressDetail>> GetByProgressIdAsync(Guid progressId);
        Task DeleteByProgressIdAsync(Guid progressId);
    }
}