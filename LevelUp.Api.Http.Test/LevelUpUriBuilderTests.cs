#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="LevelUpUriBuilderTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
//   Copyright(c) 2016 SCVNGR, Inc. d/b/a LevelUp. All rights reserved.
// </copyright>
// <license publisher="Apache Software Foundation" date="January 2004" version="2.0">
//   Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except
//   in compliance with the License. You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software distributed under the License
//   is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
//   or implied. See the License for the specific language governing permissions and limitations under
//   the License.
// </license>
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
#endregion

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LevelUp.Api.Http.Test
{
    [TestClass]
    public class LevelUpUriBuilderTests
    {
        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.UnitTests)]
        public void DefaultIsSandbox()
        {
            var uriBuilder = new LevelUpUriBuilder();
            uriBuilder.Environment.Should().Be(LevelUpEnvironment.Sandbox);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.UnitTests)]
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
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.UnitTests)]
        public void CanSetApiVersion()
        {
            var uriBuilder = new LevelUpUriBuilder().SetApiVersion(LevelUpApiVersion.v14);
            uriBuilder.ApiVersion.Should().Be(LevelUpApiVersion.v14);
            uriBuilder.SetApiVersion(LevelUpApiVersion.v15);
            uriBuilder.ApiVersion.Should().Be(LevelUpApiVersion.v15);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.UnitTests)]
        public void IncludePortNumber()
        {
            const string sslPort = "443";
            var uriBuilder = new LevelUpUriBuilder().SetPath("ThisIsATest");
            uriBuilder.Environment.Should().Be(LevelUpEnvironment.Sandbox);
            uriBuilder.Build().Should().NotContain(sslPort);
            uriBuilder.Build(false).Should().NotContain(sslPort);
            uriBuilder.Build(true).Should().Contain(sslPort);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.UnitTests)]
        public void AppendQuery()
        {
            const string query = "test=yes";
            const string query1 = "level=up";
            var uriBuilder = new LevelUpUriBuilder().SetPath("ThisIsATest").AppendQuery("test", "yes").AppendQuery("level", "up");
            uriBuilder.Environment.Should().Be(LevelUpEnvironment.Sandbox);
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
