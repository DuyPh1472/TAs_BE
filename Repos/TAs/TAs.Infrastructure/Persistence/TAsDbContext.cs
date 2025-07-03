using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TAs.Domain.Entities;

namespace TAs.Infrastructure.Persistence
{
    public class TAsDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public TAsDbContext(DbContextOptions<TAsDbContext> options)
            : base(options)
        {
        }

        internal DbSet<Skill> Skills { get; set; }
        internal DbSet<Achievement> Achievements { get; set; }
        internal DbSet<Category> Categories { get; set; }
        internal DbSet<CategoryLesson> CategoryLessons { get; set; }
        internal DbSet<Lesson> Lessons { get; set; }
        internal DbSet<Progress> Progresses { get; set; }
        internal DbSet<SkillLesson> SkillLessons { get; set; }
        internal DbSet<UserAchievement> UserAchievements { get; set; }
        internal DbSet<ProgressDetail> ProgressDetails { get; set; }
        internal DbSet<DictationSentence> DictationSentences { get; set; }
    }
}