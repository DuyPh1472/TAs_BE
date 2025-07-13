using Microsoft.EntityFrameworkCore;
using TAs.Application.Interfaces.Repositories;
using TAs.Domain.Entities;
using TAs.Infrastructure.Persistence;
using TAs.Infrastructure.Persistence.GenericRepo;

namespace TAs.Infrastructure.Repositories
{
    public class GameSessionRepository : GenericRepository<GameSession>, IGameSessionRepository
    {
        private readonly TAsDbContext context;
        public GameSessionRepository(TAsDbContext dbContext) : base(dbContext)
        {
            context = dbContext;
        }

        public async Task<GameSession?> GetByRoomIdAsync(Guid roomId)
        {
            return await context.GameSessions
                .Include(gs => gs.Room)
                .Include(gs => gs.Lesson)
                .FirstOrDefaultAsync(gs => gs.RoomId == roomId);
        }

        public async Task<List<GameSession>> GetActiveSessionsAsync()
        {
            return await context.GameSessions
                .Include(gs => gs.Room)
                .Include(gs => gs.Lesson)
                .Where(gs => gs.Status == Domain.Enums.GameStatus.Playing || gs.Status == Domain.Enums.GameStatus.Starting)
                .ToListAsync();
        }

        public async Task<GameSession?> GetActiveSessionByRoomIdAsync(Guid roomId)
        {
            return await context.GameSessions
                .Include(gs => gs.Room)
                .Include(gs => gs.Lesson)
                .FirstOrDefaultAsync(gs => gs.RoomId == roomId && 
                    (gs.Status == Domain.Enums.GameStatus.Playing || gs.Status == Domain.Enums.GameStatus.Starting));
        }
    }
} 