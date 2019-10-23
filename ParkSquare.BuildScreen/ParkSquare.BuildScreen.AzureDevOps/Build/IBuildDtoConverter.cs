using ParkSquare.BuildScreen.AzureDevOps.Build.Dto;
using ParkSquare.BuildScreen.Core.Build;

namespace ParkSquare.BuildScreen.AzureDevOps.Build
{
    public interface IBuildDtoConverter
    {
        BuildTile Convert(BuildDto buildDto, TestResults testResultsDto);
    }
}