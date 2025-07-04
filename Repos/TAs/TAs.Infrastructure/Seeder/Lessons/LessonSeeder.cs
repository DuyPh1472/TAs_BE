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
                var dictationCategoryId = GetCategoryId("Dictation Practice");
                var lessons = new List<Lesson>
                {
                    new Lesson { Title = "Canada's Multiculturalism", Description = "A reading about Canadian culture.", Level = "B2", Accent = "North American", Duration = 7.5f, Topics = "Culture, Canada", CreatedBy = adminId, CategoryId = GetCategoryId("Short Stories") },
                    new Lesson { Title = "IELTS Writing Task 2 Sample", Description = "Sample IELTS Writing Task 2 essay.", Level = "C1", Accent = "British", Duration = 8.0f, Topics = "IELTS, Writing", CreatedBy = adminId, CategoryId = GetCategoryId("IELTS Writing") },
                    new Lesson { Title = "IELTS Listening Section 1", Description = "IELTS Listening practice section 1.", Level = "B2", Accent = "American", Duration = 9.0f, Topics = "IELTS, Listening", CreatedBy = adminId, CategoryId = GetCategoryId("IELTS Listening") },
                    new Lesson { Title = "Academic Word List 1", Description = "First set of academic vocabulary.", Level = "B1", Accent = "International", Duration = 6.0f, Topics = "Vocabulary, Academic", CreatedBy = adminId, CategoryId = GetCategoryId("Academic Vocabulary") },
                    new Lesson { Title = "Ordering Food", Description = "A daily speaking practice lesson.", Level = "A2", Accent = "American", Duration = 5.0f, Topics = "Speaking, Food", CreatedBy = adminId, CategoryId = GetCategoryId("Daily Speaking") },
                    new Lesson { Title = "Present Simple Tense", Description = "Grammar basics: present simple.", Level = "A1", Accent = "International", Duration = 4.0f, Topics = "Grammar, Present Simple", CreatedBy = adminId, CategoryId = GetCategoryId("Grammar Basics") },
                    // Thêm nhiều lesson cho Dictation Practice
                    new Lesson { Title = "Dictation: Daily Routine", Description = "Dictation practice about daily routines.", Level = "B1", Accent = "American", Duration = 10.0f, Topics = "Dictation, Routine", CreatedBy = adminId, CategoryId = dictationCategoryId },
                    new Lesson { Title = "Dictation: At the Office", Description = "Dictation practice for office conversations.", Level = "B1", Accent = "American", Duration = 8.0f, Topics = "Dictation, Office", CreatedBy = adminId, CategoryId = dictationCategoryId },
                    new Lesson { Title = "Dictation: Shopping", Description = "Dictation practice for shopping situations.", Level = "A2", Accent = "American", Duration = 7.0f, Topics = "Dictation, Shopping", CreatedBy = adminId, CategoryId = dictationCategoryId },
                    new Lesson { Title = "Dictation: Travel", Description = "Dictation practice for travel scenarios.", Level = "B2", Accent = "British", Duration = 9.0f, Topics = "Dictation, Travel", CreatedBy = adminId, CategoryId = dictationCategoryId }
                };
                await dbContext.Lessons.AddRangeAsync(lessons);
                await dbContext.SaveChangesAsync();

                // Seed DictationSentence cho từng lesson thuộc category 'Dictation Practice'
                var dictationLessons = dbContext.Lessons.Where(l => l.CategoryId == dictationCategoryId).ToList();
                foreach (var lesson in dictationLessons)
                {
                    List<DictationSentence> dictationSentences = new();
                    if (lesson.Title == "Dictation: Daily Routine")
                    {
                        dictationSentences = new List<DictationSentence>
                        {
                            new DictationSentence { LessonId = lesson.Id, Text = "I wake up at 6 a.m. every day.", StartTime = 0f, EndTime = 2.5f, Position = 1, CreatedBy = adminId },
                            new DictationSentence { LessonId = lesson.Id, Text = "Then I brush my teeth and have breakfast.", StartTime = 2.5f, EndTime = 5.0f, Position = 2, CreatedBy = adminId },
                            new DictationSentence { LessonId = lesson.Id, Text = "I go to work by bus.", StartTime = 5.0f, EndTime = 7.0f, Position = 3, CreatedBy = adminId },
                            new DictationSentence { LessonId = lesson.Id, Text = "After work, I cook dinner and watch TV.", StartTime = 7.0f, EndTime = 9.0f, Position = 4, CreatedBy = adminId }
                        };
                    }
                    else if (lesson.Title == "Dictation: At the Office")
                    {
                        dictationSentences = new List<DictationSentence>
                        {
                            new DictationSentence { LessonId = lesson.Id, Text = "Good morning, everyone. Let's start the meeting.", StartTime = 0f, EndTime = 2.5f, Position = 1, CreatedBy = adminId },
                            new DictationSentence { LessonId = lesson.Id, Text = "Please review the agenda on your desk.", StartTime = 2.5f, EndTime = 5.0f, Position = 2, CreatedBy = adminId },
                            new DictationSentence { LessonId = lesson.Id, Text = "We need to finish the report by Friday.", StartTime = 5.0f, EndTime = 7.0f, Position = 3, CreatedBy = adminId },
                            new DictationSentence { LessonId = lesson.Id, Text = "Let me know if you have any questions.", StartTime = 7.0f, EndTime = 9.0f, Position = 4, CreatedBy = adminId }
                        };
                    }
                    else if (lesson.Title == "Dictation: Shopping")
                    {
                        dictationSentences = new List<DictationSentence>
                        {
                            new DictationSentence { LessonId = lesson.Id, Text = "How much does this shirt cost?", StartTime = 0f, EndTime = 2.0f, Position = 1, CreatedBy = adminId },
                            new DictationSentence { LessonId = lesson.Id, Text = "Do you have this in a larger size?", StartTime = 2.0f, EndTime = 4.0f, Position = 2, CreatedBy = adminId },
                            new DictationSentence { LessonId = lesson.Id, Text = "Can I pay by credit card?", StartTime = 4.0f, EndTime = 6.0f, Position = 3, CreatedBy = adminId },
                            new DictationSentence { LessonId = lesson.Id, Text = "Thank you for your help!", StartTime = 6.0f, EndTime = 7.0f, Position = 4, CreatedBy = adminId }
                        };
                    }
                    else if (lesson.Title == "Dictation: Travel")
                    {
                        dictationSentences = new List<DictationSentence>
                        {
                            new DictationSentence { LessonId = lesson.Id, Text = "Where is the nearest train station?", StartTime = 0f, EndTime = 2.0f, Position = 1, CreatedBy = adminId },
                            new DictationSentence { LessonId = lesson.Id, Text = "I would like to book a single room.", StartTime = 2.0f, EndTime = 4.0f, Position = 2, CreatedBy = adminId },
                            new DictationSentence { LessonId = lesson.Id, Text = "What time does the museum open?", StartTime = 4.0f, EndTime = 6.0f, Position = 3, CreatedBy = adminId },
                            new DictationSentence { LessonId = lesson.Id, Text = "Can you recommend a good restaurant nearby?", StartTime = 6.0f, EndTime = 9.0f, Position = 4, CreatedBy = adminId }
                        };
                    }
                    if (dictationSentences.Count > 0)
                    {
                        await dbContext.DictationSentences.AddRangeAsync(dictationSentences);
                    }
                }
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
                    CreatedBy = adminId,
                }
            };
        }
    }
}