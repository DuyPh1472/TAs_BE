using TAs.Domain.Entities;
using TAs.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using TAs.Infrastructure.Seeder.Lessons.Services;
using TAs.Infrastructure.Seeder.Lessons.Request;

namespace TAs.Infrastructure.Seeder.Lessons
{
    public class LessonSeedService : ILessonSeedService
    {
        private readonly TAsDbContext _dbContext;
        public LessonSeedService(TAsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(bool Success, string Message)> SeedLessonFromJsonAsync(LessonSeedRequest request)
        {
            try
            {
                var adminId = Guid.Parse("ba02df20-a2ca-4f10-be79-8f5fc5bca1da");
                var now = DateTime.UtcNow;

                // Parse lessonId from request
                var lessonId = request.LessonId;

                // Extract lesson properties
                var title = request.LessonName ?? string.Empty;
                var description = request.Description ?? string.Empty;
                var level = request.VocabLevel ?? string.Empty;
                var accent = request.Accent ?? string.Empty;
                var topics = request.Topics ?? string.Empty;
                var audioUrl = request.AudioSrc;
                var youtubeUrl = request.YoutubeUrl;
                var videoId = request.VideoId;

                // Calculate duration from challenges, defaulting to 0
                var duration = request.Challenges?.Max(ch => ch.TimeEnd) ?? 0;

                // Parse or assign a default categoryId
                var categoryId = Guid.TryParse(request.CategoryId, out var parsedCategoryId) ? parsedCategoryId : Guid.Parse("1b2c3d4e-5f6a-7b8c-9d0e-1f2a3b4c5d6e");

                // Fetch existing lesson, or create new one if not found
                var lesson = await _dbContext.Lessons
                    .Include(l => l.DictationSentences)  // Ensure we can clear old sentences
                    .FirstOrDefaultAsync(l => l.Id == lessonId);

                if (lesson == null)
                {
                    lesson = new Lesson
                    {
                        Id = lessonId,
                        Title = title,
                        Description = description,
                        Level = level,
                        Accent = accent,
                        Duration = (float)duration,
                        Topics = topics,
                        AudioUrl = audioUrl,
                        YoutubeUrl = youtubeUrl,
                        VideoId = videoId,
                        CreatedAt = now,
                        UpdatedAt = now,
                        CreatedBy = adminId,
                        UpdatedBy = adminId
                    };
                    await _dbContext.Lessons.AddAsync(lesson);
                }
                else
                {
                    // Update the existing lesson
                    lesson.Title = title;
                    lesson.Description = description;
                    lesson.Level = level;
                    lesson.Accent = accent;
                    lesson.Duration = (float)duration;
                    lesson.Topics = topics;
                    lesson.AudioUrl = audioUrl;
                    lesson.YoutubeUrl = youtubeUrl;
                    lesson.VideoId = videoId;
                    lesson.UpdatedAt = now;
                    lesson.UpdatedBy = adminId;
                }

                // Save changes for the lesson (combine into one save to improve performance)
                await _dbContext.SaveChangesAsync();

                // Remove existing dictation sentences and add new ones from the challenges
                if (lesson.DictationSentences != null && lesson.DictationSentences.Any())
                {
                    _dbContext.DictationSentences.RemoveRange(lesson.DictationSentences);
                    await _dbContext.SaveChangesAsync();  // Only save once for the sentence removal
                }

                // Add new sentences from the request
                if (request.Challenges != null && request.Challenges.Any())
                {
                    var sentences = request.Challenges
                        .Select((ch, pos) => new DictationSentence
                        {
                            LessonId = lessonId,
                            Text = ch.Content,
                            StartTime = ch.TimeStart,
                            EndTime = ch.TimeEnd,
                            AudioUrl = ch.AudioSrc,
                            Position = pos + 1,
                            CreatedBy = adminId,

                        }).ToList();

                    await _dbContext.DictationSentences.AddRangeAsync(sentences);
                    await _dbContext.SaveChangesAsync();  // Save after adding sentences
                }

                // Handle CategoryLesson relationship
                var catLesson = await _dbContext.CategoryLessons
                    .FirstOrDefaultAsync(cl => cl.LessonId == lessonId && cl.CategoryId == categoryId);

                if (catLesson == null)
                {
                    catLesson = new CategoryLesson
                    {
                        LessonId = lessonId,
                        CategoryId = categoryId,
                        CreatedBy = adminId
                    };
                    await _dbContext.CategoryLessons.AddAsync(catLesson);
                    await _dbContext.SaveChangesAsync(); // Save after adding the category relation
                }

                return (true, "Lesson and sentences seeded/updated successfully.");
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.Error.WriteLine($"Error seeding lesson: {ex.Message}");
                Console.Error.WriteLine($"Stack Trace: {ex.StackTrace}");
                return (false, $"Error: {ex.Message}");
            }
        }
    }
}
