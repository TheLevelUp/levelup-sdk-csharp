
namespace ConfigurationTool
{
    public class LevelUpData
    {
        private static LevelUpData _singleton;

        public static LevelUpData Instance 
        {
            get { return _singleton ?? (_singleton = new LevelUpData()); }
        }

        private LevelUpData() { }

        public string AccessToken { get; set; }

        public int? MerchantId { get; set; }

        public string MerchantName { get; set; }

        public int? LocationId { get; set; }

        public bool IsValid
        {
            get
            {
                return !string.IsNullOrEmpty(AccessToken) &&
                       MerchantId.HasValue &&
                       LocationId.HasValue;
            }
        }
    }
}
