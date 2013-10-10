using Newtonsoft.Json;

namespace LevelUpApi.Models.Responses
{
    /// <summary>
    /// Class representing a LevelUp access token response
    /// </summary>
    [JsonObject]
    public class AccessToken
    {
        /// <summary>
        /// The LevelUp access token. This is required for further interaction with the LevelUp API
        /// </summary>
        [JsonIgnore]
        public string Token { get { return TokenContainer.Token; } }
        
        /// <summary>
        /// Identifier for the requesting user account
        /// </summary>
        [JsonIgnore]
        public int UserId { get { return TokenContainer.UserId; } }

        /// <summary>
        /// Merchant identifier. This will have a value if the requesting account is a LevelUp merchant
        /// </summary>
        [JsonIgnore]
        public int? MerchantId { get { return TokenContainer.MerchantId; } }

        /// <summary>
        /// Returns true if the requesting account is a LevelUp merchant
        /// </summary>
        [JsonIgnore]
        public bool IsMerchant { get { return TokenContainer.IsMerchant; } }

        /// <summary>
        /// This container is used to aid in correct JSON serialization
        /// </summary>
        [JsonProperty(PropertyName = "access_token")]
        private AccessTokenContainer TokenContainer { get; set; }

        public override string ToString()
        {
            return TokenContainer.ToString();
        }
    }
}
