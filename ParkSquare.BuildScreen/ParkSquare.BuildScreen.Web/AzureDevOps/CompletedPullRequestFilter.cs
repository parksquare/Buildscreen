using System;
using System.Collections.Generic;
using System.Linq;
using ParkSquare.BuildScreen.Web.AzureDevOps.Dto;

namespace ParkSquare.BuildScreen.Web.AzureDevOps
{
    public class CompletedPullRequestFilter : IBuildFilter
    {
        public IEnumerable<BuildDto> Filter(IEnumerable<BuildDto> builds)
        {
            return builds.Where(x =>
                !(x.Reason.Equals("pullRequest", StringComparison.InvariantCultureIgnoreCase) &&
                  x.Status.Equals("completed", StringComparison.InvariantCultureIgnoreCase) && 
                  x.Result.Equals("succeeded", StringComparison.InvariantCultureIgnoreCase)));
        }
    }
}
