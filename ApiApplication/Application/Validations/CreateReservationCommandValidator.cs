using ApiApplication.Application.Commands;
using ApiApplication.Database.Entities;
using ApiApplication.Database.Repositories.Abstractions;
using ApiApplication.Models.Request;
using FluentValidation.Results;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Application.Validations
{
    public class CreateReservationCommandValidator : AbstractValidator<CreateReservationCommand>
    {
        #region Private fields

        private readonly ITicketsRepository _ticketsRepository;

        #endregion

        #region Public constructors

        public CreateReservationCommandValidator(ILogger<CreateReservationCommandValidator> logger, ITicketsRepository ticketsRepository)
        {
            _ticketsRepository = ticketsRepository;

            RuleFor(command => command.ShowtimeId).GreaterThanOrEqualTo(0).WithMessage("ShowtimeId cannot be less then 0");
            RuleFor(command => command.Seats).CustomAsync(ValidateSeats);

            logger.LogTrace("----- INSTANCE WAS CREATED - {ClassName}", GetType().Name);
        }

        #endregion

        #region Private methods

        private static void ValidateIfAnotherTicketContainsThatSeats(TicketEntity ticket, IEnumerable<Seat> seats, ValidationContext<CreateReservationCommand> validationContext)
        {
            if (ticket == null)
            {
                return;
            }

            var usedSeats = ticket.Seats.Where(seat => seats.Any(seatToReserve => seatToReserve.Row == seat.Row && seatToReserve.Number == seat.SeatNumber));
            if (!usedSeats.Any())
            {
                return;
            }

            if (ticket.Paid)
            {
                validationContext.AddFailure(new ValidationFailure("Seats", "The seats were bought", usedSeats));
            }
            else if ((DateTime.UtcNow - ticket.CreatedTime.ToUniversalTime()).Minutes < 10)
            {
                validationContext.AddFailure(new ValidationFailure("Seats", $"The seats were reserved, try again in {10 - (DateTime.UtcNow - ticket.CreatedTime.ToUniversalTime()).Minutes} minutes", usedSeats));
            }
        }

        private async Task ValidateSeats(IEnumerable<Seat> seats, ValidationContext<CreateReservationCommand> validationContext, CancellationToken cancellationToken)
        {
            if (!seats.Any())
            {
                validationContext.AddFailure("Should be at least one seat");
            }

            var groupedSeats = seats.GroupBy(seat => seat.Row).ToList();
            if (groupedSeats.Count() != 1)
            {
                validationContext.AddFailure("All seats should be from one row");
            }

            var firstGroupedSeats = groupedSeats.First();
            if (firstGroupedSeats.GroupBy(seats => seats.Number).Count() != seats.Count())
            {
                validationContext.AddFailure("All seats should be unique");
            }

            var isSeatsNotContiguous = firstGroupedSeats
                .OrderBy(x => x.Number)
                .Select((seat, index) => seat.Number - index)
                .Distinct()
                .Skip(1)
                .Any();
            if (isSeatsNotContiguous)
            {
                validationContext.AddFailure("All seats should be contiguous");
            }

            var tickets = await _ticketsRepository.GetEnrichedAsync(validationContext.InstanceToValidate.ShowtimeId, cancellationToken);
            foreach (var ticket in tickets)
            {
                ValidateIfAnotherTicketContainsThatSeats(ticket, seats, validationContext);
            }
        }

        #endregion
    }
}