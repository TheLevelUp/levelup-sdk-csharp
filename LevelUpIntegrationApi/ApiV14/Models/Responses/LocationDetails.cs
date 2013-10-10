using Newtonsoft.Json;
using System;

namespace LevelUpApi.Models.Responses
{
    /// <summary>
    /// Class representing a LevelUp merchant location details
    /// </summary>
    [JsonObject]
    public class LocationDetails
    {
        /// <summary>
        /// Address of the location
        /// </summary>
        [JsonIgnore]
        public Address Address
        {
            get
            {
                return new Address(LocationContainer.MerchantName,
                                   LocationContainer.StreetAddress,
                                   LocationContainer.ExtendedAddress,
                                   LocationContainer.Locality,
                                   LocationContainer.Region,
                                   string.Empty,
                                   LocationContainer.ZipCode,
                                   "U.S.A",
                                   string.IsNullOrEmpty(LocationContainer.MenuUrl) ? null : new Uri(LocationContainer.MenuUrl),
                                   new GeographicLocation(LocationContainer.Latitude, LocationContainer.Longitude));
            }
        }

        /// <summary>
        /// The ID of the location
        /// </summary>
        [JsonIgnore]
        public int LocationId { get { return LocationContainer.LocationId; } }

        /// <summary>
        /// ID of the merchant whose location this is
        /// </summary>
        [JsonIgnore]
        public int MerchantId { get { return LocationContainer.MerchantId; } }

        /// <summary>
        /// Name of the merchant whose location this is
        /// </summary>
        [JsonIgnore]
        public string MerchantName { get { return LocationContainer.MerchantName; } }

        /// <summary>
        /// Returns true if the location is set to be visible to users. False otherwise
        /// </summary>
        [JsonIgnore]
        public bool IsVisible { get { return LocationContainer.Visible; } }

        /// <summary>
        /// Latitude and Longitude coordinates of the location for mapping
        /// </summary>
        [JsonIgnore]
        public GeographicLocation LocationCoordinates
        {
            get { return new GeographicLocation(LocationContainer.Latitude, LocationContainer.Longitude); }
        }

        /// <summary>
        /// This container is used to aid in correct JSON serialization
        /// </summary>
        [JsonProperty(PropertyName = "location")]
        private LocationContainer LocationContainer { get; set; }

        public override string ToString()
        {
            return LocationContainer.ToString();
        }
    }
}
