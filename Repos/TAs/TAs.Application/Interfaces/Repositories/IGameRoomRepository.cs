using TAs.Domain.Entities;
using TAs.Domain.IGenericRepo;

namespace TAs.Application.Interfaces.Repositories
{
    public interface IGameRoomRepository : IGenericRepository<GameRoom>
    {
        Task<GameRoom?> GetGameRoomByRoomId(Guid? roomId);
    }
}