using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Minimal_Api_Book.Context;
using Minimal_Api_Book.Data;
using System.Linq;

namespace Minimal_Api_Book.Services
{
    public class BookRepo : IBookRepository
    {

       private readonly DataContext _Context;

        public BookRepo(DataContext db)
        {
            _Context = db;
        }
        public async Task<Book> AddBook(Book book)
        {
            // Check if a book with the same BookId already exists
            var existingBook = await _Context.Books.FindAsync(book.BookId);
            if (existingBook != null)
            {
                return null; // Return null if book with the same BookId already exists
            }

            var addedBook = await _Context.Books.AddAsync(book);
            await _Context.SaveChangesAsync();
            return addedBook.Entity;
        }

        public async Task<Book> DeleteBook(int id)
        {
            var BookToDelete = await _Context.Books.FirstOrDefaultAsync(b => b.BookId == id);

            if (BookToDelete != null)
            {
                _Context.Books.Remove(BookToDelete);
                await _Context.SaveChangesAsync();
                return BookToDelete;
            }
            return null;

        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _Context.Books.ToListAsync();
        }

        public async Task<Book> GetSingleBook(int id)
        {
            var book = await _Context.Books.FirstOrDefaultAsync(b => b.BookId == id);

            return book;
        }

        public async Task<Book> UpdateBook(int id, Book book)
        {
            var updatebook = await _Context.Books.FirstOrDefaultAsync(b => b.BookId == id);
            if (updatebook != null)
            {
                updatebook.Titel = book.Titel;
                updatebook.Author = book.Author;
                updatebook.About = book.About;
                updatebook.Loan = book.Loan;
                updatebook.Year = book.Year;

                await _Context.SaveChangesAsync();
                return updatebook;
            }

            return null;
        }
    }
}
