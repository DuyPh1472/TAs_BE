using TAs.Domain.Entities;
using TAs.Domain.IGenericRepo;

namespace TAs.Domain.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        // Task<Category> GetCategoryByIdAsync(Guid categoryId);
    }
}