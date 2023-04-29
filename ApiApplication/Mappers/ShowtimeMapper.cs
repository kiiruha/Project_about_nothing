using ApiApplication.Database.Entities;
using ApiApplication.Models.DTO;
using Riok.Mapperly.Abstractions;

namespace ApiApplication.Mappers
{
    [Mapper(UseDeepCloning = true)]
    public static partial class ShowtimeMapper
    {
        #region Public methods

        public static ShowtimeDTO ToShowtimeDTO(this ShowtimeEntity showtimeEntity, AuditoriumEntity auditoriumEntity)
        {
            var showtimeDTO = showtimeEntity.ToShowtimeDTO();
            if (auditoriumEntity != null)
            {
                showtimeDTO.Auditorium = auditoriumEntity.ToAuditoriumDTO();
            }

            return showtimeDTO;
        }

        public static partial ShowtimeDTO ToShowtimeDTO(this ShowtimeEntity showtimeEntity);

        public static partial IEnumerable<ShowtimeDTO> ToShowtimeDTOs(this IEnumerable<ShowtimeEntity> showtimeEntities);

        #endregion

        #region Private methods

        private static MovieDTO MovieConvert(MovieEntity movieEntity) => movieEntity.ToMovieDTO();

        #endregion
    }
}