using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace ParkSquare.BuildScreen.AzureDevOps
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly HttpClient _jsonClient;
        private readonly HttpClient _client;

        public HttpClientFactory(IAzureDevOpsConfig config)
        {
            _jsonClient = CreateInstance(config, "application/json");
            _client = CreateInstance(config, null);
        }

        public HttpClient GetJsonClient()
        {
            return _jsonClient;
        }

        public HttpClient GetClient()
        {
            return _client;
        }

        private static HttpClient CreateInstance(IAzureDevOpsConfig config, string contentType)
        {
            var client = new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip
            });

            client.DefaultRequestHeaders.Accept.Clear();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes($":{config.AuthToken}")));

            if (!string.IsNullOrEmpty(contentType))
            {
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.Timeout = TimeSpan.FromSeconds(config.TimeoutSeconds);
            }

            return client;
        }
    }
}
