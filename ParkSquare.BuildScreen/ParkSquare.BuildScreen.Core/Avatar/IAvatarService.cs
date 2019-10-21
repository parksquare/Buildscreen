using System.Threading.Tasks;

namespace ParkSquare.BuildScreen.Core.Avatar
{
    public interface IAvatarService
    {
        Task<UserAvatar> GetAvatarAsync(string email, ImageDimensions dimensions);
    }
}