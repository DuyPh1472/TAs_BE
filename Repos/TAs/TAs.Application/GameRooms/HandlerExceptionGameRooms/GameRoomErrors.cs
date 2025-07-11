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
    }
}