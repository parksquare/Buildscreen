using System.Threading.Tasks;

namespace ParkSquare.BuildScreen.Core.Avatar
{
    public interface IAvatarService
    {
        Task<UserAvatar> GetAvatarAsync(AvatarId avatarId, ImageDimensions dimensions);
    }
}