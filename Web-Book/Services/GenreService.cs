using Minimal_Api_Book.Data;

namespace Web_Book.Services
{
    public class GenreService : BaseService, IGenreService
    {
        private readonly IHttpClientFactory _clientFactory;
        public GenreService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<T> CreateGenre<T>(CreateGenreDto genre)
        {
            return await this.SendAsync<T>(new Models.ApiRequest()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = genre,
                Url = StaticDetails.BookApiBase + "/api/genre",
                Accesstoken = ""
            });
        }

        public async Task<T> DeleteGenre<T>(int id)
        {
            return await this.SendAsync<T>(new Models.ApiRequest
            {
                ApiType = StaticDetails.ApiType.DELETE,
                Url = StaticDetails.BookApiBase + "/api/genre/" + id,
                Accesstoken = ""
            });
        }

        public async Task<T> GetAllGenre<T>()
        {
            return await this.SendAsync<T>(new Models.ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.BookApiBase + "/api/genres",
                Accesstoken = ""
            });
        }

        public async Task<T> GetGenreById<T>(int id)
        {
            return await this.SendAsync<T>(new Models.ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.BookApiBase + "/api/genre/" + id,
                Accesstoken = ""
            });
        }

        public async Task<T> UpdateGenre<T>(Genre genre)
        {
            return await this.SendAsync<T>(new Models.ApiRequest()
            {
                ApiType = StaticDetails.ApiType.PUT,
                Data = genre,
                Url = StaticDetails.BookApiBase + $"/api/genre/{genre.GenreId}",
                Accesstoken = ""
            });
        }
    }
}
