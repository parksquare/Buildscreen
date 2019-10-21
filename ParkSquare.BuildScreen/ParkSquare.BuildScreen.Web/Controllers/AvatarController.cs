using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ParkSquare.BuildScreen.Core.Avatar;

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

        [HttpGet("{email}")]
        public Task<IActionResult> GetAsync(string email)
        {
            return GetAsync(email, 400, 400);
        }

        [HttpGet("{email}/{width}/{height}")]
        public async Task<IActionResult> GetAsync(string email, int width, int height)
        {
            try
            {
                var dimensions = new ImageDimensions(width, height);

                var avatar = await _avatarService.GetAvatarAsync(email, dimensions);

                var response = File(avatar.Data, avatar.ContentType);
                return response;
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}
