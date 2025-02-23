namespace BrGaapFiscal.Domain.Interfaces
{
    public interface IRepository<T>
        where T : class, IEntity
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(long id);
        Task<bool> Add(T entity);
        Task<bool> Update(T entity);
        Task<bool> Remove(T entity);
    }
}
