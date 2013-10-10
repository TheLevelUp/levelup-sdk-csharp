using Newtonsoft.Json;

namespace LevelUpApi.Models.Requests
{
    [JsonObject]
    internal class RefundContainer
    {
        public RefundContainer(string managerConfirmation)
        {
            this.ManagerConfirmation = managerConfirmation;
        }

        [JsonProperty(PropertyName = "manager_confirmation")]
        public string ManagerConfirmation { get; set; }
    }
}
