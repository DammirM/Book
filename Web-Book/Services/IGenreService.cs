using Minimal_Api_Book.Data;

namespace Web_Book.Services
{
    public interface IGenreService
    {
        Task<T> GetAllGenre<T>();
        Task<T> GetGenreById<T>(int id);
        Task<T> CreateGenre<T>(CreateGenreDto genre);
        Task<T> UpdateGenre<T>(Genre genre);
        Task<T> DeleteGenre<T>(int id);
    }
}
