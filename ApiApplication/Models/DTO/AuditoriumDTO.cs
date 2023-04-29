namespace ApiApplication.Models.DTO
{
    public class AuditoriumDTO
    {
        #region Public properties

        public int Id { get; set; }

        public List<SeatDTO> Seats { get; set; }

        #endregion
    }
}