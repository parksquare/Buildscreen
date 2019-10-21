using System.Collections.Generic;
using ParkSquare.BuildScreen.AzureDevOps.Build.Dto;

namespace ParkSquare.BuildScreen.AzureDevOps.Build
{
    public interface IBuildFilter
    {
        IEnumerable<BuildDto> Filter(IEnumerable<BuildDto> builds);
    }
}
