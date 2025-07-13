using TAs.Domain.Entities;
using TAs.Domain.IGenericRepo;

namespace TAs.Application.Interfaces.Repositories
{
    public interface IGameSessionRepository : IGenericRepository<GameSession>
    {
        Task<GameSession?> GetByRoomIdAsync(Guid roomId);
        Task<List<GameSession>> GetActiveSessionsAsync();
        Task<GameSession?> GetActiveSessionByRoomIdAsync(Guid roomId);
    }
} 