using Newtonsoft.Json;

namespace ApiApplication.Models.DTO
{
    public class ShowtimeDTO
    {
        #region Public properties

        public int Id { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public MovieDTO Movie { get; set; }

        public DateTime SessionDate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public AuditoriumDTO Auditorium { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<TicketDTO> Tickets { get; set; }

        #endregion
    }
}