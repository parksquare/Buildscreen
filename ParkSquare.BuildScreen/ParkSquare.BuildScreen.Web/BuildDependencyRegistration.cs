using Microsoft.Extensions.DependencyInjection;
using ParkSquare.BuildScreen.AzureDevOps.Build;
using ParkSquare.BuildScreen.Core.Build;

namespace ParkSquare.BuildScreen.Web
{
    public static class BuildDependencyRegistration
    {
        public static IServiceCollection AddBuildDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IBuildService, BuildService>();

            // Azure DevOps build provider 
            services.AddSingleton<IBuildProvider, AzureDevOpsBuildProvider>();
            services.AddSingleton<IHttpClientFactory, HttpClientFactory>();
            services.AddSingleton<IBuildDtoConverter, BuildDtoConverter>();
            services.AddSingleton<IBuildFilter, LatestBuildsFilter>();
            services.AddSingleton<IBuildFilter, CompletedPullRequestFilter>();
            services.AddSingleton<ITestResultsProvider, TestResultsProvider>();
            services.AddSingleton<IAzureDevOpsConfig, AzureDevOpsConfig>();
            services.AddSingleton<IBranchNameConverter, BranchNameConverter>();
            services.AddSingleton<IDisplayTransformer, DisplayTransformer>();

            return services;
        }
    }
}
