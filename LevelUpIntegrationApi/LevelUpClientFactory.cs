
namespace LevelUpApi
{
    public class LevelUpClientFactory
    {
        public enum LevelUpApiVersion
        {
            v14
        }

        private LevelUpClientFactory()
        {
        }

        public static ILevelUpClient Create(string companyName,
                                            string productName,
                                            string productVersion,
                                            string osName,
                                            LevelUpApiVersion version = LevelUpApiVersion.v14)
        {
            return LevelUpClientFactory.Create(new AgentIdentifier(companyName, productName, productVersion, osName),
                                               version);
        }

        public static ILevelUpClient Create(AgentIdentifier identifier,
                                            LevelUpApiVersion version = LevelUpApiVersion.v14)
        {
            switch (version)
            {
                default:
                    return new LevelUpApiV14(identifier);
            }
        }
    }
}
