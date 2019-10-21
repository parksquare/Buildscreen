using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ParkSquare.BuildScreen.Web.Avatar;

namespace ParkSquare.BuildScreen.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]

    public class AvatarController : ControllerBase
    {
        private readonly IAvatarService _avatarService;

        public AvatarController(IAvatarService avatarService)
        {
            _avatarService = avatarService ?? throw new ArgumentNullException(nameof(avatarService)); 
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

            var avatar = await _avatarService.GetAvatarAsync(avatarId, dimensions);

            var response = File(avatar.Data, avatar.ContentType);
            return response;
        }
    }
}
