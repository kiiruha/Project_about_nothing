using System.Net;

namespace ApiApplication.Exceptions
{
    [Serializable]
    public class MovieException : Exception
    {
        #region Public properties

        public HttpStatusCode HttpStatusCode { get; }

        #endregion

        #region Public constructors

        public MovieException()
        { }

        public MovieException(int statusCode, string message)
           : base(message)
        {
            HttpStatusCode = (HttpStatusCode)statusCode;
        }

        public MovieException(int statusCode, string message, Exception innerException)
            : base(message, innerException)
        {
            HttpStatusCode = (HttpStatusCode)statusCode;
        }

        #endregion
    }
}