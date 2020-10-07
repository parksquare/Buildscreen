using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ParkSquare.BuildScreen.AzureDevOps.Build
{
    public class DisplayTransformer : IDisplayTransformer
    {
        private readonly IAzureDevOpsConfig _config;

        public DisplayTransformer(IAzureDevOpsConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public string Tranform(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            text = _config.HiddenWords.Aggregate(text, (current, word) => current.Replace(word, string.Empty));

            return text;
        }
    }
}