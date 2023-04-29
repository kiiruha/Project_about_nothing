using ApiApplication.Contracts;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Application.Behaviors
{
    public class ValidatorBehavior<TRequest, TResult> : IPipelineBehavior<TRequest, Result<TResult, ValidationFailed>> where TRequest : notnull
    {
        #region Private fields

        private readonly IValidator<TRequest> _validator;

        #endregion

        #region Public constructors

        public ValidatorBehavior(IValidator<TRequest> validator)
        {
            _validator = validator;
        }

        #endregion

        #region Public methods

        public async Task<Result<TResult, ValidationFailed>> Handle(TRequest request, RequestHandlerDelegate<Result<TResult, ValidationFailed>> next, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return new ValidationFailed(validationResult.Errors);
            }

            return await next();
        }

        #endregion
    }
}