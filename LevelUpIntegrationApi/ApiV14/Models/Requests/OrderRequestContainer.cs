using Newtonsoft.Json;
using System.Collections.Generic;

namespace LevelUpApi.Models.Requests
{
    [JsonObject]
    internal class OrderRequestContainer
    {
        public OrderRequestContainer(int locationId,
                                     string qrPaymentData,
                                     int spendAmountCents,
                                     string register,
                                     string cashier,
                                     string identifierFromMerchant,
                                     IList<Item> items)
        {
            this.LocationId = locationId;
            this.PaymentData = qrPaymentData;
            this.SpendAmountCents = spendAmountCents;
            this.Identifier = identifierFromMerchant;
            this.Register = register;
            this.Cashier = cashier;
            this.Items = items;
        }

        [JsonProperty(PropertyName = "items")]
        public IList<Item> Items { get; set; }

        [JsonProperty(PropertyName = "identifier_from_merchant")]
        public string Identifier { get; set; }

        [JsonProperty(PropertyName = "location_id")]
        public int LocationId { get; set; }

        [JsonProperty(PropertyName = "payment_token_data")]
        public string PaymentData { get; set; }

        [JsonProperty(PropertyName = "spend_amount")]
        public int SpendAmountCents { get; set; }

        [JsonProperty(PropertyName = "register")]
        public string Register { get; set; }

        [JsonProperty(PropertyName = "cashier")]
        public string Cashier { get; set; }
    }
}
