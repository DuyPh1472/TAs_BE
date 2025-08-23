using Microsoft.AspNetCore.SignalR;
using TAs.Application.GameRooms;
using TAs.APi.Multiplayer;

namespace TAs.APi.Services
{
    public class GameRoomEventService : IGameRoomEventService
    {
        private readonly IHubContext<GameRoomHub> _hubContext;

        public GameRoomEventService(IHubContext<GameRoomHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task BroadcastGameFinishedAsync(Guid roomId)
        {
            await _hubContext.Clients.Group(roomId.ToString()).SendAsync("GameFinished");
        }
    }
} 