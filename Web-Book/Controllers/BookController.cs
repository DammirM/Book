using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using Minimal_Api_Book.Data;
using Newtonsoft.Json;
using System.Reflection.Metadata;
using Web_Book.Models;
using Web_Book.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Web_Book.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookservice)
        {
            _bookService= bookservice;
        }
        //public async Task<IActionResult> BookIndex()
        //{
        //    List<Book> list = new List<Book>();
        //    var response = await _bookService.GetAllBooks<ResponseDto>();
        //    if (response != null)
        //    {
        //        list = JsonConvert.DeserializeObject<List<Book>>(Convert.ToString(response.Result));
        //    }

        //    return View(list);
        //}
        public async Task<IActionResult> BookIndex()
        {
            List<Book> list = new List<Book>();

            try
            {
                var response = await _bookService.GetAllBooks<ResponseDto>();

                if (response != null && response.IsSuccess)
                {
                    list = JsonConvert.DeserializeObject<List<Book>>(Convert.ToString(response.Result));
                }
                else
                {
                    // Handle the case where the API response is not successful
                    // You can add error messages or handle it as needed
                    ViewBag.ErrorMessage = "Failed to retrieve book data.";
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the API call
                ViewBag.ErrorMessage = "An error occurred: " + ex.Message;
            }

            return View(list);
        }







        //public async Task<IActionResult> BookDetails(int id)
        //{
        //    var response = await _bookService.GetBookById<ResponseDto>(id);
        //    if (response != null && response.IsSuccess)
        //    {
        //        BookDto model = JsonConvert.DeserializeObject<BookDto>(Convert.ToString(response.Result));
        //        return View(model);
        //    }
        //    return NotFound();
        //}

        //public async Task<IActionResult> BookCreate()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> BookCreate(Book book)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var response = await _bookService.CreateBookAsync<ResponseDto>();
        //        if (response != null)
        //        {
        //            return RedirectToAction(nameof(BookIndex));
        //        }
        //    }
        //    return View(book);
        //}

        //public async Task<IActionResult> UpdateBook(int id)
        //{
        //    var response = await _bookService.GetBookById<ResponseDto>(id);
        //    if (response != null && response.IsSuccess)
        //    {
        //        BookDto model = JsonConvert.DeserializeObject<BookDto>(Convert.ToString(response.Result));
        //        return View(model);
        //    }
        //    return NotFound();
        //}

        //[HttpPost]
        //public async Task<IActionResult> UpdateBook(Book model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var response = await _bookService.UpdateBookAsync<BookDto>(model);
        //        if (response != null)
        //        {
        //            return RedirectToAction(nameof(model));
        //        }
        //    }
        //    return View(model);
        //}


        //public async Task<IActionResult> Deletebook(int id)
        //{
        //    var response = await _bookService.GetBookById<ResponseDto>(id);
        //    if (response != null && response.IsSuccess)
        //    {
        //        BookDto model = JsonConvert.DeserializeObject<BookDto>(Convert.ToString(response.Result));
        //        return View(model);
        //    }
        //    return NotFound();
        //}


        //[HttpPost]
        //public async Task<IActionResult> DeleteBook(BookDt model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var response = await _bookService.DeleteBookByIdAsync<BookDto>(model.BookId);
        //        if (response != null && response.iss)
        //        {
        //            return RedirectToAction(nameof(model));
        //        }
        //    }
        //    return NotFound();
        //}
    }
}
