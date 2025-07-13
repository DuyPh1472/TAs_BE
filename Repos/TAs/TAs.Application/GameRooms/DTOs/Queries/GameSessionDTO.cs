namespace TAs.Application.GameRooms.DTOs.Queries
{
    public class GameSessionDTO
    {
        public Guid SessionId { get; set; }
        public Guid RoomId { get; set; }
        public Guid SelectedLessonId { get; set; }
        public int TotalSentences { get; set; }
        public int CurrentSentence { get; set; }
        public DateTime StartedAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<PlayerDTO> Players { get; set; } = [];
        public GameSettingsDTO Settings { get; set; } = new();
    }

    public class GameSettingsDTO
    {
        public int TimeLimit { get; set; }
        public int MaxRetries { get; set; }
        public bool ShowRealTimeScore { get; set; }
        public bool AllowHints { get; set; }
    }
} 