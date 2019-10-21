using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkSquare.BuildScreen.Core.Avatar
{
    public class AvatarService : IAvatarService
    {
        private readonly IEnumerable<IAvatarProvider> _avatarProviders;

        public AvatarService(IEnumerable<IAvatarProvider> avatarProviders)
        {
            if (avatarProviders == null) throw new ArgumentNullException(nameof(avatarProviders));

            _avatarProviders = avatarProviders.OrderBy(x => x.Order);
        }

        public Task<Avatar> GetAvatarAsync(AvatarId avatarId, ImageDimensions dimensions)
        {
            foreach (var provider in _avatarProviders)
            {
                var result = provider.GetAvatarAsync(avatarId, dimensions);
                if (result != null) return result;
            }

            throw new AvatarServiceException("No avatar provider returned a valid response");
        }
    }
}