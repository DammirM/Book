using Minimal_Api_Book.Data;

namespace Minimal_Api_Book.Services
{
    public interface IGenericRepository<TBooks, TCreateBook>
    {
        Task<TCreateBook> Add(TCreateBook t);
        Task<IEnumerable<TBooks>> GetAll(); 
        Task<TBooks> GetSingleById(int id); 
        Task<TBooks> Update(int id, TBooks t);
        Task<TBooks> Delete(int id);
    }
}
