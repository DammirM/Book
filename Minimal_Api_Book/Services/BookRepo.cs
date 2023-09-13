using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Minimal_Api_Book.Context;
using Minimal_Api_Book.Data;
using System.Linq;

namespace Minimal_Api_Book.Services
{
    public class BookRepo : IGenericRepository<Book, CreateBookDto>
    {

       private readonly DataContext _Context;
        private readonly IMapper _mapper;

        public BookRepo(DataContext db, IMapper mapper)
        {
            _Context = db;
            _mapper = mapper;
        }
        public async Task<CreateBookDto> Add(CreateBookDto bookDto)
        {
            var existingBook = await _Context.Books.FirstOrDefaultAsync(b =>
                b.Titel == bookDto.Titel &&
                b.Author == bookDto.Author);

            if (existingBook != null)
            {
                return null;
            }

            var book = _mapper.Map<Book>(bookDto);

            var addedBook = await _Context.Books.AddAsync(book);
            await _Context.SaveChangesAsync();

            return _mapper.Map<CreateBookDto>(addedBook.Entity);
        }

        public async Task<Book> Delete(int id)
        {
            var bookToDelete = await _Context.Books.FirstOrDefaultAsync(b => b.BookId == id);

            if (bookToDelete != null)
            {
                _Context.Books.Remove(bookToDelete);
                await _Context.SaveChangesAsync();
                return bookToDelete;
            }

            return null;
        }


        public async Task<IEnumerable<Book>> GetAll()
        {
            var books = await _Context.Books.ToListAsync();

            return books;
        }

        public async Task<Book> GetSingleById(int id)
        {
            var book = await _Context.Books.FirstOrDefaultAsync(b => b.BookId == id);

            return book;
        }


        public async Task<Book> Update(int id, Book bookDto)
        {
            var updatebook = await _Context.Books.FirstOrDefaultAsync(b => b.BookId == id);

            if (updatebook != null)
            {
                updatebook.Titel = bookDto.Titel;
                updatebook.Author = bookDto.Author;
                updatebook.About = bookDto.About;
                updatebook.Loan = bookDto.Loan;
                updatebook.Year = bookDto.Year;

                await _Context.SaveChangesAsync();

                return updatebook;
            }

            return null;
        }

    }
}
