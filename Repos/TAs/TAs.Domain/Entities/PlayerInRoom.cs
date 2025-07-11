using System.ComponentModel.DataAnnotations.Schema;
using TAs.Domain.Enums;

namespace TAs.Domain.Entities
{
    public class PlayerInRoom : BaseEntity
    {
        public Guid RoomId { get; set; } 
        public Guid UserId { get; set; } 
        public bool IsHost { get; set; }
        public bool IsReady { get; set; } = false;
        public int Score { get; set; } = 0;
        public int CurrentProgress { get; set; } = 0;
        public PlayerStatus Status { get; set; } = PlayerStatus.Connected;
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
        
        // Quan hệ
        [ForeignKey(nameof(RoomId))]
        public GameRoom Room { get; set; } = null!;
        
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
    }
}