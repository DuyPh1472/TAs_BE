using TAs.Domain.Errors;

namespace TAs.Application.Skills.HandleError
{
    public static class SkillErrors
    {
        public static readonly Error SameSkill = new("Duplicate Error", "Skill name or path already exists.");
        public static Error IdNotFound(Guid Id)
                => new("Error not found", $"Skill with Id: {Id} does not exist.");
    }
}