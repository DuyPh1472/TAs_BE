using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TAs.Domain.IGenericRepo;
namespace TAs.Infrastructure.Persistence.GenericRepo
{

    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly TAsDbContext _context;
        private readonly DbSet<T> _entities;
        private readonly ILogger<GenericRepository<T>> _logger;
        public GenericRepository(TAsDbContext context, ILogger<GenericRepository<T>> logger)
        {
            _context = context;
            _entities = context.Set<T>();
            _logger = logger;
        }

        public void Add(T model)
        {
            try
            {
                _entities.Add(model);
            }
            catch (Exception)
            {
                _logger.LogError("An error occurred while adding an entity of type {EntityType}.", typeof(T).Name);
            }
        }

        public void Delete(T model)
        {
            try
            {
                _entities.Remove(model);
            }
            catch (Exception)
            {
                _logger.LogError("An error occurred while deleting an entity of type {EntityType}.", typeof(T).Name);
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
                _logger.LogError(ex, "An error occurred while updating an entity of type {EntityType}.", typeof(T).Name);
            }
        }
    }
}