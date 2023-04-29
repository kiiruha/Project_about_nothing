namespace ApiApplication.Contracts
{
    public readonly struct Result<TValue, TError>
    {
        #region Private fields

        private readonly TValue _value;
        private readonly TError _error;

        #endregion

        #region Public properties

        public bool IsError { get; }

        public bool IsSuccess => !IsError;

        #endregion

        #region Public constructors

        public Result(TValue value)
        {
            IsError = false;
            _value = value;
            _error = default;
        }

        public Result(TError error)
        {
            IsError = true;
            _error = error;
            _value = default;
        }

        #endregion

        #region Public methods

        public static implicit operator Result<TValue, TError>(TValue value)
        {
            return new Result<TValue, TError>(value);
        }

        public static implicit operator Result<TValue, TError>(TError error)
        {
            return new Result<TValue, TError>(error);
        }

        public T Match<T>(Func<TValue, T> success, Func<TError, T> failure)
        {
            if (IsError)
            {
                return failure(_error);
            }

            return success(_value);
        }

        #endregion
    }
}