using ApiApplication.Application.Queries;
using ApiApplication.Contracts;
using ApiApplication.Database.Repositories.Abstractions;
using ApiApplication.Mappers;
using ApiApplication.Models.DTO;
using FluentValidation.Results;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Application.Handlers
{
    public class GetTicketsForShowtimeQueryHandler : IRequestHandler<GetTicketsForShowtimeQuery, Result<IEnumerable<TicketDTO>, ValidationFailed>>
    {
        #region Private fields

        private readonly ITicketsRepository _ticketsRepository;

        #endregion

        #region Public constructors

        public GetTicketsForShowtimeQueryHandler(ITicketsRepository ticketsRepository)
        {
            _ticketsRepository = ticketsRepository;
        }

        #endregion

        #region Public methods

        public async Task<Result<IEnumerable<TicketDTO>, ValidationFailed>> Handle(GetTicketsForShowtimeQuery request, CancellationToken cancellationToken)
        {
            var notFoundError = new ValidationFailure("ShowtimeId", "Tickets for that showtime do not exists", request.ShowtimeId);

            var tickets = await _ticketsRepository.GetEnrichedAsync(request.ShowtimeId, cancellationToken);
            if (!tickets.Any())
            {
                return new ValidationFailed(notFoundError);
            }

            //ToList is necessary here to be able implicit cast the list of DTOs to the Result type, see Section 6.4.1 Permitted user-defined conversions
            return tickets.ToTicketDTOs().ToList();
        }

        #endregion
    }
}