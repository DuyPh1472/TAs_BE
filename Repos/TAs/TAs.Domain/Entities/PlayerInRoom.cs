using System.ComponentModel.DataAnnotations.Schema;

namespace TAs.Domain.Entities
{
    public class PlayerInRoom : BaseEntity
    {
        public Guid RoomId { get; set; } 
        public Guid UserId { get; set; } 
        public bool IsHost { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
        [ForeignKey(nameof(RoomId))]
        public GameRoom Room { get; set; } = null!;
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
    }
}