using System.Net.Http;

namespace ParkSquare.BuildScreen.Core.Gravatar
{
    public interface IHttpClientFactory
    {
        HttpClient GetClientInstance();
    }
}