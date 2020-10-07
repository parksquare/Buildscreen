using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace ParkSquare.BuildScreen.AzureDevOps
{
    public class AzureDevOpsClient : IAzureDevOpsClient
    {
        private readonly HttpClient _client;
        private readonly IAzureDevOpsConfig _config;

        public AzureDevOpsClient(HttpClient client, IAzureDevOpsConfig config)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _config = config ?? throw new ArgumentNullException(nameof(config));

            ConfigureClient();
        }

        public HttpClient GetClient()
        {
            return _client;
        }

        private void ConfigureClient()
        {
            _client.BaseAddress = _config.ApiBaseUrl;

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes($":{_config.AuthToken}")));

            _client.Timeout = TimeSpan.FromSeconds(_config.TimeoutSeconds);
        }
    }
}
