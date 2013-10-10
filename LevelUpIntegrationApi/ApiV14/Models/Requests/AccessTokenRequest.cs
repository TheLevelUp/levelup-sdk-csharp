using Newtonsoft.Json;

namespace LevelUpApi.Models.Requests
{
    /// <summary>
    /// Class representing a LevelUp access token request object
    /// </summary>
    [JsonObject]
    public class AccessTokenRequest
    {
        /// <summary>
        /// Constructor for a LevelUp access token request
        /// </summary>
        /// <param name="clientId">The developer's LevelUp client ID. This is your API key</param>
        /// <param name="username">The merchant's LevelUp user name</param>
        /// <param name="password">The merchant's LevelUp password</param>
        public AccessTokenRequest(string clientId, string username, string password)
        {
            this.AccessTokenRequestContainer = new AccessTokenRequestContainer(clientId, username, password);
        }

        /// <summary>
        /// A string identifier that uniquely identifies the source of the access token request
        /// i.e. identifies which integration the request originated from
        /// </summary>
        [JsonIgnore]
        public string ClientId { get { return AccessTokenRequestContainer.ClientId; } }

        /// <summary>
        /// The LevelUp user name of the party on whose behalf the access token request is made
        /// </summary>
        [JsonIgnore]
        public string Username { get { return AccessTokenRequestContainer.Username; } }

        /// <summary>
        /// The LevelUp password of the party on whose behalf the access token request is made
        /// </summary>
        [JsonIgnore]
        public string Password { get { return AccessTokenRequestContainer.Password; } }

        /// <summary>
        /// This container is used to aid in correct JSON serialization
        /// </summary>
        [JsonProperty(PropertyName = "access_token")]
        private AccessTokenRequestContainer AccessTokenRequestContainer { get; set; }
    }
}
