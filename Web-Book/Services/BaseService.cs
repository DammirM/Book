using Newtonsoft.Json;
using System.Text;
using Web_Book.Models;

namespace Web_Book.Services
{
    public class BaseService : IBaseService
    {
        public ResponseDto responseModel { get; set; }
        public IHttpClientFactory _httpClient { get; set; }

        public BaseService(IHttpClientFactory httpClient)
        {
            _httpClient= httpClient;
            responseModel= new ResponseDto();
        }
        

        public async Task<T> SendAsync<T>(ApiRequest apirequest)
        {
            try
            {
                var client = _httpClient.CreateClient("BooksDbApi");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apirequest.Url);
                client.DefaultRequestHeaders.Clear();
                if (apirequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apirequest.Data),
                        Encoding.UTF8, "application/json");

                }

                HttpResponseMessage apiResp = null;

                switch (apirequest.ApiType)
                {
                    case StaticDetails.ApiType.GET:
                        message.Method = HttpMethod.Get;
                        break;
                    case StaticDetails.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case StaticDetails.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case StaticDetails.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                }

                apiResp = await client.SendAsync(message);

                var apiContent = await apiResp.Content.ReadAsStringAsync();
                var apiResponse = JsonConvert.DeserializeObject<T>(apiContent);
                return apiResponse;

            }
            catch (Exception e)
            {

                var dto = new ResponseDto
                {
                    DisplayMessage = "Error",
                    ErrorMessages = new List<string> { Convert.ToString(e.Message) },
                    IsSuccess = false

                };

                var res = JsonConvert.SerializeObject(dto);
                var apiResponsDto = JsonConvert.DeserializeObject<T>(res);
                return apiResponsDto;
            }
            
        }


        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}
