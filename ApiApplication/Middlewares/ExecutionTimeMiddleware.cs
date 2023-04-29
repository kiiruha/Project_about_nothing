using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ApiApplication.Middlewares
{
    public class ExecutionTimeMiddleware
    {
        #region Private fields

        private readonly ILogger<ExecutionTimeMiddleware> _logger;
        private readonly RequestDelegate _next;

        #endregion

        #region Public constructors

        public ExecutionTimeMiddleware(ILogger<ExecutionTimeMiddleware> logger, RequestDelegate next)
        {
            _next = next;
            _logger = logger;
        }

        #endregion

        #region Public methods

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            await _next(context);

            stopwatch.Stop();

            _logger.LogInformation($"Request {context.Request.Method} {context.Request.Path} took {stopwatch.ElapsedMilliseconds} ms to complete.");
        }

        #endregion
    }
}