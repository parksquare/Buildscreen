using SixLabors.ImageSharp;

namespace ParkSquare.BuildScreen.Core.Imaging
{
    public interface IImageResizer
    {
        Image Resize(Image image, int height, int width);
    }
}