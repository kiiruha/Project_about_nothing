using ApiApplication.Database.Entities;
using ApiApplication.Models.DTO;
using Google.Protobuf.Collections;
using Riok.Mapperly.Abstractions;

namespace ApiApplication.Mappers
{
    [Mapper]
    public static partial class MovieMapper
    {
        #region Public methods

        [MapProperty(nameof(MovieEntity.Title), nameof(MovieDTO.Name))]
        public static partial MovieDTO ToMovieDTO(this MovieEntity movieEntity);

        public static MovieEntity ToMovieEntity(this MovieFromAPIDTO movie)
        {
            var movieEntity = movie.ToMovieEntityInternal();
            if (int.TryParse(movie.Year, out var year))
            {
                movieEntity.ReleaseDate = new DateTime(year, 1, 1);
            }
            return movieEntity;
        }

        public static partial MovieFromAPIDTO ToMovieFromAPIDTO(this showResponse showResponse);

        public static List<MovieFromAPIDTO> ToMovieFromAPIDTOs(this showListResponse showListResponse)
        {
            return ShowListResponseToMovieList(showListResponse.Shows);
        }

        #endregion

        #region Private methods

        [MapperIgnoreSource(nameof(MovieFromAPIDTO.Id))]
        [MapProperty(nameof(MovieFromAPIDTO.Crew), nameof(MovieEntity.Stars))]
        [MapProperty(nameof(MovieFromAPIDTO.ImDbRatingCount), nameof(MovieEntity.ImdbId))]
        private static partial MovieEntity ToMovieEntityInternal(this MovieFromAPIDTO movie);

        private static partial List<MovieFromAPIDTO> ShowListResponseToMovieList(RepeatedField<showResponse> showListResponse);

        #endregion
    }
}