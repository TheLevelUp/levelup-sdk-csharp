using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;

namespace ConfigurationTool
{
    [DataContract]
    [JsonObject]
    public class ConfiguredSettings
    {
        public const string DEFAULT_CONFIG_FILE_NAME = "LevelUp.Config";
        public static ConfiguredSettings LoadConfigData(string pathToConfigFile = DEFAULT_CONFIG_FILE_NAME)
        {
            ConfiguredSettings settings = null;

            if (File.Exists(pathToConfigFile))
            {
                string configContents = File.ReadAllText(pathToConfigFile);

                if (configContents[0] == '{')
                {
                    settings = DeserializeFromJson(configContents);
                }
                else if (configContents[0] == '<')
                {
                    settings = DeserializeFromXml(configContents);
                }
            }

            return settings;
        }

        public ConfiguredSettings() : this(string.Empty, null, null, string.Empty) { }

        public ConfiguredSettings(string levelUpAccessToken = "",
                                  int? levelUpMerchantId = null,
                                  int? levelUpLocationId = null,
                                  string levelUpMerchantName = "")
        {
            this.LevelUpAccessToken = levelUpAccessToken;
            this.LevelUpMerchantId = levelUpMerchantId;
            this.LevelUpLocationId = levelUpLocationId;
            this.LevelUpMerchantName = levelUpMerchantName;
        }

        [DataMember(Name = "AccessToken")]
        [JsonProperty(PropertyName = "access_token")]
        public string LevelUpAccessToken { get; set; }

        [DataMember(Name = "MerchantId")]
        [JsonProperty(PropertyName = "merchant_id")]
        public int? LevelUpMerchantId { get; set; }

        [DataMember(Name = "LocationId")]
        [JsonProperty(PropertyName = "location_id")]
        public int? LevelUpLocationId { get; set; }

        [DataMember(Name = "MerchantName")]
        [JsonProperty(PropertyName = "merchant_name")]
        public string LevelUpMerchantName { get; set; }
        
        public void SerializeToJson(string outputPath = DEFAULT_CONFIG_FILE_NAME)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Formatting = Formatting.Indented;

            JsonSerializer serializer = JsonSerializer.Create(settings);
            using (TextWriter writer = new StreamWriter(outputPath))
            {
                serializer.Serialize(writer, this);
            }
        }

        public void SerializeToXml(string outputPath = DEFAULT_CONFIG_FILE_NAME)
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

        public string Verify()
        {
            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrEmpty(LevelUpAccessToken))
            {
                sb.AppendLine("Access token is null or empty!");
            }

            if (!LevelUpMerchantId.HasValue)
            {
                sb.AppendLine("Merchant ID has no value!");
            }

            if (!LevelUpLocationId.HasValue)
            {
                sb.AppendLine("Location ID has no value!");
            }

            return sb.ToString();
        }

        private static ConfiguredSettings DeserializeFromJson(string jsonContent)
        {
            return JsonConvert.DeserializeObject<ConfiguredSettings>(jsonContent);
        }

        private static ConfiguredSettings DeserializeFromXml(string xmlContent)
        {
            DataContractSerializer dcs = new DataContractSerializer(typeof(ConfiguredSettings));
            ConfiguredSettings settings = null;

            using (TextReader textReader = new StringReader(xmlContent))
            {
                using (XmlReader reader = new XmlTextReader(textReader))
                {
                    settings = dcs.ReadObject(reader) as ConfiguredSettings;
                }
            }

            return settings;
        }
    }
}
