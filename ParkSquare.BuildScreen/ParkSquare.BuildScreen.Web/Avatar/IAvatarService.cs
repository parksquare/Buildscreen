using System.Threading.Tasks;
using ParkSquare.BuildScreen.Web.Controllers;

namespace ParkSquare.BuildScreen.Web.Avatar
{
    public interface IAvatarService
    {
        Task<Web.Avatar.Avatar> GetAvatarAsync(AvatarId avatarId, ImageDimensions dimensions);
    }
}