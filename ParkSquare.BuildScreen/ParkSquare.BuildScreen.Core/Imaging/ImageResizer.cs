using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace ParkSquare.BuildScreen.Core.Imaging
{
    public class ImageResizer : IImageResizer
    {
        public Image Resize(Image image, int height, int width)
        {
            // This bug in ImageSharp causes JPG images to be pink and weird in certain
            // circumstances, e.g. if Gravatar is jpg. Workaround is to use PNG instead.
            // https://github.com/SixLabors/ImageSharp/issues/871
            // Fixed in their MyGet repo but not the Nuget prerelease yet.

            if (image.Height == height && image.Width == width)
            {
                return image;
            }

            var xScaleFactor = (double) width / image.Width;
            var yScaleFactor = (double) height / image.Height;

            if (xScaleFactor.Equals(yScaleFactor))
            {
                return image.Clone(x => x.Resize(width, height));
            }
            
            return xScaleFactor > yScaleFactor
                ? PadToSize(image, height, (int) (image.Width * yScaleFactor), height, width)
                : PadToSize(image, (int) (image.Height * xScaleFactor), width, height, width);
        }

        private static Image PadToSize(Image image, int scaledHeight, int scaledWidth, int height, int width)
        {
            var resizedImage = image.Clone(x => x.Resize(scaledWidth, scaledHeight));
            resizedImage.Mutate(x => x.Pad(width, height, Color.Transparent));

            return resizedImage;
        }
    }
}
