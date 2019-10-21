using System;
using System.Threading.Tasks;
using ParkSquare.BuildScreen.Core.Avatar;

namespace ParkSquare.BuildScreen.AzureDevOps.Avatar
{
    public class AzureDevOpsAvatarProvider : IAvatarProvider
    {
        public int Order => 2;

        public Task<UserAvatar> GetAvatarAsync(string email, ImageDimensions dimensions)
        {
            throw new NotImplementedException();
        }
    }
}
