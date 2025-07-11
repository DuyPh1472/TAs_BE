using TAs.Domain.Errors;

namespace TAs.Application.GameRooms.HandlerExceptionGameRooms
{
    public class GameRoomErrors
    {
        public static Error NoRoomFound(Guid roomId)
        => new("NoRoomFound", $"Room with Id: {roomId} does not exist! ");
        public static readonly Error UserAlreadyInRoom
        = new("UserAlreadyInRoom", "User already in room");
        public static readonly Error UserNotInRoom
        = new("UserNotInRoom", "User does not in room");
        public static readonly Error OnlyHostCanStart
            = new("OnlyHostCanStart", "Only the host can start the game.");
        public static readonly Error NotAllPlayersReady
            = new("NotAllPlayersReady", "Not all players are ready.");
        public static readonly Error LessonNotSelected
            = new("LessonNotSelected", "No lesson selected for this room.");
        public static readonly Error LessonNotFound
            = new("LessonNotFound", "Lesson not found.");
    }
}