using System.Net.NetworkInformation;

namespace Web_Book
{
    public static class StaticDetails
    {
        public static string BookApiBase { get; set; }

        public enum ApiType
        {
            GET, POST, PUT, DELETE
        }
    }
}
