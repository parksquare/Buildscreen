using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ParkSquare.BuildScreen.AzureDevOps.User;
using ParkSquare.BuildScreen.Core.Avatar;

namespace ParkSquare.BuildScreen.AzureDevOps.Avatar
{
    public class AzureDevOpsAvatarProvider : IAvatarProvider
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<IAvatarProvider> _logger;
        private readonly IAzureDevOpsConfig _config;

        public AzureDevOpsAvatarProvider(IUserRepository userRepository, ILogger<IAvatarProvider> logger, IAzureDevOpsConfig config)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public int Order => 2;

        public async Task<UserAvatar> GetAvatarAsync(string email, ImageDimensions dimensions)
        {
            try
            {
                var user = await _userRepository.GetUserFromEmailAsync(email);
                if (user == null) return UserAvatar.NotAvailable;

                var requestUri = new Uri(_config.ApiBaseUrl,
                    $"/_apis/GraphProfile/MemberAvatars/{user.Descriptor}?size=2");

                // get this image
                // resize it (if necessary)
                // maybe filter out PNG ones which are just initials

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting Azure Dev Ops users list or user avatar: {ex.Message}");
            }

            throw new NotImplementedException();
        }
    }
}
