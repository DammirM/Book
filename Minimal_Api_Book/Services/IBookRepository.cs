using Minimal_Api_Book.Data;

namespace Minimal_Api_Book.Services
{
    public interface IBookRepository
    {
        Task<Book> AddBook(Book book);// Klar
        Task<IEnumerable<Book>> GetAllBooks(); // Klar
        Task<Book> GetSingleBook(int id); // Klar
        Task<Book> UpdateBook(int id, Book book);// IGÅÅÅÅÅNG
        Task<Book> DeleteBook(int id);// KLAR
    }
}
