using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace LevelUp.Api.Utilities
{
    public static class IpAddress
    {
        private const int MAX_IP_RESOLUTION_ATTEMPTS = 3;

        public enum IpAddressType
        {
            Ipv4,
            Ipv6
        };

        private static readonly Dictionary<IpAddressType, AddressFamily> AddressMapping =
            new Dictionary<IpAddressType, AddressFamily>
                {
                    {IpAddressType.Ipv4, AddressFamily.InterNetwork},
                    {IpAddressType.Ipv6, AddressFamily.InterNetworkV6},
                };


        /// <summary>
        /// Retrieves the current Ipv4 of Ipv6 ipaddress for the machine the code is running on
        /// </summary>
        public static string GetCurrentIpAddress(IpAddressType addressType = IpAddressType.Ipv4)
        {
            var ipAddresses = GetCurrentIpAddresses(addressType);
            return ipAddresses.Length > 0 ? ipAddresses[0] : null;
        }

        /// <summary>
        /// Returns a list of all IpAddresses currently available on the machine
        /// </summary>
        public static string[] GetCurrentIpAddresses(IpAddressType addressType = IpAddressType.Ipv4)
        {
            IPHostEntry hostEntry = null;
            IPAddress[] ipAddresses = null;

            int tryCount = 0;

            do
            {
                try
                {
                    hostEntry = Dns.GetHostEntry(Dns.GetHostName());
                }
                catch (SocketException)
                {
                    // Eat socket exception
                    System.Threading.Thread.Sleep(100); //Sleep for a short time then retry upto the max
                }
            } while (null == hostEntry && ++tryCount < MAX_IP_RESOLUTION_ATTEMPTS);


            if (null != hostEntry && null != hostEntry.AddressList)
            {
                ipAddresses = Array.FindAll<IPAddress>(hostEntry.AddressList,
                                                       address =>
                                                       address.AddressFamily.Equals(AddressMapping[addressType]));
            }

            return Array.ConvertAll(ipAddresses ?? new IPAddress[0], (a) => a.ToString());
        }
    }
}
