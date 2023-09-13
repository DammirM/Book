using Minimal_Api_Book.Data;

namespace Web_Book.Services
{
    public interface IBookService
    {

        Task<T> GetAllBooks<T>();
        //Task<T> GetBookById<T>(int id);
        //Task<T> CreateBookAsync<T>(T book);
        //Task<T> UpdateBookAsync<T>(T book);
        //Task<T> DeleteBookByIdAsync<T>(int id);
    }
}
