using Microsoft.EntityFrameworkCore;
using TAs.Domain.Entities;
using TAs.Infrastructure.Persistence;
using TAs.Infrastructure.Seeder.Skills;

namespace TAs.Infrastructure.Seeder.Lessons
{
    public class LessonSeeder(TAsDbContext dbContext) : ISeeder
    {
        public async Task Seed()
        {
            if (!await dbContext.Lessons.AnyAsync())
            {
                var lessons = GetLessonsAsync();
                await dbContext.Lessons.AddRangeAsync(lessons);
                await dbContext.SaveChangesAsync();
            }
        }
        private IEnumerable<Lesson> GetLessonsAsync()
        {
            var today = DateTime.UtcNow;
            var adminId = Guid.Parse("ba02df20-a2ca-4f10-be79-8f5fc5bca1da");
            return new List<Lesson>
        {
            new Lesson
            {
                LessonId = Guid.Parse("cc329906-85bc-4d3d-8a1c-a6b1d68715b5"),
                Title = "Greetings & Introductions",
                Description = "Learn how to greet and introduce yourself in everyday English.",
                Level = "Beginner",
                Sentences = "Hello, my name is John. How are you? Nice to meet you.",
                Accent = "American",
                Duration = 10.0f,
                Topics = "Greetings, Introduction",
                CreatedAt = today,
                CreatedBy = adminId
            },
            new Lesson
            {
                LessonId = Guid.Parse("ed9ab41e-7d83-49a3-bc38-c0041d86a382"),
                Title = "Effective Email Writing",
                Description = "Tips and phrases for writing professional emails.",
                Level = "Intermediate",
                Sentences = "Dear Sir/Madam, I hope this email finds you well...",
                Accent = "British",
                Duration = 15.0f,
                Topics = "Business, Email",
                CreatedAt = today,
                CreatedBy = adminId
            },
            new Lesson
            {
                LessonId = Guid.Parse("6cd1a152-8a9b-4893-a7b9-fcb556397b63"),
                Title = "Listening Practice - Maps & Directions",
                Description = "Practice listening to conversations involving directions and locations.",
                Level = "Advanced",
                Sentences = "Turn left at the traffic light, then walk straight for two blocks...",
                Accent = "Mixed",
                Duration = 20.0f,
                Topics = "IELTS, Listening, Directions",
                CreatedAt = today,
                CreatedBy = adminId
            }
        };
        }
    }
}