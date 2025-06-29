using System.ComponentModel.DataAnnotations;

namespace TAs.Domain.Entities
{
    public class Achievement : BaseEntity
    {
        [Key]
        public Guid AchievementId { get; set; }
        public string AchievementName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<UserAchievement> UserAchievements { get; set; } = [];
    }
}