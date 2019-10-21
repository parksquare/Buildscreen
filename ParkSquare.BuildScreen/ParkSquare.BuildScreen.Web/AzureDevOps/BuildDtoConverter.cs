using System;
using ParkSquare.BuildScreen.Web.AzureDevOps.Dto;
using ParkSquare.BuildScreen.Web.Build;

namespace ParkSquare.BuildScreen.Web.AzureDevOps
{
    public class BuildDtoConverter : IBuildDtoConverter
    {
        private readonly IBranchNameConverter _branchNameConverter;
        private readonly IDisplayTransformer _displayTransformer;

        public BuildDtoConverter(IBranchNameConverter branchNameConverter, IDisplayTransformer displayTransformer)
        {
            _branchNameConverter = branchNameConverter ?? throw new ArgumentNullException(nameof(branchNameConverter));
            _displayTransformer = displayTransformer ?? throw new ArgumentNullException(nameof(displayTransformer));
        }

        public Build.Build Convert(BuildDto buildDto, TestResults testResults)
        {
            return new Build.Build
            {
                Id = buildDto.Id,
                BuildReportUrl = buildDto.Links.Web.Href,
                RequestedByName = buildDto.RequestedFor.DisplayName,
                Status = string.IsNullOrEmpty(buildDto.Result) ? buildDto.Status : buildDto.Result,
                TotalNumberOfTests = testResults?.TotalTests ?? 0,
                PassedNumberOfTests = testResults?.PassedTests ?? 0,
                TeamProject = _displayTransformer.Tranform(buildDto.Project.Name),
                BuildDefinition = _displayTransformer.Tranform(buildDto.Definition.Name),
                StartBuildDateTime = buildDto.StartTime,
                FinishBuildDateTime = buildDto.FinishTime,
                Branch = ConvertBranchName(buildDto.SourceBranch),
                RepoName = _displayTransformer.Tranform(buildDto.Repository.Name),
                RequestedForId = buildDto.RequestedFor.Id,
                RequestedForUniqueName = buildDto.RequestedFor.UniqueName
            };
        }

        private string ConvertBranchName(string branchName)
        {
            return _branchNameConverter.Convert(branchName);
        }
    }
}
