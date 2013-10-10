using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LevelUpApi.Models.Responses
{
    [JsonObject]
    internal class LocationContainer : LocationContainerBase
    {
        [JsonProperty(PropertyName = "latitude")]
        public double Latitude { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public double Longitude { get; set; }

        [JsonProperty(PropertyName = "merchant_id")]
        public int MerchantId { get; set; }

        [JsonProperty(PropertyName = "merchant_name")]
        public string MerchantName { get; set; }

        [JsonProperty(PropertyName = "categories")]
        public List<int> Categories { get; set; }

        [JsonProperty(PropertyName = "shown")]
        public bool Visible { get; set; }

        [JsonProperty(PropertyName = "updated_at")]
        public string TimeOfLastUpdate { get; set; }

        [JsonProperty(PropertyName = "extended_address")]
        public string ExtendedAddress { get; set; }

        [JsonProperty(PropertyName = "facebook_url")]
        public string FacebookUrl { get; set; }

        [JsonProperty(PropertyName = "hours")]
        public string Hours { get; set; }

        [JsonProperty(PropertyName = "locality")]
        public string Locality { get; set; }

        [JsonProperty(PropertyName = "menu_url")]
        public string MenuUrl { get; set; }

        [JsonProperty(PropertyName = "merchant_description_html")]
        public string MerchantDescription { get; set; }

        [JsonProperty(PropertyName = "postal_code")]
        public string ZipCode { get; set; }

        [JsonProperty(PropertyName = "region")]
        public string Region { get; set; }

        [JsonProperty(PropertyName = "street_address")]
        public string StreetAddress { get; set; }

        public override string ToString()
        {
            if (!Visible)
            {
                return string.Format("Location with id {0} is not visible{1}", LocationId, Environment.NewLine);
            }

            return string.Format("Location Id: {0}{1}{2}{1}",
                                 LocationId,
                                 Environment.NewLine,
                                 new Address(MerchantName,
                                             StreetAddress,
                                             ExtendedAddress,
                                             Locality,
                                             Region,
                                             string.Empty,
                                             ZipCode,
                                             "U.S.A.",
                                             string.IsNullOrEmpty(MenuUrl) ? null : new Uri(MenuUrl),
                                             new GeographicLocation(Latitude, Longitude)));
        }
    }
}
