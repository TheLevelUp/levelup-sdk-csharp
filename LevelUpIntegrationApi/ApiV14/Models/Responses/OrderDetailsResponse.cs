using Newtonsoft.Json;

namespace LevelUpApi.Models.Responses
{
    /// <summary>
    /// Class representing a detailed response to a LevelUp order request
    /// </summary>
    [JsonObject]
    public class OrderDetailsResponse
    {
        /// <summary>
        /// UUID which uniquely identifies the order
        /// </summary>
        [JsonIgnore]
        public string Identifier { get { return OrderResponseContainer.Identifier; } }

        /// <summary>
        /// Amount in cents spent on the order not including tip
        /// </summary>
        [JsonIgnore]
        public int SpendAmount { get { return OrderResponseContainer.SpendAmount; } }

        /// <summary>
        /// Amount in cents of the tip spent on the order
        /// </summary>
        [JsonIgnore]
        public int TipAmount { get { return OrderResponseContainer.TipAmount; } }

        /// <summary>
        /// Total amount in cents spent on the order
        /// </summary>
        [JsonIgnore]
        public int Total { get { return OrderResponseContainer.Total; } }

        /// <summary>
        /// Time the order was created in LevelUp system
        /// UTC time ISO standard 8601  YYYY-MM-DDTHH:MM:SSZ
        /// </summary>
        [JsonIgnore]
        public string CreatedAt { get { return OrderResponseContainer.CreatedAt; } }

        /// <summary>
        /// Amount of credit the merchant sponsored
        /// </summary>
        [JsonIgnore]
        public int MerchantFundedCredit { get { return OrderResponseContainer.MerchantFundedCreditAmount; } }

        /// <summary>
        /// Customer loyalty earned in this purchase
        /// </summary>
        [JsonIgnore]
        public int EarnAmount { get { return OrderResponseContainer.EarnAmount; } }

        /// <summary>
        /// User identifier 
        /// </summary>
        [JsonIgnore]
        public int LoyaltyId { get { return OrderResponseContainer.LoyaltyId; } }

        /// <summary>
        /// Time the refund was processed on LevelUp servers. 
        /// UTC time ISO standard 8601  YYYY-MM-DDTHH:MM:SSZ
        /// If the order has not been refunded, this value will be NULL
        /// </summary>
        [JsonIgnore]
        public string TimeOfRefund { get { return OrderResponseContainer.RefundedAt; } }

        /// <summary>
        /// User display name. User "First_name Last_initial"
        /// </summary>
        [JsonIgnore]
        public string UserName { get { return OrderResponseContainer.UserName; } }

        /// <summary>
        /// Time the transaction was processed at the merchant location. 
        /// UTC time ISO standard 8601  YYYY-MM-DDTHH:MM:SSZ
        /// </summary>
        [JsonIgnore]
        public string TimeOfTransaction { get { return OrderResponseContainer.TransactedAt; } }

        /// <summary>
        /// ID of the location where the order was placed
        /// </summary>
        [JsonIgnore]
        public int LocationId { get { return OrderResponseContainer.LocationId; } }

        /// <summary>
        /// This container is used to aid in correct JSON serialization
        /// </summary>
        [JsonProperty(PropertyName = "order")]
        private OrderContainer OrderResponseContainer { get; set; }

        public override string ToString()
        {
            return OrderResponseContainer.ToString();
        }
    }
}
