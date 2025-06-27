namespace TAs.Application.Skills.Commands.DTOs
{
    public class CreateAndUpdateSkill
    {
        public string Name { get; set; } = default!;
        public string Path { get; set; } = default!;
        public string Icon { get; set; } = default!;
        public string Color { get; set; } = default!;
        public string Description { get; set; } = default!;
    }
}