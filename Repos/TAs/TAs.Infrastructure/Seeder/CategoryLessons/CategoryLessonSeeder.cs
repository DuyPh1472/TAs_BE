using Microsoft.EntityFrameworkCore;
using TAs.Domain.Entities;
using TAs.Infrastructure.Persistence;
using TAs.Infrastructure.Seeder.Skills;

namespace TAs.Infrastructure.Seeder.CategoryLessons
{
    public class CategoryLessonSeeder(TAsDbContext dbContext) : ISeeder
    {
        public async Task Seed()
        {
            if (!await dbContext.CategoryLessons.AnyAsync())
            {
                var categoryLessons = GetCategoryLessonsAsync();
                await dbContext.CategoryLessons.AddRangeAsync(categoryLessons);
                await dbContext.SaveChangesAsync();
            }

        }
        private IEnumerable<CategoryLesson> GetCategoryLessonsAsync()
        {
            var today = DateTime.UtcNow;
            var adminId = Guid.Parse("ba02df20-a2ca-4f10-be79-8f5fc5bca1da");
            return new List<CategoryLesson>
            {
                new CategoryLesson{
                    Id = Guid.NewGuid(),
                    CategoryId = Guid.Parse("e3e6dffc-7754-457f-b23a-85d76c9faa91"),
                    LessonId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    CreatedAt = today,
                    CreatedBy = adminId
                }
            };
        }


    }
}