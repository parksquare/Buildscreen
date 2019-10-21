using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace ParkSquare.BuildScreen.Core.AzureDevOps
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly HttpClient _httpClient;

        public HttpClientFactory(IAzureDevOpsConfig config)
        {
            _httpClient = CreateInstance(config);
        }

        public HttpClient GetClientInstance()
        {
            return _httpClient;
        }

        private static HttpClient CreateInstance(IAzureDevOpsConfig config)
        {
            var client = new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip
            });

            client.DefaultRequestHeaders.Accept.Clear();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes($":{config.AuthToken}")));

            client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = TimeSpan.FromSeconds(config.TimeoutSeconds);

            return client;
        }
    }
}
