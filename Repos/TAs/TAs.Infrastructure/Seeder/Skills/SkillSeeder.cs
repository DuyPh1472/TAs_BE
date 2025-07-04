using Microsoft.EntityFrameworkCore;
using TAs.Domain.Entities;
using TAs.Infrastructure.Persistence;
namespace TAs.Infrastructure.Seeder.Skills
{
    public class SkillSeeder(TAsDbContext dbContext) : ISeeder
    {
        public async Task Seed()
        {
            if (!await dbContext.Skills.AnyAsync())
            {
                var adminId = Guid.Parse("ba02df20-a2ca-4f10-be79-8f5fc5bca1da");
                var skills = new List<Skill>
                {
                    new Skill { Name = "Reading", Path = "reading", Icon = "BookOpen", Color = "from-blue-500 to-indigo-600", Description = "Improve your reading comprehension with diverse texts.", CreatedBy = adminId },
                    new Skill { Name = "Writing", Path = "writing", Icon = "Pencil", Color = "from-green-500 to-emerald-600", Description = "Practice writing essays and improve your grammar.", CreatedBy = adminId },
                    new Skill { Name = "Listening", Path = "listening", Icon = "Headphones", Color = "from-purple-500 to-violet-600", Description = "Enhance your listening skills with various audio materials.", CreatedBy = adminId },
                    new Skill { Name = "Vocabulary", Path = "vocabulary", Icon = "Book", Color = "from-orange-500 to-red-600", Description = "Build your vocabulary with flashcards and practice exercises.", CreatedBy = adminId },
                    new Skill { Name = "Speaking", Path = "speaking", Icon = "Volume2", Color = "from-pink-500 to-rose-600", Description = "Improve your pronunciation and speaking fluency.", CreatedBy = adminId },
                    new Skill { Name = "Grammar", Path = "grammar", Icon = "GraduationCap", Color = "from-teal-500 to-cyan-600", Description = "Master English grammar rules and structures.", CreatedBy = adminId },
                    new Skill { Name = "Dictation", Path = "dictation", Icon = "Mic", Color = "from-indigo-500 to-blue-600", Description = "Practice dictation skills with real audio.", CreatedBy = adminId }
                };
                await dbContext.Skills.AddRangeAsync(skills);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}