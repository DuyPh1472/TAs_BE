using TAs.Domain.Enums;

namespace TAs.Application.GameRooms
{
    public class GameRoomState
    {
        public Guid RoomId { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public Guid HostId { get; set; }
        public List<PlayerState> Players { get; set; } = new();
        public RoomSettings Settings { get; set; } = new();
        public GameStatus GameStatus { get; set; } = GameStatus.Waiting;
        public Guid CategoryId { get; set; }
        public string CategoryTitle { get; set; } = string.Empty;
        public string CategoryDescription { get; set; } = string.Empty;
        public string CategoryDifficult { get; set; } = string.Empty;
        public Guid? LessonId { get; set; }
        public DateTime? GameStartedAt { get; set; } // Thêm field để track thời điểm bắt đầu game
    }

    public class RoomSettings
    {
        public Guid? LessonId { get; set; }
        public int MaxPlayers { get; set; } = 4;
        public bool IsPrivate { get; set; } = false;
        public int TimeLimit { get; set; } = 60;
        public int MaxRetries { get; set; } = 2;
        public bool ShowRealTimeScore { get; set; } = true;
        public bool AllowHints { get; set; } = true;
        public string LessonSelection { get; set; } = "host_choice";
    }

    // DTO specifically for UpdateSettings to match frontend data
    public class UpdateSettingsDTO
    {
        public int TimeLimit { get; set; }
        public int MaxRetries { get; set; }
        public bool ShowRealTimeScore { get; set; }
        public bool AllowHints { get; set; }
        public string LessonSelection { get; set; } = "host_choice";
    }
}