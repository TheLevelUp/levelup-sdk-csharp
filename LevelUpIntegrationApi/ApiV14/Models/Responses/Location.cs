using Newtonsoft.Json;

namespace LevelUpApi.Models.Responses
{
    /// <summary>
    /// Class representing a basic LevelUp merchant location
    /// </summary>
    [JsonObject]
    public class Location
    {
        /// <summary>
        /// Identification number for the location
        /// </summary>
        [JsonIgnore]
        public int LocationId { get { return LocationContainer.LocationId; } }

        /// <summary>
        /// Name of the location
        /// </summary>
        [JsonIgnore]
        public string Name { get { return LocationContainer.Name; } }

        /// <summary>
        /// Tip preference for the location
        /// </summary>
        [JsonIgnore]
        public string TipPreference { get { return LocationContainer.TipPreference; } }

        /// <summary>
        /// This container is used to aid in correct JSON serialization
        /// </summary>
        [JsonProperty(PropertyName = "location")]
        private LocationContainerBase LocationContainer { get; set; }

        public override string ToString()
        {
            return LocationContainer.ToString();
        }
    }
}
