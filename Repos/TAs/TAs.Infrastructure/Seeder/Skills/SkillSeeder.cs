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
                var skills = GetSkills();
                await dbContext.Skills.AddRangeAsync(skills);
                await dbContext.SaveChangesAsync();
            }
        }
        private IEnumerable<Skill> GetSkills()
        {
            var adminId = Guid.Parse("ba02df20-a2ca-4f10-be79-8f5fc5bca1da");
            var today = DateTime.UtcNow;
            List<Skill> skills = new()
            {
                new Skill
                {
                    Id = Guid.Parse("a1b2c3d4-e5f6-7890-abcd-ef1234567890"),
                    Name = "Reading",
                    Path = "reading",
                    Icon = "BookOpen",
                    Color = "from-blue-500 to-indigo-600",
                    Description = "Improve your reading comprehension with diverse texts",
                    CreatedAt = today,
                    UpdatedAt = today,
                    CreatedBy = adminId
                },
                new Skill
                {
                    Id = Guid.Parse("b2c3d4e5-f6a7-8901-bcde-f23456789012"),
                    Name = "Writing",
                    Path = "writing",
                    Icon = "Pencil",
                    Color = "from-green-500 to-emerald-600",
                    Description = "Practice writing essays and improve your grammar.",
                    CreatedAt = today,
                    UpdatedAt = today,
                    CreatedBy = adminId
                },
                new Skill
                {
                    Id = Guid.Parse("c3d4e5f6-a7b8-9012-cdef-345678901234"),
                    Name = "Listening",
                    Path = "listening",
                    Icon = "Headphones",
                    Color = "from-purple-500 to-violet-600",
                    Description = "Enhance your listening skills with various audio materials",
                    CreatedAt = today,
                    UpdatedAt = today,
                    CreatedBy = adminId
                },
                new Skill
                {
                    Id = Guid.Parse("d4e5f6a7-b8c9-0123-def4-567890123456"),
                    Name = "Vocabulary",
                    Path = "vocabulary",
                    Icon = "Book",
                    Color = "from-orange-500 to-red-600",
                    Description = "Build your vocabulary with flashcards and practice exercises",
                    CreatedAt = today,
                    UpdatedAt = today,
                    CreatedBy = adminId
                },
                new Skill
                {
                    Id = Guid.Parse("e5f6a7b8-c9d0-1234-ef56-789012345678"),
                    Name = "Speaking",
                    Path = "speaking",
                    Icon = "Volume2",
                    Color = "from-pink-500 to-rose-600",
                    Description = "Improve your pronunciation and speaking fluency",
                    CreatedAt = today,
                    UpdatedAt = today,
                    CreatedBy = adminId
                },
                new Skill
                {
                    Id = Guid.Parse("f6a7b8c9-d0e1-2345-f678-901234567890"),
                    Name = "Grammar",
                    Path = "grammar",
                    Icon = "GraduationCap",
                    Color = "from-teal-500 to-cyan-600",
                    Description = "Master English grammar rules and structures",
                    CreatedAt = today,
                    UpdatedAt = today,
                    CreatedBy = adminId
                }
            };
            return skills;
        }
    }
}