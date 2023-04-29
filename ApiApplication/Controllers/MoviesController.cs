using ApiApplication.Models.DTO;
using ApiApplication.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace ApiApplication.Controllers
{
    //TODO: probably it would be great idea to move calls to handlers,
    // but we have no validations no complex operations, so if its okay for team we can leave as it is.
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        #region Private fields

        private readonly ILogger<MoviesController> _logger;
        private readonly IMovieService _movieService;

        #endregion

        #region Public constructors

        public MoviesController(ILogger<MoviesController> logger, IMovieService movieService)
        {
            _logger = logger;
            _movieService = movieService;
        }

        #endregion

        #region Public methods

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MovieFromAPIDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetMovies()
        {
            var movies = await _movieService.GetAllAsync();
            return Ok(movies);
        }

        [Route("{movieId}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MovieFromAPIDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetMovie(string movieId)
        {
            if (string.IsNullOrWhiteSpace(movieId))
            {
                return BadRequest($"{nameof(movieId)} cannot be empty");
            }

            var movies = await _movieService.GetByIdAsync(movieId);
            return Ok(movies);
        }

        [Route("search")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MovieFromAPIDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> SearchForMovies([FromQuery] string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return BadRequest($"{nameof(text)} cannot be empty");
            }

            var movies = await _movieService.SearchAsync(text);
            return Ok(movies);
        }

        #endregion
    }
}