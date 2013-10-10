using Newtonsoft.Json;
using System;

namespace LevelUpApi.Models.Responses
{
    [JsonObject]
    internal class LocationContainerBase
    {
        [JsonProperty(PropertyName = "id")]
        public int LocationId { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "merchant_tip_preference")]
        public string TipPreference { get; set; }

        public override string ToString()
        {
            return string.Format("Name: {0}{1}Location Id: {2}{1}",
                                 Name,
                                 Environment.NewLine,
                                 LocationId);
        }
    }
}
