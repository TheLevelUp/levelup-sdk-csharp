using Newtonsoft.Json;

namespace LevelUpApi.Models.Responses
{
    /// <summary>
    /// An object representing an error returned from LevelUp
    /// </summary>
    [JsonObject]
    public class ErrorResponse
    {
        /// <summary>
        /// LevelUp object that is invalid as a result of the error
        /// </summary>
        [JsonIgnore]
        public string Object { get { return ErrorContainer.Object; } }

        /// <summary>
        /// Specific attribute or property that is in error
        /// </summary>
        [JsonIgnore]
        public string Property { get { return ErrorContainer.Property; } }

        /// <summary>
        /// Friendly error message returned from LevelUp
        /// </summary>
        [JsonIgnore]
        public string Message { get { return ErrorContainer.Message; } }

        /// <summary>
        /// This container is used to aid in correct JSON serialization
        /// </summary>
        [JsonProperty(PropertyName = "error")]
        private ErrorContainer ErrorContainer { get; set; }

        public override string ToString()
        {
            return ErrorContainer.ToString();
        }
    }
}
