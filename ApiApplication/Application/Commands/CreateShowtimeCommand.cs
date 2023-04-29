using ApiApplication.Contracts;
using ApiApplication.Models.DTO;
using MediatR;

namespace ApiApplication.Application.Commands
{
    public class CreateShowtimeCommand : IRequest<Result<ShowtimeDTO, ValidationFailed>>
    {
        #region Public properties

        public string MovieId { get; set; }

        public DateTime SessionDate { get; set; }

        public int AuditoriumId { get; set; }

        #endregion
    }
}