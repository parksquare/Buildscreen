using System.Threading.Tasks;

namespace ParkSquare.BuildScreen.Core.Avatar
{
    public interface IAvatarService
    {
        Task<Avatar> GetAvatarAsync(AvatarId avatarId, ImageDimensions dimensions);
    }
}