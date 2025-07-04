namespace TAs.Domain.Entities
{
    public class Achievement : BaseEntity
    {
        public string AchievementName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<UserAchievement> UserAchievements { get; set; } = [];
    }
}