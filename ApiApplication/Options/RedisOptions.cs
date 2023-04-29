using System.ComponentModel.DataAnnotations;

namespace ApiApplication.Options
{
    public class RedisOptions
    {
        #region Public properties

        [Required]
        public string Configuration { get; set; }

        #endregion
    }
}