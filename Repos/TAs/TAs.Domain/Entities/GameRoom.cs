using System.ComponentModel.DataAnnotations.Schema;
using TAs.Domain.Enums;

namespace TAs.Domain.Entities
{
    public class GameRoom : BaseEntity
    {
        public Guid HostId { get; set; }
        public required string RoomName { get; set; }
        public bool IsActive { get; set; } = true;
        public GameStatus Status { get; set; } = GameStatus.Waiting;
        public int MaxPlayers { get; set; } = 4;
        public Guid? SelectedLessonId { get; set; }
        public int CurrentSentence { get; set; } = 0;
        public Guid CategoryId { get; set; }
        public string? Settings { get; set; } // JSON string for game settings
        
        // Individual settings properties for easier access
        public int TimeLimit { get; set; } = 60;
        public int MaxRetries { get; set; } = 2;
        public bool ShowRealTimeScore { get; set; } = true;
        public bool AllowHints { get; set; } = true;
        public string LessonSelection { get; set; } = "host_choice"; // "host_choice" or "random"
        
        [ForeignKey(nameof(HostId))]
        public User Host { get; set; } = null!;
        
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;
        
        [ForeignKey(nameof(SelectedLessonId))]
        public Lesson? SelectedLesson { get; set; }
        
        public ICollection<PlayerInRoom> PlayerInRooms { get; set; } = [];
        public ICollection<PlayerScore> PlayerScores { get; set; } = [];
        public ICollection<GameSession> GameSessions { get; set; } = [];
        public ICollection<ChatMessage> ChatMessages { get; set; } = [];
    }
}