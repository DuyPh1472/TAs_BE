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
                var adminId = Guid.Parse("ba02df20-a2ca-4f10-be79-8f5fc5bca1da");
                // Lấy CategoryId theo tên (đảm bảo đã seed Category trước)
                var categories = dbContext.Categories.ToList();
                Guid GetCategoryId(string title) => categories.First(c => c.Title == title).Id;
                var lessons = new List<Lesson>
                {
                    new Lesson { Title = "Canada's Multiculturalism", Description = "A reading about Canadian culture.", Level = "B2", Accent = "North American", Duration = 7.5f, Topics = "Culture, Canada", CreatedBy = adminId, CategoryId = GetCategoryId("Short Stories") },
                    new Lesson { Title = "IELTS Writing Task 2 Sample", Description = "Sample IELTS Writing Task 2 essay.", Level = "C1", Accent = "British", Duration = 8.0f, Topics = "IELTS, Writing", CreatedBy = adminId, CategoryId = GetCategoryId("IELTS Writing") },
                    new Lesson { Title = "IELTS Listening Section 1", Description = "IELTS Listening practice section 1.", Level = "B2", Accent = "American", Duration = 9.0f, Topics = "IELTS, Listening", CreatedBy = adminId, CategoryId = GetCategoryId("IELTS Listening") },
                    new Lesson { Title = "Academic Word List 1", Description = "First set of academic vocabulary.", Level = "B1", Accent = "International", Duration = 6.0f, Topics = "Vocabulary, Academic", CreatedBy = adminId, CategoryId = GetCategoryId("Academic Vocabulary") },
                    new Lesson { Title = "Ordering Food", Description = "A daily speaking practice lesson.", Level = "A2", Accent = "American", Duration = 5.0f, Topics = "Speaking, Food", CreatedBy = adminId, CategoryId = GetCategoryId("Daily Speaking") },
                    new Lesson { Title = "Present Simple Tense", Description = "Grammar basics: present simple.", Level = "A1", Accent = "International", Duration = 4.0f, Topics = "Grammar, Present Simple", CreatedBy = adminId, CategoryId = GetCategoryId("Grammar Basics") },
                    new Lesson { Title = "Dictation: Daily Routine", Description = "Dictation practice about daily routines.", Level = "B1", Accent = "American", Duration = 10.0f, Topics = "Dictation, Routine", CreatedBy = adminId, CategoryId = GetCategoryId("Dictation Practice") }
                };
                await dbContext.Lessons.AddRangeAsync(lessons);
                await dbContext.SaveChangesAsync();

                // Seed DictationSentence cho lesson thuộc category 'Dictation Practice'
                var dictationLesson = dbContext.Lessons.FirstOrDefault(l => l.Title == "Dictation: Daily Routine");
                if (dictationLesson != null)
                {
                    var dictationSentences = new List<DictationSentence>
                    {
                        new DictationSentence { LessonId = dictationLesson.Id, Text = "I wake up at 6 a.m. every day.", StartTime = 0f, EndTime = 2.5f, Position = 1, CreatedBy = adminId },
                        new DictationSentence { LessonId = dictationLesson.Id, Text = "Then I brush my teeth and have breakfast.", StartTime = 2.5f, EndTime = 5.0f, Position = 2, CreatedBy = adminId }
                    };
                    await dbContext.DictationSentences.AddRangeAsync(dictationSentences);
                    await dbContext.SaveChangesAsync();
                }
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
                    CreatedBy = adminId,
                }
            };
        }
    }
}