namespace ApiApplication.Database.Entities
{
    public class TicketEntity
    {
        #region Public properties

        public Guid Id { get; set; }

        public int ShowtimeId { get; set; }

        public ICollection<SeatEntity> Seats { get; set; }

        public DateTime CreatedTime { get; set; }

        public bool Paid { get; set; }

        public ShowtimeEntity Showtime { get; set; }

        #endregion

        #region Public constructors

        public TicketEntity()
        {
            CreatedTime = DateTime.UtcNow;
            Paid = false;
        }

        #endregion
    }
}