using Newtonsoft.Json;
using System;
using System.Globalization;

namespace LevelUpApi.Models.Responses
{
    [JsonObject]
    internal class OrderContainer : OrderContainerBase
    {
        [JsonProperty(PropertyName = "created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty(PropertyName = "merchant_funded_credit_amount")]
        public int MerchantFundedCreditAmount { get; set; }

        [JsonProperty(PropertyName = "earn_amount")]
        public int EarnAmount { get; set; }

        [JsonProperty(PropertyName = "loyalty_id")]
        public int LoyaltyId { get; set; }

        [JsonProperty(PropertyName = "refunded_at")]
        public string RefundedAt { get; set; }

        [JsonProperty(PropertyName = "transacted_at")]
        public string TransactedAt { get; set; }

        [JsonProperty(PropertyName = "location_id")]
        public int LocationId { get; set; }

        [JsonProperty(PropertyName = "user_display_name")]
        public string UserName { get; set; }

        public override string ToString()
        {
            return string.Format(new CultureInfo("en-Us"),
                                 "{0}" +
                                 "User name: {1}{2}" +
                                 "Created at: {3}{2}" +
                                 "Time of transaction: {4}{2}" +
                                 "Time of refund: {5}{2}" +
                                 "Merchant funded credit amount: {6}{2}" +
                                 "Amount earned: {7}{2}" +
                                 "Loyalty id: {8}{2}" +
                                 "Location of transaction: {9}{2}",
                                 base.ToString(),
                                 UserName,
                                 Environment.NewLine,
                                 CreatedAt,
                                 TransactedAt ?? "NULL",
                                 RefundedAt ?? "NULL",
                                 MerchantFundedCreditAmount,
                                 EarnAmount,
                                 LoyaltyId,
                                 LocationId);
        }
    }
}
