namespace TAs.Application.GameRooms.DTOs.Queries
{
    public class PlayerDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public bool IsHost { get; set; }
        public bool IsReady { get; set; }
        public int Score { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime JoinedAt { get; set; }
    }
} 