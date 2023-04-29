using ApiApplication.Database.Entities;
using ApiApplication.Models.DTO;
using Riok.Mapperly.Abstractions;

namespace ApiApplication.Mappers
{
    [Mapper(UseDeepCloning = true)]
    public static partial class AuditoriumMapper
    {
        #region Public methods

        public static partial AuditoriumDTO ToAuditoriumDTO(this AuditoriumEntity showtimeEntity);

        #endregion
    }
}