using ApiApplication.Database.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Database.Repositories.Abstractions
{
    public interface ITicketsRepository
    {
        #region Public methods

        Task<TicketEntity> ConfirmPaymentAsync(TicketEntity ticket, CancellationToken cancel);

        Task<TicketEntity> CreateAsync(ShowtimeEntity showtime, IEnumerable<SeatEntity> selectedSeats, CancellationToken cancel);

        Task<TicketEntity> GetAsync(Guid id, CancellationToken cancel);

        Task<IEnumerable<TicketEntity>> GetEnrichedAsync(int showtimeId, CancellationToken cancel);

        #endregion
    }
}