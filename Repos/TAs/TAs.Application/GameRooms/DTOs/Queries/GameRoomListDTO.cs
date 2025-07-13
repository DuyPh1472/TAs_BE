namespace TAs.Application.GameRooms.DTOs.Queries
{
    public class GameRoomListDTO
    {
        public Guid Id { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public string HostName { get; set; } = string.Empty;
        public string HostAvatar { get; set; } = string.Empty;
        public int PlayerCount { get; set; }
        public int MaxPlayers { get; set; }
        public string Status { get; set; } = string.Empty;
        public string CategoryTitle { get; set; } = string.Empty;
        public string CategoryDescription { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public GameRoomSettingsDTO? Settings { get; set; }
        public List<PlayerInRoomDTO> Players { get; set; } = [];
    }

    public class GameRoomSettingsDTO
    {
        public int TimeLimit { get; set; } = 60;
        public int MaxRetries { get; set; } = 2;
        public bool ShowRealTimeScore { get; set; } = true;
        public bool AllowHints { get; set; } = true;
        public string LessonSelection { get; set; } = "host_choice"; // "host_choice" or "random"
    }
} 