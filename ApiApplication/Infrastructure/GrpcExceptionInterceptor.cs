using Grpc.Core;
using Grpc.Core.Interceptors;
using System.Threading.Tasks;

namespace ApiApplication.Infrastructure
{
    public class GrpcExceptionInterceptor : Interceptor
    {
        #region Private fields

        private readonly ILogger<GrpcExceptionInterceptor> _logger;

        #endregion

        #region Public constructors

        public GrpcExceptionInterceptor(ILogger<GrpcExceptionInterceptor> logger)
        {
            _logger = logger;
        }

        #endregion

        #region Public methods

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
            TRequest request,
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            var call = continuation(request, context);

            return new AsyncUnaryCall<TResponse>(HandleResponse(call.ResponseAsync), call.ResponseHeadersAsync, call.GetStatus, call.GetTrailers, call.Dispose);
        }

        #endregion

        #region Private methods

        private async Task<TResponse> HandleResponse<TResponse>(Task<TResponse> task)
        {
            try
            {
                var response = await task;
                return response;
            }
            catch (RpcException e)
            {
                _logger.LogError("Error calling via grpc: {Status} - {Message}", e.Status, e.Message);
                return default;
            }
        }

        #endregion
    }
}