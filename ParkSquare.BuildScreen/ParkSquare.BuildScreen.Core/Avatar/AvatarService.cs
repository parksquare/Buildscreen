using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ParkSquare.BuildScreen.Core.Avatar
{
    public class AvatarService : IAvatarService
    {
        private readonly ILogger<IAvatarService> _logger;
        private readonly IEnumerable<IAvatarProvider> _avatarProviders;

        public AvatarService(ILogger<IAvatarService> logger, IEnumerable<IAvatarProvider> avatarProviders)
        {
            if (avatarProviders == null) throw new ArgumentNullException(nameof(avatarProviders));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _avatarProviders = avatarProviders.OrderBy(x => x.Order);
        }

        public async Task<UserAvatar> GetAvatarAsync(string email, ImageDimensions dimensions)
        {
            foreach (var provider in _avatarProviders)
            {
                try
                {
                    var result = await provider.GetAvatarAsync(email, dimensions);

                    if (result != null && result != UserAvatar.NotAvailable)
                    {
                        return result;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError( $"Error calling avatar provider: {ex.Message}");
                }
            }

            throw new AvatarServiceException("No avatar provider returned a valid response");
        }
    }
}