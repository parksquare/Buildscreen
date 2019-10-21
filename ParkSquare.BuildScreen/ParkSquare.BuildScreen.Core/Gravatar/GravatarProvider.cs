using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ParkSquare.BuildScreen.Core.Avatar;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace ParkSquare.BuildScreen.Core.Gravatar
{
    public class GravatarProvider : IAvatarProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<IAvatarProvider> _logger;
        private readonly ImageFormatManager _imageFormatManager;

        public GravatarProvider(IHttpClientFactory httpClientFactory, ILogger<IAvatarProvider> logger, ImageFormatManager imageFormatManager)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _imageFormatManager = imageFormatManager ?? throw new ArgumentNullException(nameof(imageFormatManager));
        }

        public async Task<UserAvatar> GetAvatarAsync(AvatarId avatarId, ImageDimensions dimensions)
        {
            try
            {
                var hash = GetHash(avatarId.UniqueName.ToLower().Trim());
                var requestUri = new Uri($"https://www.gravatar.com/avatar/{hash}?s=400&d=404");
                
                var client = _httpClientFactory.GetClientInstance();

                using (var response = await client.GetAsync(requestUri))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsByteArrayAsync();
                        var contentType = response.Content.Headers.ContentType.MediaType;
                        
                        var type = _imageFormatManager.FindFormatByMimeType(contentType);

                        using (var image = Image.Load(data))
                        {
                            if (image.Height == dimensions.Height && image.Width == dimensions.Width)
                            {
                                return new UserAvatar
                                {
                                    Data = data,
                                    ContentType = contentType
                                };
                            }

                            image.Mutate(x => x.Resize(dimensions.Width, dimensions.Height));
                            var resized = ConvertToByteArray(image, type);

                            return new UserAvatar
                            {
                                Data = resized,
                                ContentType = contentType
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting Gravatar for '{avatarId.Id}' '{avatarId.UniqueName}': {ex.Message}");
            }

            return new UserAvatar {Data = CreateTransparentPixel(), ContentType = PngFormat.Instance.DefaultMimeType};
        }

        public int Order => 1;

        private static string GetHash(string emailAddress)
        {
            using (var md5Hash = MD5.Create())
            {
                var data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(emailAddress));
                var sBuilder = new StringBuilder();

                foreach (var t in data)
                {
                    sBuilder.Append(t.ToString("x2"));
                }

                return sBuilder.ToString();
            }
        }

        private static byte[] ConvertToByteArray(Image image, IImageFormat imageFormat)
        {
            using (var memoryStream = new MemoryStream())
            {
                var imageEncoder = image.GetConfiguration().ImageFormatsManager.FindEncoder(imageFormat);
                image.Save(memoryStream, imageEncoder);
                return memoryStream.ToArray();
            }
        }

        private static byte[] CreateTransparentPixel()
        {
            using (var image = new Image<Rgba32>(1, 1))
            {
                return ConvertToByteArray(image, PngFormat.Instance);
            }
        }
    }
}