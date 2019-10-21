using System;

namespace ParkSquare.BuildScreen.Web.AzureDevOps.Dto
{
    public class RecordDto
    {
        public string Id { get; set; }

        public string ParentId { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? FinishTime { get; set; }

        public int? PercentComplete { get; set; }

        public string State { get; set; }

        public string Result { get; set; }

        public int ChangeId { get; set; }

        public DateTime? LastModified { get; set; }

        public string WorkerName { get; set; }

        public int Order { get; set; }

        public int ErrorCount { get; set; }

        public int WarningCount { get; set; }
    }
}