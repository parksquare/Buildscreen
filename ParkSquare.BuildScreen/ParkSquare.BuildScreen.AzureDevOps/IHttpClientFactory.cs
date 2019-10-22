using System.Net.Http;

namespace ParkSquare.BuildScreen.AzureDevOps
{
    public interface IHttpClientFactory
    {
        HttpClient GetJsonClient();

        HttpClient GetClient();
    }
}