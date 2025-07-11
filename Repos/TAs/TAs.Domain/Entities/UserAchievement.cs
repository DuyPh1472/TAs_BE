using System.ComponentModel.DataAnnotations.Schema;

namespace TAs.Domain.Entities
{
    public class UserAchievement : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid AchievementId { get; set; }
        
        // Quan hệ
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
        
        [ForeignKey(nameof(AchievementId))]
        public Achievement Achievement { get; set; } = null!;
    }
}