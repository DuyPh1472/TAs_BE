using Microsoft.AspNetCore.Identity;
namespace TAs.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public DateTime? DateOfBirth { get; set; }
        public string Avatar { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public float TargetScore { get; set; }
        public string Level { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public ICollection<Progress> Progresses { get; set; } = [];
        public ICollection<UserAchievement> UserAchievements { get; set; } = [];
    }
}