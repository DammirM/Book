using Minimal_Api_Book.Data;

namespace Web_Book.Services
{
    public class BookService : BaseService, IBookService
    {

        private readonly IHttpClientFactory _clientFactory;
        public BookService(IHttpClientFactory clientFactory) :base(clientFactory)
        {
            _clientFactory= clientFactory;
        }
        public async Task<T> CreateBookAsync<T>(T book)
        {
            return await this.SendAsync<T>(new Models.ApiRequest()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = book,
                Url = StaticDetails.BookApiBase + "/api/book",
                Accesstoken = ""
            });
        }

        public async Task<T> DeleteBookByIdAsync<T>(int id)
        {
            return await this.SendAsync<T>(new Models.ApiRequest
            {
                ApiType = StaticDetails.ApiType.DELETE,
                Url = StaticDetails.BookApiBase + "/api/coupon/" + id,
                Accesstoken = ""
            });
        }

        public async Task<T> GetAllBooks<T>()
        {
            return await this.SendAsync<T>(new Models.ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.BookApiBase + "/api/books",
                Accesstoken = ""
            });
        }

        public async Task<T> GetBookById<T>(int id)
        {
            return await this.SendAsync<T>(new Models.ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.BookApiBase + "/api/book/" + id,
                Accesstoken = ""
            });
        }

        public async Task<T> UpdateBookAsync<T>(T book)
        {
            return await this.SendAsync<T>(new Models.ApiRequest()
            {
                ApiType = StaticDetails.ApiType.PUT,
                Data = book,
                Url = StaticDetails.BookApiBase + "/api/",
                Accesstoken = ""
            });
        }
    }
}
