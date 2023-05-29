﻿namespace ApiApplication.Database.Entities
{
    public class MovieEntity
    {
        #region Public properties

        public int Id { get; set; }
        public string Title { get; set; }
        public string ImdbId { get; set; }
        public string Stars { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<ShowtimeEntity> Showtimes { get; set; }

        #endregion
    }
}