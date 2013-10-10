using Newtonsoft.Json;

namespace LevelUpApi.Models.Responses
{
    [JsonObject]
    internal class ErrorContainer
    {
        [JsonProperty(PropertyName = "object")]
        public string Object { get; set; }

        [JsonProperty(PropertyName = "property")]
        public string Property { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        public override string ToString()
        {
            return Message;
        }
    }
}
