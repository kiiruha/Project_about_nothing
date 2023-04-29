using ApiApplication.Exceptions;
using ApiApplication.Mappers;
using ApiApplication.Models.DTO;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace ApiApplication.Services
{
    public class MovieService : IMovieService
    {
        #region Private fields

        private readonly ILogger<MovieService> _logger;
        private readonly MoviesApi.MoviesApiClient _client;
        private readonly IDatabase _database;

        #endregion

        #region Public constructors

        public MovieService(ILogger<MovieService> logger, MoviesApi.MoviesApiClient moviesApiClient, ConnectionMultiplexer redis)
        {
            _logger = logger;
            _client = moviesApiClient;
            _database = redis.GetDatabase();
        }

        #endregion

        #region Public methods

        public async Task<IEnumerable<MovieFromAPIDTO>> GetAllAsync()
        {
            string cacheKey = $"{nameof(MovieService)}_all_movies";
            var responseModel = await _client.GetAllAsync(new Empty());
            if (!responseModel.Success)
            {
                return await HandleUnsuccessResponse<IReadOnlyList<MovieFromAPIDTO>>(responseModel, cacheKey);
            }

            responseModel.Data.TryUnpack<showListResponse>(out var showListResponse);
            var movies = showListResponse.ToMovieFromAPIDTOs();
            await _database.StringSetAsync(cacheKey, JsonConvert.SerializeObject(movies));
            return movies;
        }

        public async Task<MovieFromAPIDTO> GetByIdAsync(string id)
        {
            string cacheKey = $"{nameof(MovieService)}_{id}";
            var requestData = new IdRequest() { Id = id };
            var responseModel = await _client.GetByIdAsync(requestData);
            if (!responseModel.Success)
            {
                return await HandleUnsuccessResponse<MovieFromAPIDTO>(responseModel, cacheKey);
            }

            responseModel.Data.TryUnpack<showResponse>(out var showResponse);
            var movie = showResponse.ToMovieFromAPIDTO();
            await _database.StringSetAsync(cacheKey, JsonConvert.SerializeObject(movie));
            return movie;
        }

        public async Task<IEnumerable<MovieFromAPIDTO>> SearchAsync(string text)
        {
            string cacheKey = $"{nameof(MovieService)}_{text}";
            var requestData = new SearchRequest() { Text = text };
            var responseModel = await _client.SearchAsync(requestData);
            if (!responseModel.Success)
            {
                return await HandleUnsuccessResponse<IReadOnlyList<MovieFromAPIDTO>>(responseModel, cacheKey);
            }

            responseModel.Data.TryUnpack<showListResponse>(out var showListResponse);
            var movies = showListResponse.ToMovieFromAPIDTOs();
            await _database.StringSetAsync(cacheKey, JsonConvert.SerializeObject(movies));
            return movies;
        }

        #endregion

        #region Private methods

        private async Task<T> HandleUnsuccessResponse<T>(responseModel responseModel, string cacheKey) where T : class
        {
            //TODO: move to aggregation of exceptions and getting the most valuable
            var firstException = responseModel.Exceptions.First();
            if (responseModel.Exceptions.Any((moviesApiException x) => x.StatusCode == StatusCodes.Status404NotFound))
            {
                await _database.KeyDeleteAsync(cacheKey);
                return null;
            }

            var dataStringFromCache = await _database.StringGetAsync(cacheKey);
            if (string.IsNullOrEmpty(dataStringFromCache))
            {
                return null;
            }

            var dataFromCache = JsonConvert.DeserializeObject<T>(dataStringFromCache);
            if (dataFromCache != null)
            {
                return dataFromCache;
            }

            throw new MovieException(firstException.StatusCode, firstException.Message);
        }

        #endregion
    }
}