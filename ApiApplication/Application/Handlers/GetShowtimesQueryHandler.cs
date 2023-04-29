using ApiApplication.Application.Queries;
using ApiApplication.Database.Repositories.Abstractions;
using ApiApplication.Mappers;
using ApiApplication.Models.DTO;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Application.Handlers
{
    public class GetShowtimesQueryHandler : IRequestHandler<GetShowtimesQuery, IEnumerable<ShowtimeDTO>>
    {
        #region Private fields

        private readonly IShowtimesRepository _showtimesRepository;

        #endregion

        #region Public constructors

        public GetShowtimesQueryHandler(IShowtimesRepository showtimesRepository)
        {
            _showtimesRepository = showtimesRepository;
        }

        #endregion

        #region Public methods

        public async Task<IEnumerable<ShowtimeDTO>> Handle(GetShowtimesQuery _, CancellationToken cancellationToken)
        {
            var showtimes = await _showtimesRepository.GetAllAsync((_) => true, cancellationToken);
            return showtimes.ToShowtimeDTOs();
        }

        #endregion
    }
}