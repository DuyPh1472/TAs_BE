using Microsoft.EntityFrameworkCore;
using TAs.Domain.IGenericRepo;
namespace TAs.Infrastructure.Persistence.GenericRepo
{

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly TAsDbContext _context;
        protected readonly DbSet<T> _entities;
        public GenericRepository(TAsDbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public void Add(T model)
        {
            try
            {
                _entities.Add(model);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public void Delete(T model)
        {
            try
            {
                _entities.Remove(model);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Update(T model)
        {
            try
            {
                _entities.Update(model);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());

            }
        }
    }
}