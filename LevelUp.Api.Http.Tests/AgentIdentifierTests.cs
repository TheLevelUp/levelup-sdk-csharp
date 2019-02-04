#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="AgentIdentifierTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using System;
using FluentAssertions;
using NUnit.Framework;

namespace LevelUp.Api.Http.Tests
{
    [TestFixture]
    public class AgentIdentifierTests
    {
        [TestFixture]
        public class ToStringTests
        {
            private readonly string _osVersionString = string.Format("{0}", Environment.OSVersion.VersionString);

            [Test]
            public void AllPropertiesNull()
            {
                var agentId = new AgentIdentifier(null, null, null, null);

                agentId.ToString().Should().Be("[" + _osVersionString + "]");
            }

            [Test]
            public void OnlyCompanyName()
            {
                const string companyName = "Test Co. LLC. Inc";
                var agentId = new AgentIdentifier(companyName, null, null, null);

                agentId.ToString().Should().Be(string.Format("{0} [{1}]", companyName, _osVersionString));
            }

            [Test]
            public void OnlyProductName()
            {
                const string productName = "Whirligig";
                var agentId = new AgentIdentifier(null, productName, null, null);

                agentId.ToString().Should().Be(string.Format("{0} [{1}]", productName, _osVersionString));
            }

            [Test]
            public void ProductNameAndVersion()
            {
                const string productName = "Whirligog";
                const string version = "Mark V";

                var agentId = new AgentIdentifier(null, productName, version, null);

                agentId.ToString().Should().Be(string.Format("{0}/{1} [{2}]", productName, version, _osVersionString));
            }

            [Test]
            public void OnlyOsName()
            {
                const string osName = "TestOs";
                var agentId = new AgentIdentifier(null, null, null, osName);

                agentId.ToString().Should().Be(string.Format("[{0} | {1}]", osName, _osVersionString));
            }

            [Test]
            public void NoVersion()
            {
                const string companyName = "Test Co. Inc. LLC.";
                const string productName = "Whirligoogle";
                const string osName = "TestOs";
                var agentId = new AgentIdentifier(companyName, productName, null, osName);

                agentId.ToString().Should().Be(string.Format("{0}-{1} [{2} | {3}]", 
                    companyName, 
                    productName, 
                    osName, 
                    _osVersionString));
            }

            [Test]
            public void AllSpecified()
            {
                const string companyName = "Test Co. Inc. LLC.";
                const string productName = "Whirligoogle";
                const string productVersion = "v1.2.3";
                const string osName = "TestOs";
                var agentId = new AgentIdentifier(companyName, productName, productVersion, osName);

                agentId.ToString().Should().Be(string.Format("{0}-{1}/{2} [{3} | {4}]",
                    companyName,
                    productName,
                    productVersion,
                    osName,
                    _osVersionString));
            }
        }
    }
}
