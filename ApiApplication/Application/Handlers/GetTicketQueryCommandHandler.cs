using ApiApplication.Application.Queries;
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
    public class GetTicketQueryCommandHandler : IRequestHandler<GetTicketQuery, Result<TicketDTO, ValidationFailed>>
    {
        #region Private fields

        private readonly ITicketsRepository _ticketsRepository;

        #endregion

        #region Public constructors

        public GetTicketQueryCommandHandler(ITicketsRepository ticketsRepository)
        {
            _ticketsRepository = ticketsRepository;
        }

        #endregion

        #region Public methods

        public async Task<Result<TicketDTO, ValidationFailed>> Handle(GetTicketQuery request, CancellationToken cancellationToken)
        {
            var ticket = await _ticketsRepository.GetAsync(request.Id, cancellationToken);
            if (ticket == null)
            {
                var error = new ValidationFailure("Id", "Ticket does not exist", request.Id);
                return new ValidationFailed(error);
            }

            return ticket.ToTicketDTO();
        }

        #endregion
    }
}