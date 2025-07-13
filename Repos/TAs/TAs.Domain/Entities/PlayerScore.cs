using System.ComponentModel.DataAnnotations.Schema;

namespace TAs.Domain.Entities
{
    public class PlayerScore : BaseEntity
    {
        public Guid RoomId { get; set; }   
        public Guid UserId { get; set; }   
        public int Score { get; set; }
        public int TotalSentences { get; set; }
        public int CorrectAnswers { get; set; }
        public int IncorrectAnswers { get; set; }
        public float AverageTimePerSentence { get; set; }
        public int TotalTimeSpent { get; set; } // seconds
        public int TotalRetries { get; set; }
        public DateTime CompletedAt { get; set; }
        public string? GameSessionId { get; set; } // Reference to game session
        
        // Quan hệ
        [ForeignKey(nameof(RoomId))]
        public GameRoom Room { get; set; } = null!;
        
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
    }
}