#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="UserAddress.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using JsonEnvelopeSerializer;
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Responses
{
    /// <summary>
    /// Class representing a LevelUp user's address
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [ObjectEnvelope("user_address")]
    [JsonConverter(typeof(EnvelopeSerializer))]
    public class UserAddress : Response
    {
        /// <summary>
        /// Private constructor for deserialization
        /// </summary>
        private UserAddress() { }

        public UserAddress(string addressType, string extendedAddress, string locality, string postalCode,
            string region, string streetAddress)
        {
            AddressType = addressType;
            ExtendedAddress = extendedAddress;
            Locality = locality;
            PostalCode = postalCode;
            Region = region;
            StreetAddress = streetAddress;
        }

        /// <summary>
        /// The id of the address
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public virtual int Id { get; private set; }

        /// <summary>
        /// The type of address
        /// </summary>
        [JsonProperty(PropertyName = "address_type")]
        public virtual string AddressType { get; private set; }

        /// <summary>
        /// Street address line 2
        /// </summary>
        [JsonProperty(PropertyName = "extended_address")]
        public virtual string ExtendedAddress { get; private set; }

        /// <summary>
        /// The locality where the address is located
        /// </summary>
        [JsonProperty(PropertyName = "locality")]
        public virtual string Locality { get; private set; }

        /// <summary>
        /// The postal code associated with the address
        /// </summary>
        [JsonProperty(PropertyName = "postal_code")]
        public virtual string PostalCode { get; private set; }

        /// <summary>
        /// The region the address is in
        /// </summary>
        [JsonProperty(PropertyName = "region")]
        public virtual string Region { get; private set; }

        /// <summary>
        /// The street address of the address
        /// </summary>
        [JsonProperty(PropertyName = "street_address")]
        public virtual string StreetAddress { get; private set; }
    }
}
