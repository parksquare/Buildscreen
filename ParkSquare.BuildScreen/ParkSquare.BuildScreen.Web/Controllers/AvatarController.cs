using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ParkSquare.BuildScreen.Web.Services;

namespace ParkSquare.BuildScreen.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class AvatarController : ControllerBase
    {
        private readonly IAvatarProvider _avatarProvider;

        public AvatarController(IAvatarProvider avatarProvider)
        {
            _avatarProvider = avatarProvider ?? throw new ArgumentNullException(nameof(avatarProvider));
        }

        [HttpGet("{id}/{uniqueName}")]
        public Task<IActionResult> GetAsync(string id, string uniqueName)
        {
            return GetAsync(id, uniqueName, 400, 400);
        }

        [HttpGet("{id}/{uniqueName}/{width}/{height}")]
        public async Task<IActionResult> GetAsync(string id, string uniqueName, int width, int height)
        {
            var avatarId = new AvatarId {Id = id, UniqueName = uniqueName};
            var dimensions = new ImageDimensions(width, height);

            var avatar = await _avatarProvider.GetAvatarAsync(avatarId, dimensions);

            var response = File(avatar.Data, avatar.ContentType);
            return response;
        }
    }
}
