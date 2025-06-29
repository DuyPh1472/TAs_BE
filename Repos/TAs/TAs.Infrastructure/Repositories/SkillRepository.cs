using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TAs.Domain.Entities;
using TAs.Domain.Repositories;
using TAs.Infrastructure.Persistence;
using TAs.Infrastructure.Persistence.GenericRepo;

namespace TAs.Infrastructure.Repositories
{
    public class SkillRepository : GenericRepository<Skill>, ISkillRepository
    {
        private readonly TAsDbContext dbContext;

        public SkillRepository(TAsDbContext dbContext) 
        : base(dbContext)
        {
            this.dbContext = dbContext;
        }

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

        public async Task<bool> IsDuplicated(string skillName, string skillPath)
        {
            return await dbContext.Skills.AnyAsync(s => s.Name == skillName
            || s.Path == skillPath);
        }
    }
}