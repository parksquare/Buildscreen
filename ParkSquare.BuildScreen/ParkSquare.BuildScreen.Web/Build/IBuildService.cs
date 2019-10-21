using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParkSquare.BuildScreen.Web.Build
{
    public interface IBuildService
    {
        Task<IReadOnlyCollection<BuildTile>> GetBuildsAsync();

        Task<IReadOnlyCollection<BuildTile>> GetBuildsAsync(int sinceHours);

    }
}