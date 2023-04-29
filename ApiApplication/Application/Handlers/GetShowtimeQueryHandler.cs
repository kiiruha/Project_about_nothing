using ApiApplication.Application.Queries;
using ApiApplication.Contracts;
using ApiApplication.Database.Entities;
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
    public class GetShowtimeQueryHandler : IRequestHandler<GetShowtimeQuery, Result<ShowtimeDTO, ValidationFailed>>
    {
        #region Private fields

        private readonly IShowtimesRepository _showtimesRepository;
        private readonly IAuditoriumsRepository _auditoriumsRepository;

        #endregion

        #region Public constructors

        public GetShowtimeQueryHandler(IShowtimesRepository showtimesRepository, IAuditoriumsRepository auditoriumsRepository)
        {
            _showtimesRepository = showtimesRepository;
            _auditoriumsRepository = auditoriumsRepository;
        }

        #endregion

        #region Public methods

        public async Task<Result<ShowtimeDTO, ValidationFailed>> Handle(GetShowtimeQuery request, CancellationToken cancellationToken)
        {
            var notFoundError = new ValidationFailure("Id", "Showtime does not exists", request.Id);

            ShowtimeEntity showtime = null;
            if (request.WithMovie)
            {
                var showtimes = await _showtimesRepository.GetAllAsync(entity => entity.Id == request.Id, cancellationToken);
                if (!showtimes.Any())
                {
                    return new ValidationFailed(notFoundError);
                }
                showtime = showtimes.First();
            }

            if (request.WithTickets)
            {
                var showtimeWithTickets = await _showtimesRepository.GetWithTicketsByIdAsync(request.Id, cancellationToken);
                if (showtime == null)
                {
                    showtime = showtimeWithTickets;
                }
                else
                {
                    showtime.Tickets = showtimeWithTickets.Tickets;
                }
            }

            if (showtime == null)
            {
                return new ValidationFailed(notFoundError);
            }

            return request.IncludeAuditorium
                ? await PopulateShowtimeWithAuditorium(showtime, cancellationToken)
                : showtime.ToShowtimeDTO();
        }

        public async Task<Result<ShowtimeDTO, ValidationFailed>> PopulateShowtimeWithAuditorium(ShowtimeEntity showtime, CancellationToken cancellationToken)
        {
            var auditorium = await _auditoriumsRepository.GetAsync(showtime.AuditoriumId, cancellationToken);
            if (auditorium == null)
            {
                var error = new ValidationFailure("AuditoriumId", "Auditorium does not exist", showtime.AuditoriumId);
                return new ValidationFailed(error);
            }

            return showtime.ToShowtimeDTO(auditorium);
        }

        #endregion
    }
}