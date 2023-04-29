namespace ApiApplication.Contracts
{
    public record ValidationFailureResponse
    {
        #region Public properties

        public IEnumerable<ValidationError> Errors { get; set; }

        #endregion
    }
}