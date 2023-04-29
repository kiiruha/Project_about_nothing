using ApiApplication.Database.Entities;
using ApiApplication.Models.DTO;
using Riok.Mapperly.Abstractions;

namespace ApiApplication.Mappers
{
    [Mapper(UseDeepCloning = true)]
    public static partial class TicketMapper
    {
        #region Public methods

        public static partial TicketDTO ToTicketDTO(this TicketEntity ticketEntity);

        public static partial IEnumerable<TicketDTO> ToTicketDTOs(this IEnumerable<TicketEntity> ticketEntities);

        #endregion

        #region Private methods

        private static Guid MapGuid(Guid id) => id;

        #endregion
    }
}