using ApiApplication.Application.Behaviors;
using ApiApplication.Contracts;
using MediatR;

namespace ApiApplication.Extensions
{
    public static class ValidationExtensions
    {
        #region Public methods

        public static MediatRServiceConfiguration AddValidation<TRequest, TResponse>(this MediatRServiceConfiguration config) where TRequest : notnull
        {
            return config.AddBehavior<IPipelineBehavior<TRequest, Result<TResponse, ValidationFailed>>, ValidatorBehavior<TRequest, TResponse>>();
        }

        #endregion
    }
}