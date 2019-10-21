using System.Net.Http;

namespace ParkSquare.BuildScreen.AzureDevOps.Build
{
    public interface IHttpClientFactory
    {
        HttpClient GetClientInstance();
    }
}