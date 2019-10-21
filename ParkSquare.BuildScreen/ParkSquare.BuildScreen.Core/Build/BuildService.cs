using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkSquare.BuildScreen.Core.Build
{
    public class BuildService : IBuildService
    {
        private readonly IEnumerable<IBuildProvider> _buildProviders;

        public BuildService(IEnumerable<IBuildProvider> buildProviders)
        {
            _buildProviders = buildProviders ?? throw new ArgumentNullException(nameof(buildProviders));
        }

        public async Task<IReadOnlyCollection<BuildTile>> GetBuildsAsync()
        {
            var tasks = _buildProviders.Select(x => x.GetBuildsAsync()).ToList();
            await Task.WhenAll(tasks);

            var results = tasks.SelectMany(x => x.Result);
            return results.ToList();
        }

        public async Task<IReadOnlyCollection<BuildTile>> GetBuildsAsync(int sinceHours)
        {
            var tasks = _buildProviders.Select(x => x.GetBuildsAsync(sinceHours)).ToList();
            await Task.WhenAll(tasks);

            var results = tasks.SelectMany(x => x.Result);
            return results.ToList();
        }
    }
}
