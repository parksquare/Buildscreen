using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ParkSquare.BuildScreen.Core.Avatar;
using ParkSquare.BuildScreen.Core.Imaging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;

namespace ParkSquare.BuildScreen.Gravatar
{
    public class GravatarProvider : IAvatarProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<IAvatarProvider> _logger;
        private readonly ImageFormatManager _imageFormatManager;
        private readonly IImageResizer _imageResizer;

        public int Order => 1;

        public GravatarProvider(
            IHttpClientFactory httpClientFactory, 
            ILogger<IAvatarProvider> logger, 
            ImageFormatManager imageFormatManager,
            IImageResizer imageResizer)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _imageFormatManager = imageFormatManager ?? throw new ArgumentNullException(nameof(imageFormatManager));
            _imageResizer = imageResizer ?? throw new ArgumentNullException(nameof(imageResizer));
        }

        public async Task<UserAvatar> GetAvatarAsync(string email, ImageDimensions dimensions)
        {
            try
            {
                var hash = GetHash(email.ToLower().Trim());
                var requestUri = new Uri($"https://www.gravatar.com/avatar/{hash}?s=400&d=404");

                var client = _httpClientFactory.CreateClient();

                using (var response = await client.GetAsync(requestUri))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsByteArrayAsync();
                        var contentType = response.Content.Headers.ContentType.MediaType;
                        
                        var type = _imageFormatManager.FindFormatByMimeType(contentType);

                        using (var image = Image.Load(data))
                        {
                            var resizedImage = _imageResizer.Resize(image, dimensions.Height, dimensions.Width);
                            var bytes = resizedImage.ConvertToByteArray(type);

                            return new UserAvatar
                            {
                                Data = bytes,
                                ContentType = contentType
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting Gravatar for '{email}': {ex.Message}");
            }

            return UserAvatar.NotAvailable;
        }

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
    }
}