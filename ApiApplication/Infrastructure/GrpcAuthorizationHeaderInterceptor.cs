using ApiApplication.Configs;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace ApiApplication.Infrastructure
{
    public class GrpcAuthorizationHeaderInterceptor : Interceptor
    {
        #region Private fields

        private readonly ILogger<GrpcAuthorizationHeaderInterceptor> _logger;
        private readonly ProvidedApiOptions _authData;

        #endregion

        #region Public constructors

        public GrpcAuthorizationHeaderInterceptor(ILogger<GrpcAuthorizationHeaderInterceptor> logger, IOptions<ProvidedApiOptions> authDataOptions)
        {
            _logger = logger;
            _authData = authDataOptions.Value;
        }

        #endregion

        #region Public methods

        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
            TRequest request,
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            return base.AsyncUnaryCall(request, GetAuthorizedClientInterceptorContext(context), continuation);
        }

        #endregion

        #region Private methods

        private ClientInterceptorContext<TRequest, TResponse> GetAuthorizedClientInterceptorContext<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context) where TRequest : class where TResponse : class
        {
            var headers = new Metadata
            {
                new Metadata.Entry("X-Apikey", _authData.ApiKey)
            };

            var newOptions = context.Options.WithHeaders(headers);

            var newContext = new ClientInterceptorContext<TRequest, TResponse>(
                context.Method,
                context.Host,
                newOptions);

            return newContext;
        }

        #endregion
    }
}