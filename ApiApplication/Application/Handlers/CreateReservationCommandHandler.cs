using ApiApplication.Application.Commands;
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
    public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, Result<TicketDTO, ValidationFailed>>
    {
        #region Private fields

        private readonly IShowtimesRepository _showtimesRepository;
        private readonly ITicketsRepository _ticketRepository;
        private readonly IAuditoriumsRepository _auditoriumsRepository;

        #endregion

        #region Public constructors

        public CreateReservationCommandHandler(IShowtimesRepository showtimesRepository, ITicketsRepository ticketRepository, IAuditoriumsRepository auditoriumsRepository)
        {
            _showtimesRepository = showtimesRepository;
            _ticketRepository = ticketRepository;
            _auditoriumsRepository = auditoriumsRepository;
        }

        #endregion

        #region Public methods

        public async Task<Result<TicketDTO, ValidationFailed>> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
        {
            var showtime = await _showtimesRepository.GetWithTicketsByIdAsync(request.ShowtimeId, cancellationToken);
            if (showtime == null)
            {
                var error = new ValidationFailure("ShowtimeId", "Showtime does not exist", request.ShowtimeId);
                return new ValidationFailed(error);
            }

            if (showtime.SessionDate <= DateTime.UtcNow.AddMinutes(10))
            {
                var error = new ValidationFailure("ShowtimeId", "Showtime was in the past", request.ShowtimeId);
                return new ValidationFailed(error);
            }

            var auditorium = await _auditoriumsRepository.GetAsync(showtime.AuditoriumId, cancellationToken);
            var seatsToReserve = request.Seats
                .Select(seatToReserve => auditorium.Seats
                    .FirstOrDefault(seat => seatToReserve.Row == seat.Row && seat.SeatNumber == seatToReserve.Number))
                .ToList();

            if (seatsToReserve.Any(seatToReserve => seatToReserve == null))
            {
                var error = new ValidationFailure("Seat", "One or many of seats do not exist in auditorium with current showtime");
                return new ValidationFailed(error);
            }

            var ticketEntity = await _ticketRepository.CreateAsync(showtime, seatsToReserve, cancellationToken);
            return ticketEntity.ToTicketDTO();
        }

        #endregion
    }
}