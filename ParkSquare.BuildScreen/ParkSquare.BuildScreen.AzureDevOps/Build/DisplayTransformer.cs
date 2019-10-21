using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ParkSquare.BuildScreen.AzureDevOps.Build
{
    public class DisplayTransformer : IDisplayTransformer
    {
        private readonly IAzureDevOpsConfig _config;
        private readonly Regex _regex;

        public DisplayTransformer(IAzureDevOpsConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _regex = new Regex("[^a-zA-Z0-9]", RegexOptions.Compiled);
        }

        public string Tranform(string text)
        {
            text = _config.HiddenWords.Aggregate(text, (current, word) => current.Replace(word, string.Empty));

            return _regex.Replace(text, " ");
        }
    }
}