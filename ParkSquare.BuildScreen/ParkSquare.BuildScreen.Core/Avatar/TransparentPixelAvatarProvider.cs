using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;

namespace ParkSquare.BuildScreen.Core.Avatar
{
    public class TransparentPixelAvatarProvider : IAvatarProvider
    {
        public int Order => int.MaxValue;

        public Task<UserAvatar> GetAvatarAsync(AvatarId avatarId, ImageDimensions dimensions)
        {
            return Task.FromResult(CreateAvatar());
        }

        private static UserAvatar CreateAvatar()
        {
            return new UserAvatar
            {
                Data = CreateTransparentPixel(), 
                ContentType = PngFormat.Instance.DefaultMimeType
            };
        }

        private static byte[] CreateTransparentPixel()
        {
            using (var image = new Image<Rgba32>(1, 1))
            {
                return image.ConvertToByteArray(PngFormat.Instance);
            }
        }
    }
}
