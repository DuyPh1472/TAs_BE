using TAs.Domain.Errors;

namespace TAs.Application.Categories.HandleErrors
{
    public class CategoryErrors
    {
        public static  Error NoCategoryFound(Guid categoryId)
        => new("NoCategoryFound", $"category:{categoryId} does not exist.");
    }
}