using System;
using System.Collections.Generic;

namespace ParkSquare.BuildScreen.Web.AzureDevOps
{
    public interface IConfig
    {
        string AuthToken { get; set; }

        int TimeoutSeconds { get; set; }

        string Projects { get; set; }

        int MaxBuildAgeDays { get; set; }

        string ServerUrl { get; set; }

        string ProjectCollection { get; set; }

        Uri ApiBaseUrl { get; }

        string RemoveWords { get; set; }
        
        IReadOnlyCollection<string> TeamProjects { get; }

        IReadOnlyCollection<string> HiddenWords { get; }
    }
}