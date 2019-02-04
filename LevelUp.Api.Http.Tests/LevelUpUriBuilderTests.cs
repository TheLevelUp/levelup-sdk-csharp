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
using NUnit.Framework;

namespace LevelUp.Api.Http.Tests
{
    [TestFixture]
    public class LevelUpUriBuilderTests
    {
        [Test]
        
        public void CanSetApiVersion()
        {
            var uriBuilder = new LevelUpUriBuilder(LevelUpEnvironment.Sandbox).SetApiVersion(LevelUpApiVersion.v14);
            uriBuilder.ApiVersion.Should().Be(LevelUpApiVersion.v14);
            uriBuilder.SetApiVersion(LevelUpApiVersion.v15);
            uriBuilder.ApiVersion.Should().Be(LevelUpApiVersion.v15);
        }

        [Test]
        
        public void IncludePortNumber()
        {
            const string sslPort = "443";
            var uriBuilder = new LevelUpUriBuilder(LevelUpEnvironment.Sandbox).SetPath("ThisIsATest");
            uriBuilder.Environment.Should().Be(LevelUpEnvironment.Sandbox);
            uriBuilder.Build().Should().NotContain(sslPort);
            uriBuilder.Build(false).Should().NotContain(sslPort);
            uriBuilder.Build(true).Should().Contain(sslPort);
        }

        [Test]
        
        public void AppendQuery()
        {
            const string query = "test=yes";
            const string query1 = "level=up";
            var uriBuilder = new LevelUpUriBuilder(LevelUpEnvironment.Sandbox).SetPath("ThisIsATest").AppendQuery("test", "yes").AppendQuery("level", "up");
            uriBuilder.Environment.Should().Be(LevelUpEnvironment.Sandbox);
            string uri = uriBuilder.Build();

            uri.Should().Contain("?" + query);
            uri.Should().Contain("&" + query1);

            var uriBuilder1 = new LevelUpUriBuilder(LevelUpEnvironment.Sandbox).SetPath("ThisIsATest").AppendQuery("test", "yes").AppendQuery("test", "yes");
            string uri1 = uriBuilder1.Build();
            uri1.Should().Contain("?" + query);
            uri1.Should().NotContain("&" + query);
        }

    }
}
