using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkSquare.BuildScreen.Web.Build;
using ParkSquare.BuildScreen.Web.Models;

namespace ParkSquare.BuildScreen.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<BuildsController> _logger;
        private readonly IBuildService _buildService;

        public BuildsController(IMapper mapper, ILogger<BuildsController> logger, IBuildService buildService)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _buildService = buildService ?? throw new ArgumentNullException(nameof(buildService));
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<BuildInfoDto>>> GetAsync()
        {
            try
            {
                var builds = await _buildService.GetBuildsAsync();
                return Ok(_mapper.Map<IReadOnlyCollection<BuildInfoDto>>(builds));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in BuildsController");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{sinceHoursAgo}")]
        public async Task<ActionResult<IReadOnlyCollection<BuildInfoDto>>> GetAsync(int sinceHoursAgo)
        {
            try
            {
                var builds = await _buildService.GetBuildsAsync(sinceHoursAgo);
                return Ok(_mapper.Map<IReadOnlyCollection<BuildInfoDto>>(builds));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in BuildsController");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
