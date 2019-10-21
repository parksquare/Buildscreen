namespace ParkSquare.BuildScreen.Core.AzureDevOps.Dto
{
    public class TestRunDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public bool IsAutomated { get; set; }

        public string State { get; set; }

        public int TotalTests { get; set; }

        public int IncompleteTests { get; set; }

        public int NotApplicableTests { get; set; }

        public int PassedTests { get; set; }

        public int UnanalyzedTests { get; set; }

        public int Revision { get; set; }

        public string WebAccessUrl { get; set; }
    }
}