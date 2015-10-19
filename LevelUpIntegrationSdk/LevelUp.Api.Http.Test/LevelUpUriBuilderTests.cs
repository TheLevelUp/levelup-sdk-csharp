using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LevelUp.Api.Http.Test
{
    [TestClass]
    public class LevelUpUriBuilderTests
    {
        [TestMethod]
        public void DefaultIsProduction()
        {
            var uriBuilder = new LevelUpUriBuilder();
            uriBuilder.Environment.Should().Be(LevelUpEnvironment.Production);
        }

        [TestMethod]
        public void CanSetEnvironment()
        {
            var uriBuilder = new LevelUpUriBuilder(LevelUpEnvironment.Sandbox);
            Assert.AreEqual(LevelUpEnvironment.Sandbox, uriBuilder.Environment);
            uriBuilder.SetEnvironment(LevelUpEnvironment.Staging);
            uriBuilder.Environment.Should().Be(LevelUpEnvironment.Staging);
            uriBuilder.SetEnvironment(LevelUpEnvironment.Production);
            uriBuilder.Environment.Should().Be(LevelUpEnvironment.Production);
        }

        [TestMethod]
        public void CanSetApiVersion()
        {
            var uriBuilder = new LevelUpUriBuilder().SetApiVersion(LevelUpApiVersion.v14);
            uriBuilder.ApiVersion.Should().Be(LevelUpApiVersion.v14);
            uriBuilder.SetApiVersion(LevelUpApiVersion.v15);
            uriBuilder.ApiVersion.Should().Be(LevelUpApiVersion.v15);
        }

        [TestMethod]
        public void IncludePortNumber()
        {
            const string sslPort = "443";
            var uriBuilder = new LevelUpUriBuilder().SetPath("ThisIsATest");
            uriBuilder.Environment.Should().Be(LevelUpEnvironment.Production);
            uriBuilder.Build().Should().NotContain(sslPort);
            uriBuilder.Build(false).Should().NotContain(sslPort);
            uriBuilder.Build(true).Should().Contain(sslPort);
        }

        [TestMethod]
        public void AppendQuery()
        {
            const string query = "test=yes";
            const string query1 = "level=up";
            var uriBuilder = new LevelUpUriBuilder().SetPath("ThisIsATest").AppendQuery("test", "yes").AppendQuery("level", "up");
            uriBuilder.Environment.Should().Be(LevelUpEnvironment.Production);
            string uri = uriBuilder.Build();

            uri.Should().Contain("?" + query);
            uri.Should().Contain("&" + query1);

            var uriBuilder1 = new LevelUpUriBuilder().SetPath("ThisIsATest").AppendQuery("test", "yes").AppendQuery("test", "yes");
            string uri1 = uriBuilder1.Build();
            uri1.Should().Contain("?" + query);
            uri1.Should().NotContain("&" + query);
        }

    }
}
