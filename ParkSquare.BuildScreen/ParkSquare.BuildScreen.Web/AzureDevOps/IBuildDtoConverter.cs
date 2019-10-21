using ParkSquare.BuildScreen.Web.AzureDevOps.Dto;
using ParkSquare.BuildScreen.Web.Build;

namespace ParkSquare.BuildScreen.Web.AzureDevOps
{
    public interface IBuildDtoConverter
    {
        BuildTile Convert(BuildDto build, TestResults testResultsDto);
    }
}