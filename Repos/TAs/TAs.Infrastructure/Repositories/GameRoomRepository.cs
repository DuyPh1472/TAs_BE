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

    }
}