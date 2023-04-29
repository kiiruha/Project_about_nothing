using FluentValidation.Results;
using System.Collections.Immutable;

namespace ApiApplication.Contracts
{
    public record ValidationFailed
    {
        public IImmutableList<ValidationFailure> Errors { get; }

        public ValidationFailed(IEnumerable<ValidationFailure> errors)
        {
            Errors = errors.ToImmutableList();
        }

        public ValidationFailed(ValidationFailure error) : this(new[] { error })
        {
        }
    }
}