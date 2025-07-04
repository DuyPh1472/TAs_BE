using Microsoft.EntityFrameworkCore;
using TAs.Application.Categories.DTOs.Retrieval;
using TAs.Application.Interfaces.Repositories;
using TAs.Domain.Entities;
using TAs.Infrastructure.Persistence;
using TAs.Infrastructure.Persistence.GenericRepo;

namespace TAs.Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly TAsDbContext context;
        public CategoryRepository(TAsDbContext dbContext) : base(dbContext)
        {
            context = dbContext;
        }

        public async Task<List<Category>> GetCategoriesBySkillName(string skillName)
        {
            return await context.Categories
                         .Where(s => s.Skill.Name == skillName)
                         .ToListAsync();
        }

        public async Task<GetCategoryIdDTO?> GetCategoryByIdAsync(Guid categoryId)
        {
            return await context
                  .Categories
                  .Where(c => c.Id == categoryId)
                  .Include(c => c.Lessons)
                  .Select(c => GetCategoryIdDTO.FromEntity(c))
                  .FirstOrDefaultAsync();
        }

        public async Task<Category?> GetCategoryByTitle(string categoryTitle)
        {
            try
            {
                return await context
                    .Categories
                    .AsNoTracking()
                    .Include(c => c.Lessons)
                    .FirstOrDefaultAsync(c => c.Title == categoryTitle);
            }
            catch (Exception ex)
            {
                // Log exception
                throw new Exception("An error occurred while fetching category.", ex);
            }

        }
    }
}