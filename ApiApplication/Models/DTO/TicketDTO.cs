using Newtonsoft.Json;

namespace ApiApplication.Models.DTO
{
    public class TicketDTO
    {
        #region Public properties

        public Guid Id { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ShowtimeWithoutTicketsDTO Showtime { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<SeatDTO> Seats { get; set; }

        public DateTime CreatedTime { get; set; }

        public bool Paid { get; set; }

        #endregion
    }
}