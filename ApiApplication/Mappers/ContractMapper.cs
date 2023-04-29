using ApiApplication.Contracts;
using System.Linq;

namespace ApiApplication.Mappers
{
    public static class ContractMapper
    {
        #region Public methods

        public static ValidationFailureResponse MapToResponse(this ValidationFailed validationFailed)
        {
            return new ValidationFailureResponse
            {
                Errors = validationFailed.Errors.Select(failure => new ValidationError
                {
                    PropertyName = failure.PropertyName,
                    Message = failure.ErrorMessage,
                    AttemptedValue = failure.AttemptedValue,
                }).Distinct()
            };
        }

        #endregion
    }
}