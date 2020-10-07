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
        private readonly IAzureDevOpsClient _client; 

        public TestResultsProvider(IAzureDevOpsClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<TestResults> GetTestsForBuildAsync(string project, string buildUri)
        {
            var requestPath = GetRequestPath(project, buildUri);

            using (var response = await _client.GetClient().GetAsync(requestPath))
            {
                if (response.IsSuccessStatusCode)
                {
                    var deserialized = await DeserializeResponseAsync(response);
                    return CalculateTestResults(deserialized.Value, buildUri);
                }

                throw new AzureDevOpsProviderException(
                    $"Unable to get test results for {project} build {buildUri}. " +
                    $"Call to '{requestPath}' returned {response.StatusCode}: {response.ReasonPhrase}");
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
