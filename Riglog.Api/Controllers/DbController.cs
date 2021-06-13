using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Riglog.Api.Services.Interfaces;


namespace Riglog.Api.Controllers
{
    [ApiController]
    [Authorize]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class DbController : ControllerBase
    {
        private readonly IDbService _dbService;

        public DbController(IDbService dbService)
        {
            _dbService = dbService;
        }

        /// <summary>
        /// Check pending database migrations
        /// </summary>
        /// <response code="200">No pending migrations</response>
        /// <response code="202">Some pending migrations</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status202Accepted, Type = typeof(List<string>))]
        [HttpGet("check")]
        public async Task<IActionResult> MigrationsCheck()
        {
            var migrations = await _dbService.GetMigrationsAsync();

            return migrations.Any() ? Accepted(migrations) : Ok();
        }

        /// <summary>
        /// Apply database migrations
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPatch("update")]
        public async Task<IActionResult> Migrate()
        {
            await _dbService.MigrateAsync();
            return Ok();
        }
        
    }
}