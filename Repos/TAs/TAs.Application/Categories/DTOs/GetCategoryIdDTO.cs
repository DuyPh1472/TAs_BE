using TAs.Application.Lessons.DTOs;
using TAs.Domain.Entities;

namespace TAs.Application.Categories.DTOs;

public class GetCategoryIdDTO
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Difficult { get; set; } = string.Empty;
    public string Accent { get; set; } = string.Empty;
    public float Duration { get; set; }
    public List<LessonDTO> Lessons { get; set; } = null!;
    public static GetCategoryIdDTO FromEntity(Category category, List<Lesson> lessons)
    {
        return new GetCategoryIdDTO
        {
            Accent = category.Accent,
            Description = category.Description,
            Difficult = category.Difficult,
            Duration = category.Duration,
            Title = category.Title,
            Lessons = lessons.Select(l => new LessonDTO
            {
                LessonId = l.LessonId,
                Accent = l.Accent,
                Description = l.Description,
                Duration = l.Duration,
                Level = l.Level,
                Sentences = l.Sentences,
                Title = l.Title,
                Topics = l.Topics
            }).ToList()
        };
    }
}

