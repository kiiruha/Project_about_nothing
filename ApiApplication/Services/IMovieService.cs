using ApiApplication.Models.DTO;
using System.Threading.Tasks;

namespace ApiApplication.Services
{
    public interface IMovieService
    {
        #region Public methods

        Task<MovieFromAPIDTO> GetByIdAsync(string id);

        Task<IEnumerable<MovieFromAPIDTO>> SearchAsync(string text);

        Task<IEnumerable<MovieFromAPIDTO>> GetAllAsync();

        #endregion
    }
}