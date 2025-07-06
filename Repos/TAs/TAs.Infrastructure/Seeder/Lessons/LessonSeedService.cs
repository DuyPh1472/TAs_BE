using TAs.Domain.Entities;
using TAs.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using TAs.Infrastructure.Seeder.Lessons.Services;
using TAs.Infrastructure.Seeder.Lessons.Request;
using System.Text.Json; 

namespace TAs.Infrastructure.Seeder.Lessons
{
    public class LessonSeedService : ILessonSeedService
    {
        private readonly TAsDbContext _dbContext;
        public LessonSeedService(TAsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(bool Success, string Message)> SeedLessonFromJsonAsync(string categoryTitle, LessonSeedRequest request)
        {
            try
            {
                var adminId = Guid.Parse("ba02df20-a2ca-4f10-be79-8f5fc5bca1da");
                var now = DateTime.UtcNow;

                var lessonId = request.LessonId;

                var title = request.LessonName ?? string.Empty;
                var description = request.Description ?? string.Empty;
                var level = request.VocabLevel ?? string.Empty;
                var accent = request.Accent ?? string.Empty;
                var topics = request.Topics ?? string.Empty;
                var audioUrl = request.AudioSrc;
                var youtubeUrl = request.YoutubeUrl;
                var videoId = request.VideoId;

                var duration = request.Challenges?.Max(ch => ch.TimeEnd) ?? 0;

                var category = await _dbContext
                    .Categories.FirstOrDefaultAsync(c => c.Title == categoryTitle);

                if (category == null)
                {
                    return (false, $"Category with title '{categoryTitle}' not found.");
                }

                var lesson = await _dbContext.Lessons
                    .Include(l => l.DictationSentences)
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
                        UpdatedBy = adminId,
                        CategoryId = category.Id
                    };
                    await _dbContext.Lessons.AddAsync(lesson);
                }
                else
                {
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
                    lesson.CategoryId = category.Id;
                }

                await _dbContext.SaveChangesAsync();

                if (lesson.DictationSentences != null && lesson.DictationSentences.Any())
                {
                    _dbContext.DictationSentences.RemoveRange(lesson.DictationSentences);
                    await _dbContext.SaveChangesAsync();
                }

                if (request.Challenges != null && request.Challenges.Any())
                {
                    var sentences = request.Challenges
                        .Select((ch, pos) =>
                        {
                            // Logic xử lý JsonContent
                            // Bạn cần quyết định cách bạn muốn kết hợp các lựa chọn thay thế
                            // Ví dụ: lấy lựa chọn đầu tiên hoặc kết hợp chúng thành một chuỗi
                            string processedJsonContent = string.Empty;
                            if (ch.JsonContent != null && ch.JsonContent.Any())
                            {
                                var parts = new List<string>();
                                foreach (var element in ch.JsonContent)
                                {
                                    if (element.ValueKind == JsonValueKind.String)
                                    {
                                        parts.Add(element.GetString() ?? string.Empty);
                                    }
                                    else if (element.ValueKind == JsonValueKind.Array)
                                    {
                                        // Nếu là mảng, lấy phần tử đầu tiên (hoặc xử lý logic phức tạp hơn)
                                        var innerArray = element.EnumerateArray().ToList();
                                        if (innerArray.Any() && innerArray[0].ValueKind == JsonValueKind.String)
                                        {
                                            parts.Add(innerArray[0].GetString() ?? string.Empty);
                                        }
                                    }
                                }
                                processedJsonContent = string.Join(" ", parts); // Nối các phần lại thành một chuỗi
                            }

                            return new DictationSentence
                            {
                                LessonId = lessonId,
                                Text = ch.Content ?? string.Empty, // Giữ nguyên Content
                                StartTime = ch.TimeStart.GetValueOrDefault(0),
                                EndTime = ch.TimeEnd.GetValueOrDefault(0),
                                AudioUrl = ch.AudioSrc,
                                Position = pos + 1,
                                CreatedBy = adminId,
                                CreatedAt = now
                            };
                        }).ToList();

                    await _dbContext.DictationSentences.AddRangeAsync(sentences);
                    await _dbContext.SaveChangesAsync(); // Sửa lại dòng này nếu DbContext là property
                }

                return (true, "Lesson and sentences seeded/updated successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error seeding lesson: {ex.Message}");
                Console.Error.WriteLine($"Stack Trace: {ex.StackTrace}");
                return (false, $"Error: {ex.Message}");
            }
        }
    }
}