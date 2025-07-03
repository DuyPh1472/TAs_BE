using TAs.Domain.Entities;
using TAs.Domain.IGenericRepo;

namespace TAs.Domain.Repositories
{
    public interface ISkillRepository : IGenericRepository<Skill>
    {
        Task<IEnumerable<Skill>> GetAllSkillsAsync();
        Task<Skill?> GetSkillByIdAsync(Guid id);
        Task<bool> IsDuplicated(string skillName , string skillPath);
    }
}