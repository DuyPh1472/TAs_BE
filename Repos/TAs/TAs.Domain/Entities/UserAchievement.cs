using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TAs.Domain.Entities
{
    public class UserAchievement : BaseEntity
    {
        [Key]
        public Guid UserAchievementId { get; set; }
        public Guid UserId { get; set; }
        public Guid AchievementId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;
        [ForeignKey(nameof(AchievementId))]
        public Achievement Achievement { get; set; } = null!;
    }
}