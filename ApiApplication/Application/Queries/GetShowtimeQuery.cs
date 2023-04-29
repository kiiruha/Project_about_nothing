using ApiApplication.Contracts;
using ApiApplication.Models.DTO;
using MediatR;

namespace ApiApplication.Application.Queries
{
    public class GetShowtimeQuery : IRequest<Result<ShowtimeDTO, ValidationFailed>>
    {
        #region Public properties

        public int Id { get; set; }

        public bool WithMovie { get; set; }

        public bool WithTickets { get; set; }

        public bool IncludeAuditorium { get; set; }

        #endregion
    }
}