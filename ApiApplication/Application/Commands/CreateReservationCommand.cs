using ApiApplication.Contracts;
using ApiApplication.Models.DTO;
using ApiApplication.Models.Request;
using MediatR;

namespace ApiApplication.Application.Commands
{
    public class CreateReservationCommand : IRequest<Result<TicketDTO, ValidationFailed>>
    {
        #region Public properties

        public int ShowtimeId { get; set; }

        public IEnumerable<Seat> Seats { get; set; }

        #endregion
    }
}