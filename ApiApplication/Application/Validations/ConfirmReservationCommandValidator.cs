using ApiApplication.Application.Commands;

namespace ApiApplication.Application.Validations
{
    public class ConfirmReservationCommandValidator : AbstractValidator<ConfirmReservationCommand>
    {
        #region Public constructors

        public ConfirmReservationCommandValidator(ILogger<ConfirmReservationCommandValidator> logger)
        {
            RuleFor(command => command.Id).NotEmpty().WithMessage("Id cannot be empty");

            logger.LogTrace("----- INSTANCE WAS CREATED - {ClassName}", GetType().Name);
        }

        #endregion
    }
}