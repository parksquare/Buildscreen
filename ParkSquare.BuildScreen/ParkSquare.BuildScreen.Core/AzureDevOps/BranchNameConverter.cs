using System;

namespace ParkSquare.BuildScreen.Core.AzureDevOps
{
    public class BranchNameConverter : IBranchNameConverter
    {
        public string Convert(string branchName)
        {
            return ConvertPullRequest(StripRefsPrefix(branchName));
        }

        private static string StripRefsPrefix(string branchName)
        {
            return branchName.Replace("refs/heads/", string.Empty, StringComparison.InvariantCultureIgnoreCase);
        }

        private static string ConvertPullRequest(string branchName)
        {
            return branchName.Replace("refs/pull/", "PR ").Replace("/merge", string.Empty);
        }
    }
}