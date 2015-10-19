//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="UserAddress.cs" company="SCVNGR, Inc. d/b/a LevelUp">
//   Copyright(c) 2014 SCVNGR, Inc. d/b/a LevelUp. All rights reserved.
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

using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Responses
{
    /// <summary>
    /// Class representing a LevelUp user's address
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class UserAddress
    {
        /// <summary>
        /// The id of the address
        /// </summary>
        public virtual int Id { get { return UserAddressContainer.Id; } }

        /// <summary>
        /// The type of address
        /// </summary>
        public virtual string AddressType { get { return UserAddressContainer.AddressType; } }

        /// <summary>
        /// Street address line 2
        /// </summary>
        public virtual string ExtendedAddress { get { return UserAddressContainer.ExtendedAddress; } }

        /// <summary>
        /// The locality where the address is located
        /// </summary>
        public virtual string Locality { get { return UserAddressContainer.Locality; } }

        /// <summary>
        /// The postal code associated with the address
        /// </summary>
        public virtual string PostalCode { get { return UserAddressContainer.PostalCode; } }

        /// <summary>
        /// The region the address is in
        /// </summary>
        public virtual string Region { get { return UserAddressContainer.Region; } }

        /// <summary>
        /// The street address of the address
        /// </summary>
        public virtual string StreetAddress { get { return UserAddressContainer.StreetAddress; } }

        [JsonProperty(PropertyName = "user_address")]
        private UserAddressContainer UserAddressContainer { get; set; }

        public override string ToString()
        {
            return UserAddressContainer.ToString();
        }
    }
}
