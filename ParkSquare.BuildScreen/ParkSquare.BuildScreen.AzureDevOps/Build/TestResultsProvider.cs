using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ParkSquare.BuildScreen.AzureDevOps.Build.Dto;
using ParkSquare.BuildScreen.Core.Build;

namespace ParkSquare.BuildScreen.AzureDevOps.Build
{
    public class TestResultsProvider : ITestResultsProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAzureDevOpsConfig _config;

        public TestResultsProvider(IHttpClientFactory httpClientFactory, IAzureDevOpsConfig config)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task<TestResults> GetTestsForBuildAsync(string project, string buildUri)
        {
            var client = _httpClientFactory.GetJsonClient();
            var requestPath = GetRequestPath(project, buildUri);
            var requestUri = new Uri(_config.ApiBaseUrl, requestPath);

            using (var response = await client.GetAsync(requestUri))
            {
                if (response.IsSuccessStatusCode)
                {
                    var deserialized = await DeserializeResponseAsync(response);
                    return CalculateTestResults(deserialized.Value, buildUri);
                }

                throw new AzureDevOpsProviderException(
                    $"Unable to get test results for {project} build {buildUri}. " +
                    $"Call to '{requestUri}' returned {response.StatusCode}: {response.ReasonPhrase}");
            }
        }

        private static async Task<GetTestRunsResponseDto> DeserializeResponseAsync(HttpResponseMessage response)
        {
            var deserialized = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GetTestRunsResponseDto>(deserialized);
        }

        private static string GetRequestPath(string project, string buildUri)
        {
            return $"{project}/_apis/test/runs?buildUri={buildUri}&$top=5000&api-version=5.0";
        }

        private static TestResults CalculateTestResults(IReadOnlyCollection<TestRunDto> testResults, string buildUri)
        {
            return new TestResults
            {
                BuildUri = buildUri, 
                TotalTests = testResults.Sum(x => x.TotalTests),
                PassedTests = testResults.Sum(x => x.PassedTests)
            };
        }
    }
}
