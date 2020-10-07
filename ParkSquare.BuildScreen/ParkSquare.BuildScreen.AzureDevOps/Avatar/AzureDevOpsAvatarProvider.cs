﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ParkSquare.BuildScreen.AzureDevOps.User;
using ParkSquare.BuildScreen.Core.Avatar;
using ParkSquare.BuildScreen.Core.Imaging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;

namespace ParkSquare.BuildScreen.AzureDevOps.Avatar
{
    public class AzureDevOpsAvatarProvider : IAvatarProvider
    {
        private readonly IAzureDevOpsClient _client;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<IAvatarProvider> _logger;
        private readonly ImageFormatManager _imageFormatManager;
        private readonly IImageResizer _imageResizer;

        public AzureDevOpsAvatarProvider(
            IAzureDevOpsClient client,
            IUserRepository userRepository, 
            ILogger<IAvatarProvider> logger, 
            ImageFormatManager imageFormatManager,
            IImageResizer imageResizer)
        {
            _client = client;
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _imageFormatManager = imageFormatManager ?? throw new ArgumentNullException(nameof(imageFormatManager));
            _imageResizer = imageResizer ?? throw new ArgumentNullException(nameof(imageResizer));
        }

        public int Order => 2;

        public async Task<UserAvatar> GetAvatarAsync(string email, ImageDimensions dimensions)
        {
            try
            {
                var user = await _userRepository.GetUserFromEmailAsync(email);
                if (user == null) return UserAvatar.NotAvailable;

                var requestUri = $"_apis/GraphProfile/MemberAvatars/{user.Descriptor}?size=2";

                var client = _client.GetClient();

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
                _logger.LogError($"Error getting Azure Dev Ops users list or user avatar: {ex.Message}");
            }

            throw new NotImplementedException();
        }
    }
}
