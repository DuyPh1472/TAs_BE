using Microsoft.EntityFrameworkCore;
using TAs.Application.Categories.DTOs.Retrieval;
using TAs.Application.Interfaces.Repositories;
using TAs.Domain.Entities;
using TAs.Infrastructure.Persistence;
using TAs.Infrastructure.Persistence.GenericRepo;

namespace TAs.Infrastructure.Repositories
{
    public class CategoryRepository(TAsDbContext context) : GenericRepository<Category>(context), ICategoryRepository
    {
        public async Task<GetCategoryIdDTO?> GetCategoryByIdAsync(Guid categoryId)
        {
            return await context
                  .Categories
                  .AsNoTracking()
                  .Include(c => c.CategoryLessons).ThenInclude(cl => cl.Lesson)
                  .Where(c => c.Id == categoryId)
                  .Select(c => GetCategoryIdDTO.FromEntity(c))
                  .FirstOrDefaultAsync();
        }
    }
}