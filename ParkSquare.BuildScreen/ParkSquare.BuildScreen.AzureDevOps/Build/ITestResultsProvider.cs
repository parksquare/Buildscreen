using System.Threading.Tasks;
using ParkSquare.BuildScreen.Core.Build;

namespace ParkSquare.BuildScreen.AzureDevOps.Build
{
    public interface ITestResultsProvider
    {
        Task<TestResults> GetTestsForBuildAsync(string project, string buildUri);
    }
}