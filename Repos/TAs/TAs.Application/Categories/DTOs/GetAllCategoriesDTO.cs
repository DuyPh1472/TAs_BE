using TAs.Domain.Entities;

namespace TAs.Application.Categories.DTOs
{
    public class GetAllCategoriesDTO : BaseEntity
    {
        public Guid CategoryId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Difficult { get; set; } = string.Empty;
        public string Accent { get; set; } = string.Empty;
        public float Duration { get; set; }
        public static GetAllCategoriesDTO FromEntity(Category category)
        {
            return new GetAllCategoriesDTO
            {
                CategoryId = category.CategoryId,
                Accent = category.Accent,
                Description = category.Description,
                Difficult = category.Difficult,
                Duration = category.Duration,
                Title = category.Title,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt,
                CreatedBy = category.CreatedBy,
                UpdatedBy = category.UpdatedBy
            };
        }
    }
}