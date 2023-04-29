using ApiApplication.Application.Commands;
using ApiApplication.Contracts;
using ApiApplication.Database.Entities;
using ApiApplication.Database.Repositories.Abstractions;
using ApiApplication.Mappers;
using ApiApplication.Models.DTO;
using ApiApplication.Services;
using FluentValidation.Results;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Application.Handlers
{
    public class CreateShowtimeCommandHandler : IRequestHandler<CreateShowtimeCommand, Result<ShowtimeDTO, ValidationFailed>>
    {
        #region Private fields

        private readonly IShowtimesRepository _showtimesRepository;
        private readonly IAuditoriumsRepository _auditoriumsRepository;
        private readonly IMovieService _movieService;

        #endregion

        #region Public constructors

        public CreateShowtimeCommandHandler(IShowtimesRepository showtimesRepository, IMovieService movieService, IAuditoriumsRepository auditoriumsRepository)
        {
            _showtimesRepository = showtimesRepository;
            _movieService = movieService;
            _auditoriumsRepository = auditoriumsRepository;
        }

        #endregion

        #region Public methods

        public async Task<Result<ShowtimeDTO, ValidationFailed>> Handle(CreateShowtimeCommand request, CancellationToken cancellationToken)
        {
            var movie = await _movieService.GetByIdAsync(request.MovieId);
            if (movie == null)
            {
                var error = new ValidationFailure("MovieId", "Movie does not exists", request.MovieId);
                return new ValidationFailed(error);
            }

            var auditorium = await _auditoriumsRepository.GetAsync(request.AuditoriumId, cancellationToken);
            if (auditorium == null)
            {
                var error = new ValidationFailure("AuditoriumId", "Auditorium does not exist", request.AuditoriumId);
                return new ValidationFailed(error);
            }

            var showtimes = await _showtimesRepository.GetAllAsync(entity => entity.AuditoriumId == request.AuditoriumId
            && Math.Abs((entity.SessionDate - request.SessionDate).TotalHours) < 2, cancellationToken);
            if (showtimes.Any())
            {
                var error = new ValidationFailure("AuditoriumId", "Movie within 2 hours in the same auditorium already exists", request.AuditoriumId);
                return new ValidationFailed(error);
            }

            var showtimeEntity = new ShowtimeEntity()
            {
                Movie = movie.ToMovieEntity(),
                AuditoriumId = request.AuditoriumId,
                SessionDate = request.SessionDate.ToUniversalTime()
            };

            var createdShowTime = await _showtimesRepository.CreateShowtime(showtimeEntity, cancellationToken);
            return createdShowTime.ToShowtimeDTO(auditorium);
        }

        #endregion
    }
}