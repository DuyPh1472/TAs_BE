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
        [ForeignKey(nameof(HostId))]
        public User Host { get; set; } = null!;
        public ICollection<PlayerInRoom> PlayerInRooms = [];
        public ICollection<PlayerScore> playerScores = [];
        
    }
}