using ApiApplication.Models.DTO;
using MediatR;

namespace ApiApplication.Application.Queries
{
    public class GetShowtimesQuery : IRequest<IEnumerable<ShowtimeDTO>>
    {
    }
}