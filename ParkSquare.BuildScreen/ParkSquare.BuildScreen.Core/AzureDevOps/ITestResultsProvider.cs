using System.Threading.Tasks;
using ParkSquare.BuildScreen.Core.Build;

namespace ParkSquare.BuildScreen.Core.AzureDevOps
{
    public interface ITestResultsProvider
    {
        Task<TestResults> GetTestsForBuildAsync(string project, string buildUri);
    }
}