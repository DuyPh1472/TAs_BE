using System.ComponentModel.DataAnnotations.Schema;
using TAs.Domain.Enums;

namespace TAs.Domain.Entities
{
    public class GameSession : BaseEntity
    {
        public Guid RoomId { get; set; }
        public Guid LessonId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public int TotalSentences { get; set; }
        public int CurrentSentence { get; set; } = 0;
        public GameStatus Status { get; set; } = GameStatus.Waiting;
        public int TimeLimit { get; set; } = 60; // seconds per sentence
        public int MaxRetries { get; set; } = 2;
        public bool ShowRealTimeScore { get; set; } = true;
        public bool AllowHints { get; set; } = true;
        public string? Settings { get; set; } // JSON string for additional settings
        
        // Quan hệ
        [ForeignKey(nameof(RoomId))]
        public GameRoom Room { get; set; } = null!;
        
        [ForeignKey(nameof(LessonId))]
        public Lesson Lesson { get; set; } = null!;
        

    }
} 