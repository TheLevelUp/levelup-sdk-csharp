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
