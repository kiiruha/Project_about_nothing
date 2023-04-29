using ApiApplication.Application.Queries;

namespace ApiApplication.Application.Validations
{
    public class GetTicketsForShowtimeQueryValidator : AbstractValidator<GetTicketsForShowtimeQuery>
    {
        #region Public constructors

        public GetTicketsForShowtimeQueryValidator(ILogger<GetTicketsForShowtimeQueryValidator> logger)
        {
            RuleFor(command => command.ShowtimeId).GreaterThanOrEqualTo(0).WithMessage("ShowtimeId cannot be less then 0");

            logger.LogTrace("----- INSTANCE WAS CREATED - {ClassName}", GetType().Name);
        }

        #endregion
    }
}