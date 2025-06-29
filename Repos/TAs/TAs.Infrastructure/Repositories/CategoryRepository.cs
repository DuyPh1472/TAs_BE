using Microsoft.EntityFrameworkCore;
using TAs.Domain.Entities;
using TAs.Domain.Repositories;
using TAs.Infrastructure.Persistence;
using TAs.Infrastructure.Persistence.GenericRepo;

namespace TAs.Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private TAsDbContext context;
        public CategoryRepository(TAsDbContext context)
        : base(context)
        {
            this.context = context;
        }

        public async Task<Category?> GetCategoryByIdAsync(Guid categoryId)
        {
            return await context
                  .Categories
                  .AsNoTracking()
                  .Include(c => c.CategoryLessons)
                  .ThenInclude(cl => cl.Lesson)
                  .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
        }
    }
}