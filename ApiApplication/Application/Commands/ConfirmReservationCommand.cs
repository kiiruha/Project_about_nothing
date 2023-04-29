using ApiApplication.Contracts;
using ApiApplication.Models.DTO;
using MediatR;

namespace ApiApplication.Application.Commands
{
    public class ConfirmReservationCommand : IRequest<Result<TicketDTO, ValidationFailed>>
    {
        #region Public properties

        public Guid Id { get; set; }

        #endregion
    }
}