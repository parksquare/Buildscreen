using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParkSquare.BuildScreen.Web.Build
{
    public interface IBuildService
    {
        Task<IReadOnlyCollection<Web.Build.Build>> GetBuildsAsync();

        Task<IReadOnlyCollection<Web.Build.Build>> GetBuildsAsync(int sinceHours);

    }
}