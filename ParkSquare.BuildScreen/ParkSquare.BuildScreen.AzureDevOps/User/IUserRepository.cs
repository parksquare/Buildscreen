using System.Threading.Tasks;

namespace ParkSquare.BuildScreen.AzureDevOps.User
{
    public interface IUserRepository
    {
        Task<AzureUser> GetUserFromEmailAsync(string email);
    }
}