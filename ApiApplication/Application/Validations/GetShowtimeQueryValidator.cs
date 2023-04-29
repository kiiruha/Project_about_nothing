using ApiApplication.Application.Queries;

namespace ApiApplication.Application.Validations
{
    public class GetShowtimeQueryValidator : AbstractValidator<GetShowtimeQuery>
    {
        #region Public constructors

        public GetShowtimeQueryValidator(ILogger<GetShowtimeQueryValidator> logger)
        {
            RuleFor(command => command.Id).GreaterThanOrEqualTo(0).WithMessage("Id cannot be less then 0");

            logger.LogTrace("----- INSTANCE WAS CREATED - {ClassName}", GetType().Name);
        }

        #endregion
    }
}