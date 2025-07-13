namespace TAs.Application.GameRooms.DTOs.Queries
{
    public class GetRoomDetailsDTO
    {
        public string Id { get; set; } = string.Empty;
        public string RoomName { get; set; } = string.Empty;
        public string HostId { get; set; } = string.Empty;
        public string HostName { get; set; } = string.Empty;
        public string HostAvatar { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int MaxPlayers { get; set; }
        public int CurrentPlayers { get; set; }
        public string? SelectedLessonId { get; set; }
        public string? SelectedLessonTitle { get; set; }
        public int CurrentSentence { get; set; }
        public string CategoryId { get; set; } = string.Empty;
        public string CategoryTitle { get; set; } = string.Empty;
        public string CategoryDescription { get; set; } = string.Empty;
        public string CategoryDifficult { get; set; } = string.Empty;
        public DateTimeOffset CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public List<PlayerInRoomDTO> Players { get; set; } = [];
        public GameRoomSettingsDTO? Settings { get; set; }
    }

    public class PlayerInRoomDTO
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public bool IsHost { get; set; }
        public bool IsReady { get; set; }
        public int Score { get; set; }
        public int CurrentProgress { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime JoinedAt { get; set; }
    }

    
} 