using System.Net.Http;

namespace ParkSquare.BuildScreen.AzureDevOps
{
    public interface IAzureDevOpsClient
    {
        HttpClient GetClient();
    }
}