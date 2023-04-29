using ApiApplication.Contracts;
using ApiApplication.Models.DTO;
using MediatR;

namespace ApiApplication.Application.Queries
{
    public class GetTicketsForShowtimeQuery : IRequest<Result<IEnumerable<TicketDTO>, ValidationFailed>>
    {
        #region Public properties

        public int ShowtimeId { get; set; }

        #endregion
    }
}