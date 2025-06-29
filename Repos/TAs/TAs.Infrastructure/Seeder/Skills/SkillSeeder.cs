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
                    CreatedBy = adminId
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
                    UpdatedAt = DateTime.UtcNow,
                    CreatedBy = adminId
                },
                // Add more skills as needed
            };
            return skills;
        }
    }

}