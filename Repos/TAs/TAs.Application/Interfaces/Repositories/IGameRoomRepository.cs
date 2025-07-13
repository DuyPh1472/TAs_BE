using TAs.Domain.Entities;
using TAs.Domain.IGenericRepo;

namespace TAs.Application.Interfaces.Repositories
{
    public interface IGameRoomRepository : IGenericRepository<GameRoom>
    {
        Task<GameRoom?> GetGameRoomByRoomId(Guid? roomId);
        Task<GameRoom?> GetGameRoomByUserId(Guid UserId);
        Task<List<GameRoom>> GetActiveRoomsAsync(string? searchTerm = null, Guid? categoryId = null, int page = 1, int pageSize = 20);
        Task<GameRoom?> GetRoomWithPlayersAsync(Guid roomId);
    }
}