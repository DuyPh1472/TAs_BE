using TAs.Domain.Entities;
using TAs.Domain.Repositories;
using TAs.Infrastructure.Persistence;
using TAs.Infrastructure.Persistence.GenericRepo;

namespace TAs.Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(TAsDbContext context) : base(context)
        {
        }

        // public async Task<Category> GetCategoryByIdAsync(Guid categoryId)
        // {
        //     return await 
        // }
    }
}