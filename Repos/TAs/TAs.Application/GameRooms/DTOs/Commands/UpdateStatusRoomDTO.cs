namespace TAs.Application.GameRooms.DTOs.Commands
{
    public class UpdateStatusRoomDTO
    {
        public Guid RoomId { get; set; }
        public Guid UserId { get; set; }
        public bool IsReady { get; set; }
        public bool AllReady { get; set; }
    }
}