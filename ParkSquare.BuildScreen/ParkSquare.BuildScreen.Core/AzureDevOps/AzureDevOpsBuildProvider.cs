using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ParkSquare.BuildScreen.Core.AzureDevOps.Dto;
using ParkSquare.BuildScreen.Core.Build;

namespace ParkSquare.BuildScreen.Core.AzureDevOps
{
    public class AzureDevOpsBuildProvider : IBuildProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IBuildDtoConverter _buildDtoConverter;
        private readonly List<IBuildFilter> _buildFilters;
        private readonly ITestResultsProvider _testResultsProvider;
        private readonly IAzureDevOpsConfig _config;
        private readonly ILogger<IBuildProvider> _logger;

        public AzureDevOpsBuildProvider(
            IHttpClientFactory httpClientFactory, 
            IBuildDtoConverter buildDtoConverter,
            IEnumerable<IBuildFilter> buildFilters,
            ITestResultsProvider testResultsProvider,
            IAzureDevOpsConfig config,
            ILogger<IBuildProvider> logger)
        {
            _buildFilters = buildFilters == null ? new List<IBuildFilter>() : buildFilters.ToList();
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _buildDtoConverter = buildDtoConverter ?? throw new ArgumentNullException(nameof(buildDtoConverter));
            _testResultsProvider = testResultsProvider ?? throw new ArgumentNullException(nameof(testResultsProvider));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<IReadOnlyCollection<BuildTile>> GetBuildsAsync()
        {
            return GetBuildsAsync(_config.TeamProjects, DateTime.Now.AddDays(-_config.MaxBuildAgeDays));
        }

        public Task<IReadOnlyCollection<BuildTile>> GetBuildsAsync(int sinceHours)
        {
            return GetBuildsAsync(_config.TeamProjects, DateTime.Now.AddHours(-sinceHours));
        }

        private async Task<IReadOnlyCollection<BuildTile>> GetBuildsAsync(IEnumerable<string> projects, DateTime since)
        {
            var client = _httpClientFactory.GetClientInstance();
            var dtos = new List<BuildDto>();

            foreach (var project in projects)
            {
                var requestPath = GetRequestPath(project, since);
                var requestUri = new Uri(_config.ApiBaseUrl, requestPath);

                try
                {
                    using (var response = await client.GetAsync(requestUri))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var deserialized = await DeserializeResponseAsync(response);
                            dtos.AddRange(deserialized.Value);
                        }
                        else
                        {
                            _logger.LogError($"Azure Dev Ops API returned {response.StatusCode} {response.ReasonPhrase}");

                            throw new AzureDevOpsProviderException(
                                "Unable to get latest builds. " +
                                $"Call to '{requestUri}' returned {response.StatusCode}: {response.ReasonPhrase}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error getting {project} builds from Azure Dev Ops API");
                }
            }

            var latestBuilds = ApplyFilters(dtos).ToList();

            var getTestTasks =
                latestBuilds.Select(x =>
                    _testResultsProvider.GetTestsForBuildAsync(x.Project.Name, x.Uri)).ToList();

            await Task.WhenAll(getTestTasks);

            var testResults = getTestTasks.Select(x => x.Result).Where(y => y != null).ToList();

            var converted = new List<BuildTile>();
            foreach (var buildDto in latestBuilds)
            {
                var testsForBuild = testResults.FirstOrDefault(x => x.BuildUri.Equals(buildDto.Uri));
                converted.Add(_buildDtoConverter.Convert(buildDto, testsForBuild));
            }

            return converted;
        }

        private IEnumerable<BuildDto> ApplyFilters(IEnumerable<BuildDto> dtos)
        {
            var filtered = dtos;

            foreach (var filter in _buildFilters)
            {
                filtered = filter.Filter(filtered);
            }

            return filtered;
        }

        private static async Task<GetBuildsResponseDto> DeserializeResponseAsync(HttpResponseMessage response)
        {
            var deserialized = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<GetBuildsResponseDto>(deserialized);
        }

        private static string GetRequestPath(string project, DateTime buildSince)
        {
            // Should use X-MS-ContinuationToken instead of an arbitrarily large number of results, but
            // on inspecting the responses from TFS, said header doesn't seem to be present. Despite it 
            // being mentioned in Access-Control-Expose-Headers.
            // https://www.visualstudio.com/en-us/docs/integrate/api/build/builds 

            return project + "/_apis/build/builds" + CreateFilterQuery(buildSince) + "&$top=5000";
        }

        private static string CreateFilterQuery(DateTime dateTime)
        {
            // For API versions see:
            // https://docs.microsoft.com/en-us/rest/api/azure/devops/?view=azure-devops-server-rest-5.0#api-and-tfs-version-mapping
            return $"?api-version=5.0&minTime={dateTime:yyyy-MM-ddTHH:mm:ss.000Z}&statusFilter=all&queryOrder=queueTimeAscending";
        }
    }
}