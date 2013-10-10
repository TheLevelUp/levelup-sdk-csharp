using Newtonsoft.Json;

namespace LevelUpApi.Models.Requests
{
    /// <summary>
    /// Class representing a request for a LevelUp refund
    /// </summary>
    [JsonObject]
    public class Refund
    {
        /// <summary>
        /// Constructor for creating a LevelUp refund request
        /// </summary>
        /// <param name="managerConfirmation">A manager confirmation string if required by the POS system. 
        /// Default is null</param>
        public Refund(string managerConfirmation = null)
        {
            RefundContainer = new RefundContainer(managerConfirmation);
        }

        /// <summary>
        /// A manager refund confirmation string. May be null.
        /// </summary>
        [JsonIgnore]
        public string ManagerConfirmation { get { return RefundContainer.ManagerConfirmation; } }

        /// <summary>
        /// Use this container to aid in correct JSON serialization
        /// </summary>
        [JsonProperty(PropertyName = "refund")]
        private RefundContainer RefundContainer { get; set; }
    }
}
