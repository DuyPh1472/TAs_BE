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
                var today = DateTime.UtcNow;
                var adminId = Guid.Parse("ba02df20-a2ca-4f10-be79-8f5fc5bca1da");
                var lessonId = Guid.Parse("14540000-0000-0000-0000-000000000000"); // fixed for sample
                var categoryId = Guid.Parse("1b2c3d4e-5f6a-7b8c-9d0e-1f2a3b4c5d6e"); // IELTS Listening

                var lesson = new Lesson
                {
                    Id = lessonId,
                    Title = "5 tips to improve your critical thinking",
                    Description = "5 tips to improve your critical thinking - Samantha Agoos",
                    Level = "C1",
                    Accent = "American",
                    Duration = 251.66f, // tổng thời lượng cuối cùng
                    Topics = "Critical Thinking, TED-Ed",
                    AudioUrl = null,
                    YoutubeUrl = "https://www.youtube.com/watch?v=dItUGF8GdTw",
                    VideoId = "dItUGF8GdTw",
                    Sentences = "", // không dùng
                    CreatedAt = today,
                    CreatedBy = adminId,
                    UpdatedAt = today,
                    UpdatedBy = adminId
                };
                await dbContext.Lessons.AddAsync(lesson);
                await dbContext.SaveChangesAsync();

                // Seed DictationSentences
                var dictationSentences = new List<DictationSentence>
                {
                    new DictationSentence {
                        Id = Guid.NewGuid(),
                        LessonId = lessonId,
                        Text = "Every day, a sea of decisions stretches before us.",
                        StartTime = 11.11f,
                        EndTime = 14.93f,
                        AudioUrl = null,
                        Position = 1,
                    CreatedBy = adminId,

                    },
                    new DictationSentence {
                        Id = Guid.NewGuid(),
                        LessonId = lessonId,
                        Text = "Some are small and unimportant,",
                        StartTime = 14.93f,
                        EndTime = 16.8f,
                        AudioUrl = null,
                        Position = 2,
                    CreatedBy = adminId,

                    },
                    new DictationSentence {
                        Id = Guid.NewGuid(),
                        LessonId = lessonId,
                        Text = "but others have a larger impact on our lives.",
                        StartTime = 16.8f,
                        EndTime = 19.25f,
                        AudioUrl = null,
                        Position = 3,
                    CreatedBy = adminId,

                    },
                    new DictationSentence {
                        Id = Guid.NewGuid(),
                        LessonId = lessonId,
                        Text = "For example, which politician should I vote for?",
                        StartTime = 19.25f,
                        EndTime = 22.05f,
                        AudioUrl = null,
                        Position = 4,
                    CreatedBy = adminId,

                    },
                    new DictationSentence {
                        Id = Guid.NewGuid(),
                        LessonId = lessonId,
                        Text = "Should I try the latest diet craze?",
                        StartTime = 22.05f,
                        EndTime = 24.37f,
                        AudioUrl = null,
                        Position = 5,
                    CreatedBy = adminId,

                    },
                    new DictationSentence {
                        Id = Guid.NewGuid(),
                        LessonId = lessonId,
                        Text = "Or will email make me a millionaire?",
                        StartTime = 24.37f,
                        EndTime = 26.94f,
                        AudioUrl = null,
                        Position = 6,
                    CreatedBy = adminId,

                    }
                    // ... (các câu còn lại, bạn có thể bổ sung tương tự từ data.json)
                };
                await dbContext.DictationSentences.AddRangeAsync(dictationSentences);
                await dbContext.SaveChangesAsync();

                // Liên kết với category IELTS Listening
                var categoryLesson = new CategoryLesson {
                    Id = Guid.NewGuid(),
                    LessonId = lessonId,
                    CategoryId = categoryId,
                    CreatedAt = today,
                    CreatedBy = adminId,
                    UpdatedAt = today,
                    UpdatedBy = adminId
                };
                await dbContext.CategoryLessons.AddAsync(categoryLesson);
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
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Title = "5 tips to improve your critical thinking",
                    Description = "Sample lesson for critical thinking.",
                    Level = "C1",
                    Accent = "American",
                    Duration = 60.0f,
                    Topics = "Critical Thinking, TED-Ed",
                    AudioUrl = null,
                    YoutubeUrl = "https://www.youtube.com/watch?v=dItUGF8GdTw",
                    VideoId = "dItUGF8GdTw",
                    Sentences = "",
                    CreatedAt = today,
                    CreatedBy = adminId
                }
            };
        }
    }
}