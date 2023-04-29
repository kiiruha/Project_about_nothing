using Newtonsoft.Json;

namespace ApiApplication.Contracts
{
    public class ValidationError
    {
        #region Public properties

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PropertyName { get; set; }

        public string Message { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object AttemptedValue { get; set; }

        #endregion
    }
}