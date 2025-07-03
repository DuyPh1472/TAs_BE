using TAs.Application.Lessons.DTOs;
using TAs.Domain.Entities;

namespace TAs.Application.Categories.DTOs.Retrieval;

public class GetCategoryIdDTO
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Difficulty { get; set; } = string.Empty;
    public string Accent { get; set; } = string.Empty;
    public float Duration { get; set; }
    public List<LessonDTO> Lessons { get; set; } = null!;
    public static GetCategoryIdDTO FromEntity(Category category)
    {
        return new GetCategoryIdDTO
        {
            Accent = category.Accent,
            Description = category.Description,
            Difficulty = category.Difficult,
            Duration = category.Duration,
            Title = category.Title,
            Lessons = [.. category.CategoryLessons.Select(l => new LessonDTO
            {
                LessonId = l.Lesson.Id,
                Accent = l.Lesson.Accent,
                Description = l.Lesson.Description,
                Duration = l.Lesson.Duration,
                Level = l.Lesson.Level,
                Sentences = l.Lesson.Sentences,
                Title = l.Lesson.Title,
                Topics = l.Lesson.Topics
            })]
        };
    }
}

