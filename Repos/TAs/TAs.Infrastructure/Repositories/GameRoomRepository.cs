using Microsoft.EntityFrameworkCore;
using TAs.Application.Interfaces.Repositories;
using TAs.Domain.Entities;
using TAs.Infrastructure.Persistence;
using TAs.Infrastructure.Persistence.GenericRepo;

namespace TAs.Infrastructure.Repositories
{
    public class GameRoomRepository : GenericRepository<GameRoom>, IGameRoomRepository
    {
        private readonly TAsDbContext context;
        public GameRoomRepository(TAsDbContext dbContext) : base(dbContext)
        {
            context = dbContext;
        }

        public async Task<List<GameRoom>> GetActiveRoomsAsync(string? searchTerm = null, Guid? categoryId = null, int page = 1, int pageSize = 20)
        {
            var query = context.GameRooms
                .Include(gr => gr.Category)
                .Include(gr => gr.Host)
                .Include(gr => gr.PlayerInRooms)
                .Where(gr => gr.IsActive);

            // Filter by search term
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(gr => gr.RoomName.Contains(searchTerm) || gr.Host.FullName.Contains(searchTerm));
            }

            // Filter by category
            if (categoryId.HasValue)
            {
                query = query.Where(gr => gr.CategoryId == categoryId.Value);
            }

            // Apply pagination
            var skip = (page - 1) * pageSize;
            return await query
                .OrderByDescending(gr => gr.CreatedAt)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<GameRoom?> GetGameRoomByRoomId(Guid? roomId)
        {
            return await context
            .GameRooms
            .Include(gr => gr.Category)
            .Include(gr => gr.Host)
            .Include(gr => gr.PlayerInRooms)
                .ThenInclude(pir => pir.User)
            .Include(gr => gr.SelectedLesson)
            .FirstOrDefaultAsync(gr => gr.Id == roomId);
        }

        public async Task<GameRoom?> GetGameRoomByUserId(Guid UserId)
        {
            return await context
            .GameRooms.FirstOrDefaultAsync(gr => gr.HostId == UserId);
        }

        public async Task<GameRoom?> GetRoomWithPlayersAsync(Guid roomId)
        {
            return await context.GameRooms
                .Include(gr => gr.Category)
                .Include(gr => gr.Host)
                .Include(gr => gr.PlayerInRooms)
                    .ThenInclude(pir => pir.User)
                .Include(gr => gr.SelectedLesson)
                .FirstOrDefaultAsync(gr => gr.Id == roomId);
        }
    }
}