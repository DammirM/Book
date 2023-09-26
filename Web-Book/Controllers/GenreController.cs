using Microsoft.AspNetCore.Mvc;
using Minimal_Api_Book.Data;
using Newtonsoft.Json;
using Web_Book.Models;
using Web_Book.Services;

namespace Web_Book.Controllers
{
    public class GenreController : Controller
    {
        private readonly IGenreService _genreService;
        private readonly ILogger<BookController> _logger;


        public GenreController(IGenreService genreService, ILogger<BookController> logger)
        {
            _genreService = genreService;
            _logger = logger;
        }

        public async Task<IActionResult> GenreIndex()
        {
            List<Genre> list = new List<Genre>();

            var response = await _genreService.GetAllGenre<ResponseDto>();

            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<Genre>>(Convert.ToString(response.Result));
            }

            return View(list);
        }

        public async Task<IActionResult> GenreDetails(int id)
        {
            var response = await _genreService.GetGenreById<ResponseDto>(id);
            if (response != null && response.IsSuccess)
            {
                Genre model = JsonConvert.DeserializeObject<Genre>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }

        public async Task<IActionResult> GenreCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GenreCreate(CreateGenreDto genre)
        {
            if (ModelState.IsValid)
            {
                var response = await _genreService.CreateGenre<ResponseDto>(genre);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(GenreIndex));
                }
            }
            return View(genre);
        }

        public async Task<IActionResult> UpdateGenre(int id)
        {
            var response = await _genreService.GetGenreById<ResponseDto>(id);
            if (response != null && response.IsSuccess)
            {
                Genre model = JsonConvert.DeserializeObject<Genre>(Convert.ToString(response.Result));

                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateGenre(Genre genre)
        {
            if (ModelState.IsValid)
            {
                var response = await _genreService.UpdateGenre<ResponseDto>(genre);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(GenreIndex));
                }
            }
            return View(genre);
        }

        public async Task<IActionResult> DeleteGenre(int id)
        {
            var response = await _genreService.GetGenreById<ResponseDto>(id);
            if (response != null && response.IsSuccess)
            {
                Genre model = JsonConvert.DeserializeObject<Genre>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteGenre(Genre model)
        {

            var response = await _genreService.DeleteGenre<ResponseDto>(model.GenreId);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(GenreIndex));
            }

            return NotFound();
        }
    }
}
