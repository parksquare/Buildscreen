using System;
using System.Collections.Generic;
using System.Linq;
using ParkSquare.BuildScreen.Core.AzureDevOps.Dto;

namespace ParkSquare.BuildScreen.Core.AzureDevOps
{
    public class LatestBuildsFilter : IBuildFilter
    {
        public IEnumerable<BuildDto> Filter(IEnumerable<BuildDto> builds)
        {
            var list = builds.ToList();
            var definitions = list.Select(GetBuildDefinitionKey).Distinct();
            var filtered = new List<BuildDto>(list.Where(IsQueued));

            foreach (var definition in definitions)
            {
                var latest =
                    list.Where(x => GetBuildDefinitionKey(x) == definition && !IsQueued(x))
                        .OrderByDescending(x => x.FinishTime)
                        .FirstOrDefault();

                if (latest != null) filtered.Add(latest);
            }

            return filtered;
        }

        private static string GetBuildDefinitionKey(BuildDto buildDto)
        {
            return buildDto.Repository.Type.Equals("TfsGit", StringComparison.InvariantCultureIgnoreCase)
                ? $"{buildDto.Definition.Name}|{buildDto.SourceBranch}"
                : buildDto.Definition.Name;
        }

        private static bool IsQueued(BuildDto buildDto)
        {
            return buildDto.Status.Equals("inProgress", StringComparison.InvariantCultureIgnoreCase) ||
                   buildDto.Status.Equals("notStarted", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}