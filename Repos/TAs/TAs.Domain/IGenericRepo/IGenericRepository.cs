namespace TAs.Domain.IGenericRepo
{
    public interface IGenericRepository<T> where T : class
    {
        void Add(T model);
        void Delete(T model);
        void Update(T model);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Guid id);
        Task<int> SaveChangeAsync();
    }
}