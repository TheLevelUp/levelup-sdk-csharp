using Newtonsoft.Json;

namespace LevelUpApi.Models.Requests
{
    [JsonObject]
    internal class AccessTokenRequestContainer
    {
        public AccessTokenRequestContainer(string clientId, string username, string password)
        {
            this.ClientId = clientId;
            this.Username = username;
            this.Password = password;
        }

        [JsonProperty(PropertyName = "client_id")]
        public string ClientId { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }
    }
}
