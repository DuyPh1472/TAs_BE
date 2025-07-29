using TAs.Application.GameRooms;

namespace TAs.Application.GameRooms
{
    public interface IInMemoryGameRoomService
    {
        IEnumerable<GameRoomState> GetAllRooms();
        GameRoomState? GetRoom(Guid roomId);
        GameRoomState AddRoom(GameRoomState room);
        AddPlayerResult AddPlayer(Guid roomId, PlayerState player);
        void RemovePlayer(Guid roomId, Guid userId);
        bool SetLessonForRoom(Guid roomId, Guid lessonId);
        Guid CreateRoom(string roomName, int maxPlayers, Guid categoryId, string categoryTitle, Guid hostId, string hostName, string categoryDifficult);
        IEnumerable<object> GetActiveRoomsDTO();
        object? GetRoomDetailsDTO(Guid roomId);
        ReadyPlayerResult ReadyInMemory(Guid roomId, Guid userId, bool isReady);
        KickPlayerResult KickPlayerInMemory(Guid roomId, Guid hostId, Guid targetUserId);
    }
} 