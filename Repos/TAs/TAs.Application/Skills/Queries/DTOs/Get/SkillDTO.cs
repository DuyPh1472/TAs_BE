using TAs.Domain.Entities;

namespace TAs.Application.Skills.DTOs
{
    public class SkillDTO : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string Path { get; set; } = default!;
        public string Icon { get; set; } = default!;
        public string Color { get; set; } = default!;
        public string Description { get; set; } = default!;
        public static SkillDTO FromEntity(Skill skill)
        {
            return new SkillDTO
            {

                Id = skill.Id,
                Color = skill.Color,
                Description = skill.Description,
                Icon = skill.Icon,
                Name = skill.Name,
                Path = skill.Path,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedBy = skill.CreatedBy
            };
        }
    }
}