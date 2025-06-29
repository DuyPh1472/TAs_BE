using TAs.Application.Interfaces;
using TAs.Domain.Repositories;
using TAs.Infrastructure.Persistence;
using TAs.Infrastructure.Repositories;

namespace TAs.Infrastructure.UOW
{
    public class UnitOfWork(TAsDbContext context) : IUnitOfWork
    {
        private ISkillRepository _skillRepository = null!;
        public ISkillRepository SkillRepository
        {
            get
            {
                if (_skillRepository is null)
                {
                    _skillRepository = new SkillRepository(context);
                }
                return _skillRepository;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
                GC.SuppressFinalize(this);
            }
            ;
        }
    }
}