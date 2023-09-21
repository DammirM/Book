using Minimal_Api_Book.Data;

namespace Minimal_Api_Book.Services
{
    public interface IGenericRepository<T, TCreate>
    {
        Task<TCreate> Add(TCreate t);
        Task<IEnumerable<T>> GetAll(); 
        Task<T> GetSingleById(int id); 
        Task<T> Update(int id, T t);
        Task<T> Delete(int id);
    }
}
