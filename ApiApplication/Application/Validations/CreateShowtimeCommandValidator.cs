using ApiApplication.Application.Commands;

namespace ApiApplication.Application.Validations
{
    public class CreateShowtimeCommandValidator : AbstractValidator<CreateShowtimeCommand>
    {
        #region Public constructors

        public CreateShowtimeCommandValidator(ILogger<CreateShowtimeCommandValidator> logger)
        {
            RuleFor(command => command.MovieId).NotEmpty();
            RuleFor(command => command.AuditoriumId).GreaterThanOrEqualTo(0).WithMessage("Auditorium id cannot be less then 0");
            RuleFor(command => command.SessionDate.ToUniversalTime()).NotEmpty().GreaterThan(DateTime.UtcNow).WithMessage("Session date should be in the future");

            logger.LogTrace("----- INSTANCE WAS CREATED - {ClassName}", GetType().Name);
        }

        #endregion
    }
}