using System.Threading.Tasks;
using ParkSquare.BuildScreen.Web.Controllers;

namespace ParkSquare.BuildScreen.Web.Avatar
{
    public interface IAvatarProvider
    {
        Task<Web.Avatar.Avatar> GetAvatarAsync(AvatarId avatarId, ImageDimensions dimensions);

        int Order { get; }
    }
}
