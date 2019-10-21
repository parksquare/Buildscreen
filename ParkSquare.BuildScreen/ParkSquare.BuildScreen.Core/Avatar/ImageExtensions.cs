using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats;

namespace ParkSquare.BuildScreen.Core.Avatar
{
    public static class ImageExtensions
    {
        public static byte[] ConvertToByteArray(this Image image, IImageFormat imageFormat)
        {
            using (var memoryStream = new MemoryStream())
            {
                var imageEncoder = image.GetConfiguration().ImageFormatsManager.FindEncoder(imageFormat);
                image.Save(memoryStream, imageEncoder);
                return memoryStream.ToArray();
            }
        }
    }
}
