using Minimal_Api_Book.Data;

namespace Web_Book.Services
{
    public interface IBookService
    {

        Task<T> GetAllBooks<T>();
        Task<T> GetBookById<T>(int id);
        Task<T> CreateBookAsync<T>(CreateBookDto book);
        Task<T> UpdateBookAsync<T>(Book book);
        Task<T> DeleteBookAsync<T>(int id);
        Task<T> AvailableBooksForLoan<T>();
        Task<T> GetBooksByGenreNameAsync<T>(string GenreName);


    }
}
