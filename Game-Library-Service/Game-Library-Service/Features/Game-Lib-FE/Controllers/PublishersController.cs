using Game_Library_Service.Common.Mediator.Interfaces;
using Game_Library_Service.Features.Game_Lib_FE.Logic;
using Microsoft.AspNetCore.Mvc;

namespace Game_Library_Service.Features.Game_Lib_FE.Controllers
{
    [ApiController]
    [Route("api/publishers")]
    public class PublishersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PublishersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets a paginated list of publishers (50 per page), optionally filtered by name.
        /// </summary>
        [HttpGet]
        [ApiExplorerSettings(GroupName = "v1")]
        public async Task<ActionResult<GetPublishers.Result>> GetPublishers(
            [FromQuery] string? name,
            [FromQuery] int page,
            CancellationToken cancellationToken)
        {
            var query = new GetPublishers.Query
            {
                Name = name,
                Page = page < 1 ? 1 : page
            };

            var result = await _mediator.SendQueryAsync<GetPublishers.Query, GetPublishers.Result>(query, cancellationToken);

            return Ok(result);
        }
    }
}
