using Microsoft.EntityFrameworkCore;
using TAs.Domain.Entities;
using TAs.Infrastructure.Persistence;
using TAs.Infrastructure.Seeder.Skills;

namespace TAs.Infrastructure.Seeder.Categories
{
    public class CategorySeeder(TAsDbContext dbContext) : ISeeder
    {
        public async Task Seed()
        {
            if (!await dbContext.Categories.AnyAsync())
            {
                var categories = GetCategories();
                await dbContext.Categories.AddRangeAsync(categories);
                await dbContext.SaveChangesAsync();
            }
        }
        private IEnumerable<Category> GetCategories()
        {
            var today = DateTime.UtcNow;
            var adminId = Guid.Parse("ba02df20-a2ca-4f10-be79-8f5fc5bca1da");
            return new List<Category>
        {
        new Category
        {
            CategoryId = Guid.Parse("e3e6dffc-7754-457f-b23a-85d76c9faa91"),
            Title = "Basic English Conversation",
            Description = "Learn common English phrases used in daily conversations.",
            Difficult = "Beginner",
            Accent = "American",
            Duration = 30.0f,
            CreatedAt = today,
            CreatedBy = adminId

        },
        new Category
        {
            CategoryId = Guid.Parse("71a9568d-f5d9-4fc8-bac9-283e34b0aa7b"),
            Title = "Business English",
            Description = "Improve your English for meetings, emails, and presentations.",
            Difficult = "Intermediate",
            Accent = "British",
            Duration = 45.0f,
            CreatedAt = today,
            CreatedBy = adminId

        },
        new Category
        {
            CategoryId = Guid.Parse("6fe3caca-8225-49a3-93d6-79e864789986"),
            Title = "IELTS Listening Practice",
            Description = "Practice listening skills for the IELTS exam.",
            Difficult = "Advanced",
            Accent = "Mixed",
            Duration = 60.0f,
            CreatedAt = today,
            CreatedBy = adminId

        }
    };
        }

    }
}