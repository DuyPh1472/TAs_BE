using Microsoft.EntityFrameworkCore;
using TAs.Application.Interfaces.Repositories;
using TAs.Domain.Entities;
using TAs.Infrastructure.Persistence;
using TAs.Infrastructure.Persistence.GenericRepo;

namespace TAs.Infrastructure.Repositories
{
    public class ChatMessageRepository : GenericRepository<ChatMessage>, IChatMessageRepository
    {
        private readonly TAsDbContext context;
        public ChatMessageRepository(TAsDbContext dbContext) : base(dbContext)
        {
            context = dbContext;
        }

        public async Task<List<ChatMessage>> GetMessagesByRoomIdAsync(Guid roomId, int limit = 50)
        {
            return await context.ChatMessages
                .Include(cm => cm.User)
                .Where(cm => cm.RoomId == roomId)
                .OrderByDescending(cm => cm.SentAt)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<List<ChatMessage>> GetMessagesByRoomIdAfterAsync(Guid roomId, DateTime after, int limit = 50)
        {
            return await context.ChatMessages
                .Include(cm => cm.User)
                .Where(cm => cm.RoomId == roomId && cm.SentAt > after)
                .OrderByDescending(cm => cm.SentAt)
                .Take(limit)
                .ToListAsync();
        }
    }
} 