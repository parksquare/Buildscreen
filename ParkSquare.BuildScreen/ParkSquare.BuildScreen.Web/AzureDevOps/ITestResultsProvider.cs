using System.Threading.Tasks;
using ParkSquare.BuildScreen.Web.Build;

namespace ParkSquare.BuildScreen.Web.AzureDevOps
{
    public interface ITestResultsProvider
    {
        Task<TestResults> GetTestsForBuildAsync(string project, string buildUri);
    }
}