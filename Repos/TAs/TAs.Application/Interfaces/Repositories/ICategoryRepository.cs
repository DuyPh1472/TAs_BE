using TAs.Application.Categories.DTOs.Retrieval;
using TAs.Domain.Entities;
using TAs.Domain.IGenericRepo;

namespace TAs.Application.Interfaces.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<GetCategoryIdDTO?> GetCategoryByIdAsync(Guid categoryId);
        Task<Category?> GetCategoryByTitle(string categoryTitle);
        Task<List<Category>> GetCategoriesBySkillName(string skillName);
    }
}