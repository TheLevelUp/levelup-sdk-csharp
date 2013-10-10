using System.Globalization;
using Newtonsoft.Json;
using System;

namespace LevelUpApi.Models.Responses
{
    [JsonObject]
    internal class OrderContainerBase
    {
        [JsonProperty(PropertyName = "spend_amount")]
        public int SpendAmount { get; set; }

        [JsonProperty(PropertyName = "tip_amount")]
        public int TipAmount { get; set; }

        [JsonProperty(PropertyName = "total_amount")]
        public int Total { get; set; }

        [JsonProperty(PropertyName = "uuid")]
        public string Identifier { get; set; }

        public override string ToString()
        {
            return string.Format(new CultureInfo("en-US"),
                                 "Id: {0}{1}Spent: {2:C2}{1}Tip: {3:C2}{1}Total: {4:C2}{1}",
                                 Identifier,
                                 Environment.NewLine,
                                 (double) SpendAmount/100,
                                 (double) TipAmount/100,
                                 (double) Total/100);
        }
    }
}
