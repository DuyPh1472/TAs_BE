namespace TAs.Application.GameRooms
{
    public interface IGameRoomEventService
    {
        Task BroadcastGameFinishedAsync(Guid roomId);
    }
} 