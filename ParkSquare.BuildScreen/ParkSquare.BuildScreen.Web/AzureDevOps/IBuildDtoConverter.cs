using ParkSquare.BuildScreen.Web.AzureDevOps.Dto;
using ParkSquare.BuildScreen.Web.Build;

namespace ParkSquare.BuildScreen.Web.AzureDevOps
{
    public interface IBuildDtoConverter
    {
        Build.Build Convert(BuildDto build, TestResults testResultsDto);
    }
}