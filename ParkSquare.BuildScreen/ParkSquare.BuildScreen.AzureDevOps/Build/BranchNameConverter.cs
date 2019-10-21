using System.Text.RegularExpressions;

namespace ParkSquare.BuildScreen.AzureDevOps.Build
{
    public class BranchNameConverter : IBranchNameConverter
    {
        public string Convert(string branchName)
        {
            return ConvertPullRequest(StripRefsPrefix(branchName));
        }

        private static string StripRefsPrefix(string branchName)
        {
            return CaseInsensitiveReplace(branchName, "refs/heads/", string.Empty);
        }

        private static string ConvertPullRequest(string branchName)
        {
            return branchName.Replace("refs/pull/", "PR ").Replace("/merge", string.Empty);
        }

        private static string CaseInsensitiveReplace(string original, string find, string replace)
        {
            return Regex.Replace(original, Regex.Escape(find), replace.Replace("$", "$$"), RegexOptions.IgnoreCase);
        }
    }
}