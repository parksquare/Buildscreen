using System.Net.Http;

namespace ParkSquare.BuildScreen.Web.Services
{
    public interface IHttpClientFactory
    {
        HttpClient GetClientInstance();
    }
}