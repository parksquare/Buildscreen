using ParkSquare.BuildScreen.Core.AzureDevOps.Dto;
using ParkSquare.BuildScreen.Core.Build;

namespace ParkSquare.BuildScreen.Core.AzureDevOps
{
    public interface IBuildDtoConverter
    {
        BuildTile Convert(BuildDto build, TestResults testResultsDto);
    }
}