using Microsoft.EntityFrameworkCore;
using TAs.Domain.Entities;
using TAs.Infrastructure.Persistence;
namespace TAs.Infrastructure.Seeder.Skills
{
    public class SkillSeeder(TAsDbContext dbContext) : ISkillSeeder
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
            List<Skill> skills = new()
            {
                new Skill
                {
                    Id = Guid.NewGuid(),
                    Name = "C#",
                    Path = "csharp",
                    Icon = "csharp.svg",
                    Color = "#178600",
                    Description = "C# is a modern, object-oriented programming language developed by Microsoft.",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                },
                new Skill
                {
                    Id = Guid.NewGuid(),
                    Name = "JavaScript",
                    Path = "javascript",
                    Icon = "javascript.svg",
                    Color = "#f7df1e",
                    Description = "JavaScript is a versatile programming language primarily used for web development.",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                // Add more skills as needed
            };
            return skills;
        }
    }

}