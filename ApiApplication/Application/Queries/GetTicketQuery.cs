using ApiApplication.Contracts;
using ApiApplication.Models.DTO;
using MediatR;

namespace ApiApplication.Application.Queries
{
    public class GetTicketQuery : IRequest<Result<TicketDTO, ValidationFailed>>
    {
        #region Public properties

        public Guid Id { get; set; }

        #endregion
    }
}