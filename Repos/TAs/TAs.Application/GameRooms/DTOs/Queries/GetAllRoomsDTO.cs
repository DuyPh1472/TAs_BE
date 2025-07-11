namespace TAs.Application.GameRooms.DTOs.Queries
{
    public class GetAllRoomsDTO
    {
        public string Id { get; set; } = string.Empty;
        public string RoomName { get; set; } = string.Empty;
        public string HostId { get; set; } = string.Empty;
        public string HostName { get; set; } = string.Empty;
        public string HostAvatar { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int MaxPlayers { get; set; }
        public int CurrentPlayers { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CategoryId { get; set; } = string.Empty;
        public string CategoryTitle { get; set; } = string.Empty;
        public string CategoryDifficult { get; set; } = string.Empty;
    }
} 