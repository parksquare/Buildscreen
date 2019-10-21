using System.Net.Http;

namespace ParkSquare.BuildScreen.Web.AzureDevOps
{
    public interface IHttpClientFactory
    {
        HttpClient GetClientInstance();
    }
}