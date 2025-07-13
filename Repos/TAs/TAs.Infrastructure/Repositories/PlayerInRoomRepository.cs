using Microsoft.EntityFrameworkCore;
using TAs.Application.Interfaces.Repositories;
using TAs.Domain.Entities;
using TAs.Infrastructure.Persistence;
using TAs.Infrastructure.Persistence.GenericRepo;

namespace TAs.Infrastructure.Repositories
{
    public class PlayerInRoomRepository(TAsDbContext dbContext)
    : GenericRepository<PlayerInRoom>(dbContext), IPlayerInRoomRepository
    {
        public async Task<bool> AllPlayerReady(Guid roomId)
        {
            return await _context.PlayerInRooms.Where(pl => pl.RoomId == roomId)
            .AllAsync(pl => pl.IsReady);
        }

        public async Task<bool> CheckPlayerIsHost(Guid userId)
        {
            return await _context.PlayerInRooms
                .AnyAsync(pl => pl.UserId == userId && pl.IsHost == true);
        }

        public async Task<PlayerInRoom?> GetPlayerInRoomByUserAndRoom(Guid? userId, Guid? RoomId)
        {
            return await _context.PlayerInRooms
            .FirstOrDefaultAsync(pl => pl.RoomId == RoomId
            && pl.UserId == userId);
        }

        public async Task<PlayerInRoom?> GetPlayerInRoomByUser(Guid userId)
        {
            return await _context.PlayerInRooms
                .FirstOrDefaultAsync(pl => pl.UserId == userId);
        }

        public async Task<List<PlayerInRoom>> GetPlayerInRoomsByRoomId(Guid roomId)
        {
            return await _context.PlayerInRooms
            .Where(pl => pl.RoomId == roomId)
            .ToListAsync();
        }

        public async Task<List<PlayerInRoom>> GetPlayersByRoomIdAsync(Guid roomId)
        {
            return await _context.PlayerInRooms
                .Include(pir => pir.User)
                .Where(pl => pl.RoomId == roomId)
                .OrderBy(pl => pl.JoinedAt)
                .ToListAsync();
        }
    }
}