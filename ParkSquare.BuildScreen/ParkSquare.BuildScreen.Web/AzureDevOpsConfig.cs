using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using ParkSquare.BuildScreen.AzureDevOps.Build;

namespace ParkSquare.BuildScreen.Web
{
    public class AzureDevOpsConfig : IAzureDevOpsConfig
    {
        public AzureDevOpsConfig(IConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            configuration.Bind("AzureDevOpsProvider", this);

            ApiBaseUrl = GetApiBaseUrl();
            TeamProjects = ParseMultiString(Projects);
            HiddenWords = ParseMultiString(RemoveWords);
        }

        public string AuthToken { get; set; }

        public int TimeoutSeconds { get; set; }

        public string Projects { get; set; }

        public int MaxBuildAgeDays { get; set; }

        public string ServerUrl { get; set; }

        public string ProjectCollection { get; set; }

        public Uri ApiBaseUrl { get; }

        public string RemoveWords { get; set; }

        public IReadOnlyCollection<string> TeamProjects { get; }

        public IReadOnlyCollection<string> HiddenWords { get; }

        private Uri GetApiBaseUrl()
        {
            var separator = ServerUrl.EndsWith('/') ? string.Empty : "/";
            return new Uri($"{ServerUrl}{separator}{ProjectCollection}/");
        }

        private static string[] ParseMultiString(string value)
        {
            return value.Split(",");
        }
    }
}