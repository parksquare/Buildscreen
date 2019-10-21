using System;
using Newtonsoft.Json;

namespace ParkSquare.BuildScreen.Core.AzureDevOps.Dto
{
    public class BuildDto
    {
        public string Id { get; set;  }

        public string BuildNumber { get; set; }

        public string Status { get; set; }

        public string Result { get; set; }

        public DateTime QueueTime { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime FinishTime { get; set; }

        public string Url { get; set; }

        public DefinitionDto Definition { get; set; }

        public ProjectDto Project { get; set; }

        public string Uri { get; set; }

        public string SourceBranch { get; set; }
        
        public string SourceVersion { get; set; }

        public ControllerDto Controller { get; set; }

        public string Priority { get; set; }

        public string Reason { get; set; }

        public UserDto RequestedFor { get; set; }

        public UserDto RequestedBy { get; set; }

        public DateTime LastChangedDate { get; set; }

        public UserDto LastChangedBy { get; set; }

        public bool KeepForever { get; set; }

        public LogsDto Logs { get; set; }

        public RepositoryDto Repository { get; set; }

        [JsonProperty("_links")]
        public LinksDto Links { get; set; }

        public int QueuePosition { get; set; }

        // JSON slug
        public string Parameters { get; set; }

        public QueueDto Queue { get; set; }
    }
}