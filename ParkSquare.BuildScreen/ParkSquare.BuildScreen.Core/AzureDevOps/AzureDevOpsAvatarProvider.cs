using System;
using System.Threading.Tasks;
using ParkSquare.BuildScreen.Core.Avatar;

namespace ParkSquare.BuildScreen.Core.AzureDevOps
{
    public class AzureDevOpsAvatarProvider : IAvatarProvider
    {
        public int Order => 2;

        public Task<UserAvatar> GetAvatarAsync(AvatarId avatarId, ImageDimensions dimensions)
        {
            throw new NotImplementedException();
        }
    }
}
