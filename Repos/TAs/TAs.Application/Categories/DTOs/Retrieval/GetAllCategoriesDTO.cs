using TAs.Domain.Entities;

namespace TAs.Application.Categories.DTOs.Retrieval
{
    public class GetAllCategoriesDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public string Accent { get; set; } = string.Empty;
        public float Duration { get; set; }
        public int LessonsCount { get; set; }
        public static GetAllCategoriesDTO FromEntity(Category category)
        {
            return new GetAllCategoriesDTO
            {
                Id = category.Id,
                Accent = category.Accent,
                Description = category.Description,
                Difficulty = category.Difficult,
                Duration = category.Duration,
                Title = category.Title,
                LessonsCount = category.CategoryLessons?.Count ?? 0
            };
        }
    }
}