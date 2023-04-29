using ApiApplication.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;

namespace ApiApplication.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        #region Private fields

        private readonly ILogger<ErrorHandlerMiddleware> _logger;
        private readonly RequestDelegate _next;

        #endregion

        #region Public constructors

        public ErrorHandlerMiddleware(ILogger<ErrorHandlerMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        #endregion

        #region Public methods

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case MovieException movieException:
                        response.StatusCode = movieException.HttpStatusCode == default
                            ? (int)HttpStatusCode.BadRequest
                            : (int)movieException.HttpStatusCode;
                        break;

                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                _logger.LogError(error, "An error occurred while processing the request.");

                var result = JsonConvert.SerializeObject(new { message = error?.Message });
                await response.WriteAsync(result);
            }
        }

        #endregion
    }
}