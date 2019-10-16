using System.Threading.Tasks;
using ParkSquare.BuildScreen.Web.Controllers;

namespace ParkSquare.BuildScreen.Web.Services
{
    public interface IAvatarProvider
    {
        Task<Avatar> GetAvatarAsync(AvatarId avatarId, ImageDimensions dimensions);
    }
}
