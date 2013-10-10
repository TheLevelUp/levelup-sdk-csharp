using System;
using Newtonsoft.Json;

namespace LevelUpApi.Models.Responses
{
    [JsonObject]
    internal class AccessTokenContainer
    {
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }

        [JsonProperty(PropertyName = "user_id")]
        public int UserId { get; set; }

        [JsonProperty(PropertyName = "merchant_id")]
        public int? MerchantId { get; set; }

        [JsonIgnore]
        public bool IsMerchant { get { return MerchantId.HasValue && MerchantId.Value > 0; } }

        public override string ToString()
        {
            string output = string.Format("Access Token: {0}{1}" +
                                          "User Id: {2}{1}",
                                          Token,
                                          Environment.NewLine,
                                          UserId);
            if (IsMerchant)
            {
                output += string.Format("Merchant Id: {0}{1}", MerchantId.Value, Environment.NewLine);
            }

            return output;
        }
    }
}
