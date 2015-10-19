//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="IpAddress.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
