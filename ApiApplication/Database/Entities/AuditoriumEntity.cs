namespace ApiApplication.Database.Entities
{
    public class AuditoriumEntity
    {
        #region Public properties

        public int Id { get; set; }
        public List<ShowtimeEntity> Showtimes { get; set; }
        public ICollection<SeatEntity> Seats { get; set; }

        #endregion
    }
}