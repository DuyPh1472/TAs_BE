using Microsoft.EntityFrameworkCore;
using TAs.Application.Interfaces.Repositories;
using TAs.Domain.Entities;
using TAs.Infrastructure.Persistence;
using TAs.Infrastructure.Persistence.GenericRepo;

namespace TAs.Infrastructure.Repositories
{
    public class SkillRepository(TAsDbContext dbContext)
     : GenericRepository<Skill>(dbContext), ISkillRepository
    {
        private readonly TAsDbContext dbContext = dbContext;

        public async Task<IEnumerable<Skill>> GetAllSkillsAsync()
        {
            var skills = await dbContext.Skills.ToListAsync();
            return skills;
        }

        public async Task<Skill?> GetSkillByIdAsync(Guid id)
        {
            return await dbContext.Skills
                .FirstOrDefaultAsync(skill => skill.Id == id);
        }

        public async Task<Skill?> GetSkillByNameAsync(string name)
        {
            return await dbContext.Skills
            .FirstOrDefaultAsync(skill => skill.Name == name);
        }

        public async Task<bool> IsDuplicated(string skillName, string skillPath)
        {
            return await dbContext.Skills.AnyAsync(s => s.Name == skillName
            || s.Path == skillPath);
        }
    }
}