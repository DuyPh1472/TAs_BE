using TAs.Domain.Entities;
using TAs.Domain.IGenericRepo;

namespace TAs.Application.Interfaces.Repositories
{
    public interface IPlayerInRoomRepository : IGenericRepository<PlayerInRoom>
    {
        Task<PlayerInRoom?> GetPlayerInRoomByUserAndRoom(Guid? userId, Guid? RoomId);
        Task<bool> CheckPlayerIsHost(Guid userId);
        Task<List<PlayerInRoom>> GetPlayerInRoomsByRoomId(Guid roomId);
    }
}