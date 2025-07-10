using System.ComponentModel.DataAnnotations.Schema;

namespace TAs.Domain.Entities
{
    public class PlayerScore : BaseEntity
    {
        public Guid RoomId { get; set; }   
        public Guid UserId { get; set; }   
        public int Score { get; set; }
        [ForeignKey(nameof(RoomId))]
        public GameRoom Room { get; set; } = null!;
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
    }
}