using System.Net;
using System.Net.Http;

namespace ParkSquare.BuildScreen.Gravatar
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly HttpClient _httpClient;

        public HttpClientFactory()
        {
            _httpClient = CreateInstance();
        }

        public HttpClient GetClientInstance()
        {
            return _httpClient;
        }

        private static HttpClient CreateInstance()
        {
            var client = new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip
            });

            return client;
        }
    }
}
