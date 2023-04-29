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
    public class TicketsController : ControllerBase
    {
        #region Private fields

        private readonly IMediator _mediator;
        private readonly ILogger<ShowtimesController> _logger;

        #endregion

        #region Public constructors

        public TicketsController(
            IMediator mediator,
            ILogger<ShowtimesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        #endregion

        #region Public methods

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(TicketDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationFailureResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetTicket(Guid id)
        {
            var query = new GetTicketQuery { Id = id };

            _logger.LogInformation("----- Sending query: ({@Query})", query);

            var result = await _mediator.Send(query);
            return result.Match<IActionResult>(
              dto => Ok(dto),
              failed => NotFound(failed.MapToResponse()));
        }

        [HttpPost("reserve")]
        [ProducesResponseType(typeof(TicketDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationFailureResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ReserveTicket(CreateReservationCommand createReservationCommand)
        {
            _logger.LogInformation("----- Sending command: ({@Command})", createReservationCommand);

            var result = await _mediator.Send(createReservationCommand);
            return result.Match<IActionResult>(
              dto => CreatedAtAction(nameof(GetTicket), new { id = dto.Id }, dto),
              failed => BadRequest(failed.MapToResponse()));
        }

        [HttpPost("{id:guid}/confirm")]
        [ProducesResponseType(typeof(TicketDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ValidationFailureResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ConfirmReservation(Guid id)
        {
            var command = new ConfirmReservationCommand { Id = id };

            _logger.LogInformation("----- Sending command: ({@Command})", command);

            var result = await _mediator.Send(command);
            return result.Match<IActionResult>(
              dto => Ok(dto),
              failed => BadRequest(failed.MapToResponse()));
        }

        #endregion
    }
}