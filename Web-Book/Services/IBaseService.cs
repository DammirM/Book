using Web_Book.Models;

namespace Web_Book.Services
{
    public interface IBaseService : IDisposable
    {
        ResponseDto responseModel { get; set; }
        Task<T> SendAsync<T>(ApiRequest apirequest);
    }
}
