using System.Collections.Generic;
using ParkSquare.BuildScreen.Web.AzureDevOps.Dto;

namespace ParkSquare.BuildScreen.Web.AzureDevOps
{
    public interface IBuildFilter
    {
        IEnumerable<BuildDto> Filter(IEnumerable<BuildDto> builds);
    }
}
