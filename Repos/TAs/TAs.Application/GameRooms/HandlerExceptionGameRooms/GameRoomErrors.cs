using TAs.Domain.Errors;

namespace TAs.Application.GameRooms.HandlerExceptionGameRooms
{
    public static class GameRoomErrors
    {
        public static Error RoomNotFound(Guid roomId) => 
            new("RoomNotFound", $"Room with ID {roomId} not found.");

        public static Error RoomFull(Guid roomId) => 
            new("RoomFull", $"Room {roomId} is full.");

        public static Error UserAlreadyInRoom(Guid userId, Guid roomId) => 
            new("UserAlreadyInRoom", $"User {userId} is already in room {roomId}.");

        public static Error UserNotInRoom(Guid userId, Guid roomId) => 
            new("UserNotInRoom", $"User {userId} is not in room {roomId}.");

        public static Error NotRoomHost() => 
            new("NotRoomHost", "Only room host can perform this action.");

        public static Error GameInProgress(Guid roomId) => 
            new("GameInProgress", $"Game is already in progress in room {roomId}.");

        public static Error LessonNotFound(Guid lessonId) => 
            new("LessonNotFound", $"Lesson with ID {lessonId} not found.");

        public static Error GameSessionAlreadyExists() => 
            new("GameSessionAlreadyExists", "Game session already exists for this room.");

        public static Error GameSessionNotFound(Guid sessionId) => 
            new("GameSessionNotFound", $"Game session with ID {sessionId} not found.");

        public static Error CannotKickHost() => 
            new("CannotKickHost", "Cannot kick the host from the room.");

        public static Error PlayerNotFound(Guid playerId) => 
            new("PlayerNotFound", $"Player with ID {playerId} not found.");

        public static Error NotAllPlayersReady() => 
            new("NotAllPlayersReady", "Not all players are ready.");

        public static Error LessonNotSelected() => 
            new("LessonNotSelected", "No lesson selected for this room.");
    }
}