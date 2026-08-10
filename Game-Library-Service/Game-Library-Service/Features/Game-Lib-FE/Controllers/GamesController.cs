using Game_Library_Service.Common.Mediator.Interfaces;
using Game_Library_Service.Data.Enums;
using Game_Library_Service.Features.Game_Lib_FE.Logic;
using Microsoft.AspNetCore.Mvc;

namespace Game_Library_Service.Features.Game_Lib_FE.Controllers
{
    [ApiController]
    [Route("api/games")]
    public class GamesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GamesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets a paginated list of games (50 per page), optionally filtered by name, release year, genre, and publisher.
        /// </summary>
        [HttpGet]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<GetGames.Result>> GetGames(
            [FromQuery] string? name,
            [FromQuery] int? releaseYear,
            [FromQuery] Genre? genre,
            [FromQuery] int? publisherId,
            [FromQuery] int page,
            CancellationToken cancellationToken)
        {
            var query = new GetGames.Query
            {
                Name = name,
                ReleaseYear = releaseYear,
                Genre = genre,
                PublisherId = publisherId,
                Page = page < 1 ? 1 : page
            };

            var result = await _mediator.SendQueryAsync<GetGames.Query, GetGames.Result>(query, cancellationToken);

            return Ok(result);
        }
    }
}
