//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="IpAddressTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
//   Copyright(c) 2015 SCVNGR, Inc. d/b/a LevelUp. All rights reserved.
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

using System.Net;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LevelUp.Api.Utilities.Test
{
    [TestClass]
    public class IpAddressTests
    {
        [TestMethod]
        public void GetCurrentIpAddress_DefaultIsV4()
        {
            VerifyIpV4Address(IpAddress.GetCurrentIpAddress());
        }

        [TestMethod]
        public void GetCurrentIpV4Address()
        {
            VerifyIpV4Address(IpAddress.GetCurrentIpAddress(IpAddress.IpAddressType.Ipv4));
        }

        [TestMethod]
        public void GetCurrentIpV6Address()
        {
            VerifyIpV6Address(IpAddress.GetCurrentIpAddress(IpAddress.IpAddressType.Ipv6));
        }

        private void VerifyIpV4Address(string ipV4Address)
        {
            IPAddress parsed;
            IPAddress.TryParse(ipV4Address, out parsed).Should().BeTrue();
            parsed.AddressFamily.Should().Be(System.Net.Sockets.AddressFamily.InterNetwork);
        }

        private void VerifyIpV6Address(string ipV6Address)
        {
            IPAddress parsed;
            IPAddress.TryParse(ipV6Address, out parsed).Should().BeTrue();
            parsed.AddressFamily.Should().Be(System.Net.Sockets.AddressFamily.InterNetworkV6);
        }
    }
}
