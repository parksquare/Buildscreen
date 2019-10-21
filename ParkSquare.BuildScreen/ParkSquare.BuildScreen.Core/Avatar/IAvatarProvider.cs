using System.Threading.Tasks;

namespace ParkSquare.BuildScreen.Core.Avatar
{
    public interface IAvatarProvider
    {
        Task<UserAvatar> GetAvatarAsync(AvatarId avatarId, ImageDimensions dimensions);

        int Order { get; }
    }
}
