using ApiApplication.Application.Queries;

namespace ApiApplication.Application.Validations
{
    public class GetTicketQueryValidator : AbstractValidator<GetTicketQuery>
    {
        #region Public constructors

        public GetTicketQueryValidator(ILogger<GetTicketQueryValidator> logger)
        {
            RuleFor(command => command.Id).NotEmpty().WithMessage("Id cannot be empty");

            logger.LogTrace("----- INSTANCE WAS CREATED - {ClassName}", GetType().Name);
        }

        #endregion
    }
}