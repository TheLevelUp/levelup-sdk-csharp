using Newtonsoft.Json;

namespace LevelUpApi.Models.Requests
{
    [JsonObject]
    internal class ItemContainer
    {
        public ItemContainer(string name,
                    string description,
                    string sku,
                    string upc,
                    string category,
                    int chargedPriceCents,
                    int? unitPrice,
                    int quantity = 1)
        {
            this.Name = name;
            this.Description = description;
            this.Sku = sku;
            this.Upc = upc;
            this.Category = category;
            this.ChargedPriceInCents = chargedPriceCents;
            this.UnitPrice = unitPrice;
            this.Quantity = quantity;
        }

        [JsonProperty(PropertyName = "charged_price")]
        public int ChargedPriceInCents { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "quantity")]
        public int Quantity { get; set; }

        [JsonProperty(PropertyName = "sku")]
        public string Sku { get; set; }

        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; }

        [JsonProperty(PropertyName = "unit_price")]
        public int? UnitPrice { get; set; }

        [JsonProperty(PropertyName = "upc")]
        public string Upc { get; set; }
    }
}
