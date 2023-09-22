using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using Minimal_Api_Book.Data;
using Minimal_Api_Book.Services;
using Newtonsoft.Json;
using System.Net.Http;
using System.Reflection.Metadata;
using Web_Book.Models;
using Web_Book.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Web_Book.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BookController> _logger;


        public BookController(IBookService bookservice, ILogger<BookController> logger)
        {
            _bookService = bookservice;
            _logger = logger;
        }
        public async Task<IActionResult> BookIndex()
        {
            List<Book> list = new List<Book>();

            var response = await _bookService.GetAllBooks<ResponseDto>();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<Book>>(Convert.ToString(response.Result));
            }

            return View(list);
        }

        public async Task<IActionResult> BookDetails(int id)
        {
            var response = await _bookService.GetBookById<ResponseDto>(id);
            if (response != null && response.IsSuccess)
            {
                Book model = JsonConvert.DeserializeObject<Book>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }

        public async Task<IActionResult> BookCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BookCreate(CreateBookDto book)
        {
            if (ModelState.IsValid)
            {
                var response = await _bookService.CreateBookAsync<ResponseDto>(book);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(BookIndex));
                }
            }
            return View(book);
        }

        public async Task<IActionResult> UpdateBook(int id)
        {
            var response = await _bookService.GetBookById<ResponseDto>(id);
            if (response != null && response.IsSuccess)
            {
                Book model = JsonConvert.DeserializeObject<Book>(Convert.ToString(response.Result));

                return View(model);
            }
            return NotFound();
        }



        [HttpPost]
        public async Task<IActionResult> UpdateBook(Book book)
        {
            if (ModelState.IsValid)
            {
                var response = await _bookService.UpdateBookAsync<ResponseDto>(book);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(BookIndex));
                }
            }
            return View(book);
        }


        public async Task<IActionResult> Deletebook(int id)
        {
            var response = await _bookService.GetBookById<ResponseDto>(id);
            if (response != null && response.IsSuccess)
            {
                Book model = JsonConvert.DeserializeObject<Book>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> DeleteBook(Book model)
        {

            var response = await _bookService.DeleteBookAsync<ResponseDto>(model.BookId);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(BookIndex));
            }

            return NotFound();
        }

        public async Task<IActionResult> AvailableBooksForLoan()
        {

            List<Book> list = new List<Book>();

            var response = await _bookService.GetAllBooks<ResponseDto>();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<Book>>(Convert.ToString(response.Result));

                list = list.Where(b => b.Loan).ToList();
            }

            return View(list);
        }

        public async Task<IActionResult> MyIndex()
        {
            {
                return View();
            }

        }

        public async Task<IActionResult> GenreSearch()
        {
            return View(); // Return the GenreSearch.cshtml view
        }

        [HttpGet]
        public async Task<IActionResult> GenreSearchResults(string genreName)
        {
            // Call the repository method to get books by genre name
            var response = await _bookService.GetBooksByGenreNameAsync<ResponseDto>(genreName);

            if (response != null && response.IsSuccess)
            {
                // Deserialize the response
                var bookList = JsonConvert.DeserializeObject<List<Book>>(Convert.ToString(response.Result));

                // Filter books by genre name
                var filteredBooks = bookList.Where(b => b.Genre != null && b.Genre.GenreName == genreName).ToList();

                return View("GenreSearchResults", filteredBooks);
            }

            // No books found for the specified genre, display a message
            ViewBag.ErrorMessage = "No books found for the specified genre.";
            return View("GenreSearchResults", new List<Book>());
        }


    }
}
