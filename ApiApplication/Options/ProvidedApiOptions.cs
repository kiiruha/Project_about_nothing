using System.ComponentModel.DataAnnotations;

namespace ApiApplication.Configs
{
    public class ProvidedApiOptions
    {
        #region Public properties

        [Required]
        public string ApiKey { get; set; }

        [Required]
        [Url]
        public string GrpcUrl { get; set; }

        #endregion
    }
}