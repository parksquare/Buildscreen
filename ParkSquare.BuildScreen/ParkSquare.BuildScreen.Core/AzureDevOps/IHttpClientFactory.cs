using System.Net.Http;

namespace ParkSquare.BuildScreen.Core.AzureDevOps
{
    public interface IHttpClientFactory
    {
        HttpClient GetClientInstance();
    }
}