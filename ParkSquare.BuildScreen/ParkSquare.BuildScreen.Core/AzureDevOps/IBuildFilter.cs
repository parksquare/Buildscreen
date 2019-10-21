using System.Collections.Generic;
using ParkSquare.BuildScreen.Core.AzureDevOps.Dto;

namespace ParkSquare.BuildScreen.Core.AzureDevOps
{
    public interface IBuildFilter
    {
        IEnumerable<BuildDto> Filter(IEnumerable<BuildDto> builds);
    }
}
