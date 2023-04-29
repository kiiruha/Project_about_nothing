using ApiApplication.Application.Commands;
using ApiApplication.Application.Queries;
using ApiApplication.Contracts;
using ApiApplication.Mappers;
using ApiApplication.Models.DTO;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace ApiApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowtimesController : ControllerBase
    {
        #region Private fields

        private readonly IMediator _mediator;
        private readonly ILogger<ShowtimesController> _logger;

        #endregion

        #region Public constructors

        public ShowtimesController(
            IMediator mediator,
            ILogger<ShowtimesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        #endregion

        #region Public methods

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ShowtimeDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetShowtimes()
        {
            var query = new GetShowtimesQuery();

            _logger.LogInformation("----- Sending query: ({@Query})", query);

            var result = await _mediator.Send(query);
            return result is not null
                ? Ok(result)
                : NotFound();
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ShowtimeDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationFailureResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetShowtime(int id, [FromQuery] bool includeTickets, [FromQuery] bool includeMovies, [FromQuery] bool includeAuditorium)
        {
            var query = new GetShowtimeQuery { Id = id, WithMovie = includeMovies, WithTickets = includeTickets, IncludeAuditorium = includeAuditorium };

            _logger.LogInformation("----- Sending query: ({@Query})", query);

            var result = await _mediator.Send(query);
            return result.Match<IActionResult>(
                dto => Ok(dto),
                failed => NotFound(failed.MapToResponse()));
        }

        [HttpGet("{id:int}/tickets")]
        [ProducesResponseType(typeof(IEnumerable<TicketDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationFailureResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetTicketsForShowtime(int id)
        {
            var query = new GetTicketsForShowtimeQuery { ShowtimeId = id };

            _logger.LogInformation("----- Sending query: ({@Query})", query);

            var result = await _mediator.Send(query);
            return result.Match<IActionResult>(
              dto => Ok(dto),
              failed => NotFound(failed.MapToResponse()));
        }

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<ShowtimeDTO>), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationFailureResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateShowtime([FromBody] CreateShowtimeCommand createShowtime)
        {
            _logger.LogInformation("----- Sending command: ({@Command})", createShowtime);

            var result = await _mediator.Send(createShowtime);
            return result.Match<IActionResult>(
                dto => CreatedAtAction(nameof(GetShowtime), new { id = dto.Id }, dto),
                failed => BadRequest(failed.MapToResponse()));
        }

        #endregion
    }
}