using TAs.Domain.Entities;
using TAs.Domain.IGenericRepo;

namespace TAs.Application.Interfaces.Repositories
{
    public interface IChatMessageRepository : IGenericRepository<ChatMessage>
    {
        Task<List<ChatMessage>> GetMessagesByRoomIdAsync(Guid roomId, int limit = 50);
        Task<List<ChatMessage>> GetMessagesByRoomIdAfterAsync(Guid roomId, DateTime after, int limit = 50);
    }
} 