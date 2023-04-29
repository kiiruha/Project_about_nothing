using ApiApplication.Database.Entities;
using ApiApplication.Models.DTO;
using Riok.Mapperly.Abstractions;

namespace ApiApplication.Mappers
{
    [Mapper]
    public static partial class SeatMapper
    {
        #region Public methods

        public static partial SeatDTO ToSeatDTO(this SeatEntity seatEntity);

        public static partial IEnumerable<SeatDTO> ToSeatDTOs(this IEnumerable<SeatEntity> seatEntities);

        #endregion
    }
}