using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace LevelUpExampleApp
{
    public class LevelUpData
    {
        private static LevelUpData _singleton;

        private static LevelUpData DeserializeFromJson(string jsonContent)
        {
            return JsonConvert.DeserializeObject<LevelUpData>(jsonContent);
        }

        private static LevelUpData DeserializeFromXml(string xmlContent)
        {
            DataContractSerializer dcs = new DataContractSerializer(typeof(LevelUpData));
            LevelUpData settings = null;

            using (TextReader textReader = new StringReader(xmlContent))
            {
                using (XmlReader reader = new XmlTextReader(textReader))
                {
                    settings = dcs.ReadObject(reader) as LevelUpData;
                }
            }

            return settings;
        }

        public static LevelUpData Instance 
        {
            get { return _singleton ?? (_singleton = new LevelUpData()); }
            private set { _singleton = value; }
        }

        public static void LoadConfigData(string pathToConfigFile)
        {
            if (!File.Exists(pathToConfigFile))
            {
                throw new FileNotFoundException(string.Format("{0} not found!", pathToConfigFile), pathToConfigFile);
            }

            string configContents = File.ReadAllText(pathToConfigFile);

            if (configContents[0] == '{')
            {
                Instance = DeserializeFromJson(configContents);
            }
            else if (configContents[0] == '<')
            {
                Instance = DeserializeFromXml(configContents);
            }
        }

        public LevelUpData() : this(string.Empty, null, null, string.Empty) { }

        public LevelUpData(string accessToken = "",
                           int? merchantId = null,
                           int? locationId = null,
                           string merchantName = "")
        {
            this.AccessToken = accessToken;
            this.MerchantId = merchantId;
            this.LocationId = locationId;
            this.MerchantName = merchantName;
        }

        [DataMember(Name = "AccessToken")]
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [DataMember(Name = "MerchantId")]
        [JsonProperty(PropertyName = "merchant_id")]
        public int? MerchantId { get; set; }

        [DataMember(Name = "LocationId")]
        [JsonProperty(PropertyName = "location_id")]
        public int? LocationId { get; set; }

        [DataMember(Name = "MerchantName")]
        [JsonProperty(PropertyName = "merchant_name")]
        public string MerchantName { get; set; }

        public bool IsValid()
        {
            string errorMessages;
            return IsValid(out errorMessages);
        }

        public bool IsValid(out string errorMessages)
        {
            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrEmpty(AccessToken))
            {
                sb.AppendLine("Access token is null or empty!");
            }

            if (!MerchantId.HasValue)
            {
                sb.AppendLine("Merchant ID has no value!");
            }

            if (!LocationId.HasValue)
            {
                sb.AppendLine("Location ID has no value!");
            }

            errorMessages = sb.ToString();

            return string.IsNullOrEmpty(errorMessages);
        }

        public void SerializeToJson(string outputPath)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Formatting = Formatting.Indented;

            JsonSerializer serializer = JsonSerializer.Create(settings);
            using (TextWriter writer = new StreamWriter(outputPath))
            {
                serializer.Serialize(writer, this);
            }
        }

        public void SerializeToXml(string outputPath)
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "    "
            };

            DataContractSerializer dcs = new DataContractSerializer(this.GetType());
            using (XmlWriter writer = XmlWriter.Create(outputPath, xmlWriterSettings))
            {
                dcs.WriteObject(writer, this);
            }
        }
    }
}
