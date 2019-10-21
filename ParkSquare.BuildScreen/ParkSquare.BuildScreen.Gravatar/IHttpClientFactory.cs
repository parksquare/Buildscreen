using System.Net.Http;

namespace ParkSquare.BuildScreen.Gravatar
{
    public interface IHttpClientFactory
    {
        HttpClient GetClientInstance();
    }
}