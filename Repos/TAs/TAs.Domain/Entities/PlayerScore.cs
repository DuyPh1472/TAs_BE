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
        public float AverageTimePerSentence { get; set; }
        public DateTime CompletedAt { get; set; }
        
        // Quan hệ
        [ForeignKey(nameof(RoomId))]
        public GameRoom Room { get; set; } = null!;
        
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
    }
}