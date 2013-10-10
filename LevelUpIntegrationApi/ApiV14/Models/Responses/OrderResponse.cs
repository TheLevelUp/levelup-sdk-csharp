using Newtonsoft.Json;

namespace LevelUpApi.Models.Responses
{
    /// <summary>
    /// Class representing a response to an order request made to LevelUp
    /// </summary>
    [JsonObject]
    public class OrderResponse
    {
        /// <summary>
        /// Amount in cents spent on purchase not including tip. 
        /// </summary>
        [JsonIgnore]
        public int SpendAmount { get { return OrderResponseContainer.SpendAmount; } }

        /// <summary>
        /// Amount in cents spent as tip/gratuity
        /// </summary>
        [JsonIgnore]
        public int TipAmount { get { return OrderResponseContainer.TipAmount; } }

        /// <summary>
        /// Total amount in cents spent on purchase plus tip/gratuity
        /// </summary>
        [JsonIgnore]
        public int Total { get { return OrderResponseContainer.Total; } }

        /// <summary>
        /// UUID that uniquely identifies the order
        /// </summary>
        [JsonIgnore]
        public string Identifier { get { return OrderResponseContainer.Identifier; } }

        /// <summary>
        /// This container is used to aid in correct JSON serialization
        /// </summary>
        [JsonProperty(PropertyName = "order")]
        private OrderContainerBase OrderResponseContainer { get; set; }

        public override string ToString()
        {
            return OrderResponseContainer.ToString();
        }
    }
}
