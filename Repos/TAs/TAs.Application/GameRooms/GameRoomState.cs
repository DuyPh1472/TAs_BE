namespace TAs.Application.GameRooms
{
    public class GameRoomState
    {
        public Guid RoomId { get; set; }
        public string RoomName { get; set; } = string.Empty;
        public Guid HostId { get; set; }
        public List<PlayerState> Players { get; set; } = new();
        public RoomSettings Settings { get; set; } = new();
        public string GameStatus { get; set; } = "Waiting";
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
} 