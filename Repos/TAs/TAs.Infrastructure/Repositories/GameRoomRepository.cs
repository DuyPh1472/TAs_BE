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

        public async Task<GameRoom?> GetGameRoomByRoomId(Guid? roomId)
        {
            return await context
            .GameRooms
            .AsNoTracking()
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
    }
}