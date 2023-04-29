using ApiApplication.Application.Commands;
using ApiApplication.Contracts;
using ApiApplication.Database.Repositories.Abstractions;
using ApiApplication.Mappers;
using ApiApplication.Models.DTO;
using FluentValidation.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Application.Handlers
{
    public class ConfirmReservationCommandHandler : IRequestHandler<ConfirmReservationCommand, Result<TicketDTO, ValidationFailed>>
    {
        #region Private fields

        private readonly ITicketsRepository _ticketRepository;

        #endregion

        #region Public constructors

        public ConfirmReservationCommandHandler(ITicketsRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        #endregion

        #region Public methods

        public async Task<Result<TicketDTO, ValidationFailed>> Handle(ConfirmReservationCommand request, CancellationToken cancellationToken)
        {
            var ticket = await _ticketRepository.GetAsync(request.Id, cancellationToken);
            if (ticket == null)
            {
                var error = new ValidationFailure("Id", "Ticket does not exist", request.Id);
                return new ValidationFailed(error);
            }

            if (ticket.Paid)
            {
                var error = new ValidationFailure("Id", "Ticket was confirmed", request.Id);
                return new ValidationFailed(error);
            }

            ticket = await _ticketRepository.ConfirmPaymentAsync(ticket, cancellationToken);
            return ticket.ToTicketDTO();
        }

        #endregion
    }
}