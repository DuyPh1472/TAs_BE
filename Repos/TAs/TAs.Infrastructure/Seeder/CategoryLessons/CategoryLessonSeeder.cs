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
                     CategoryLessonId = Guid.NewGuid(),
                     CategoryId = Guid.Parse("e3e6dffc-7754-457f-b23a-85d76c9faa91"),
                     LessonId = Guid.Parse("cc329906-85bc-4d3d-8a1c-a6b1d68715b5"),
                     CreatedAt = today,
                     CreatedBy = adminId
                 },
                 new CategoryLesson{
                     CategoryLessonId = Guid.NewGuid(),
                     CategoryId =Guid.Parse("71a9568d-f5d9-4fc8-bac9-283e34b0aa7b"),
                     LessonId = Guid.Parse("ed9ab41e-7d83-49a3-bc38-c0041d86a382"),
                     CreatedAt = today,
                     CreatedBy = adminId
                 },
                 new CategoryLesson{
                     CategoryLessonId = Guid.NewGuid(),
                     CategoryId = Guid.Parse("6fe3caca-8225-49a3-93d6-79e864789986"),
                     LessonId = Guid.Parse("6cd1a152-8a9b-4893-a7b9-fcb556397b63"),
                     CreatedAt = today,
                     CreatedBy = adminId
                 }
            };

        }


    }
}