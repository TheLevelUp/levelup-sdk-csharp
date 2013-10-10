using Newtonsoft.Json;

namespace LevelUpApi.Models.Responses
{
    /// <summary>
    /// Class representing a LevelUp response to a refund request
    /// </summary>
    [JsonObject]
    public class RefundResponse
    {
        /// <summary>
        /// Time the order was created in LevelUp system
        /// UTC time ISO standard 8601  YYYY-MM-DDTHH:MM:SSZ
        /// </summary>
        [JsonIgnore]
        public string CreatedAt { get { return OrderRefundContainer.CreatedAt; } }

        /// <summary>
        /// Amount of credit the merchant sponsored
        /// </summary>
        [JsonIgnore]
        public int MerchantFundedCreditAmount { get { return OrderRefundContainer.MerchantFundedCreditAmount; } }

        /// <summary>
        /// Customer loyalty earned in this purchase
        /// </summary>
        [JsonIgnore]
        public int EarnAmount { get { return OrderRefundContainer.EarnAmount; } } 

        /// <summary>
        /// UUID that uniquely identifies the order
        /// </summary>
        [JsonIgnore]
        public string Identifier { get { return OrderRefundContainer.Identifier; } }

        /// <summary>
        /// User identifier 
        /// </summary>
        [JsonIgnore]
        public int LoyaltyId { get { return OrderRefundContainer.LoyaltyId; } }

        /// <summary>
        /// Amount spent in cents not including tip
        /// </summary>
        [JsonIgnore]
        public int SpendAmount { get { return OrderRefundContainer.SpendAmount; } }

        /// <summary>
        /// Time the refund was processed on LevelUp servers. 
        /// UTC time ISO standard 8601  YYYY-MM-DDTHH:MM:SSZ
        /// If the order has not been refunded, this value will be NULL
        /// </summary>
        [JsonIgnore]
        public string TimeOfRefund { get { return OrderRefundContainer.RefundedAt; } }

        /// <summary>
        /// Tip amount in cents
        /// </summary>
        [JsonIgnore]
        public int TipAmount { get { return OrderRefundContainer.TipAmount; } } 

        /// <summary>
        /// Total amount spent in cents including cents
        /// </summary>
        [JsonIgnore]
        public int TotalAmount { get { return OrderRefundContainer.Total; } } 

        /// <summary>
        /// ID of the location where transaction occurred
        /// </summary>
        [JsonIgnore]
        public int LocationId { get { return OrderRefundContainer.LocationId; } }

        /// <summary>
        /// User display name. User "First_name Last_initial"
        /// </summary>
        [JsonIgnore]
        public string UserName { get { return OrderRefundContainer.UserName; } }

        /// <summary>
        /// Time the transaction was processed at the merchant location. 
        /// UTC time ISO standard 8601  YYYY-MM-DDTHH:MM:SSZ
        /// </summary>
        [JsonIgnore]
        public string TimeOfTransaction { get { return OrderRefundContainer.TransactedAt; } }

        /// <summary>
        /// Use this container to aid in correct JSON serialization
        /// </summary>
        [JsonProperty(PropertyName = "order")]
        private OrderContainer OrderRefundContainer { get; set; }

        public override string ToString()
        {
            return OrderRefundContainer.ToString();
        }
    }
}
