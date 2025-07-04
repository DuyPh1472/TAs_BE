using TAs.Application.Lessons.DTOs;

namespace TAs.Application.Categories.DTOs.Retrieval
{
    public class GetCategoryByTitleDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public string Accent { get; set; } = string.Empty;
        public float Duration { get; set; }
        public List<LessonDTO> Lessons { get; set; } = null!;

    }
}