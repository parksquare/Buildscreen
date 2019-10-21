using System.Threading.Tasks;

namespace ParkSquare.BuildScreen.Core.Avatar
{
    public interface IAvatarProvider
    {
        Task<UserAvatar> GetAvatarAsync(string email, ImageDimensions dimensions);

        int Order { get; }
    }
}
