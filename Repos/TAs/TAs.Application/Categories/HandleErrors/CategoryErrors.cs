using TAs.Domain.Errors;

namespace TAs.Application.Categories.HandleErrors
{
    public class CategoryErrors
    {
        public static Error NoCategoryFound(Guid categoryId)
        => new("NoCategoryFound", $"category: {categoryId} does not exist.");
        public static Error NoTitleFound(string title)
        => new("NoTitleFound", $"Category's title: {title} does not exist.");
        public static Error NoSkillFound(string skill)
        => new("NoSkillFound", $"Categories with skill's name: {skill} does not exist.");
        
        
    }
}