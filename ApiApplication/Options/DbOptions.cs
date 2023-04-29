using System.ComponentModel.DataAnnotations;

namespace ApiApplication.Options
{
    public class DbOptions
    {
        #region Public properties

        [Required]
        public string Name { get; set; }

        #endregion
    }
}