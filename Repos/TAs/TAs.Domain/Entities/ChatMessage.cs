using System.ComponentModel.DataAnnotations.Schema;

namespace TAs.Domain.Entities
{
    public class ChatMessage : BaseEntity
    {
        public Guid RoomId { get; set; }
        public Guid UserId { get; set; }
        public string Message { get; set; } = string.Empty;
        public string MessageType { get; set; } = "chat"; // "chat", "system", "game"
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        
        // Quan hệ
        [ForeignKey(nameof(RoomId))]
        public GameRoom Room { get; set; } = null!;
        
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
    }
} 