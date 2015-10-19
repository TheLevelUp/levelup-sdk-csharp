//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="UserAddressContainer.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using System;
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Responses
{
    /// <summary>
    /// Class to hold the results from the user addresses end point
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class UserAddressContainer
    {
        [JsonProperty(PropertyName = "id")]
        public virtual int Id { get; set; }

        [JsonProperty(PropertyName = "address_type")]
        public virtual string AddressType { get; set; }

        [JsonProperty(PropertyName = "extended_address")]
        public virtual string ExtendedAddress { get; set; }

        [JsonProperty(PropertyName = "locality")]
        public virtual string Locality { get; set; }

        [JsonProperty(PropertyName = "postal_code")]
        public virtual string PostalCode { get; set; }

        [JsonProperty(PropertyName = "region")]
        public virtual string Region { get; set; }

        [JsonProperty(PropertyName = "street_address")]
        public virtual string StreetAddress { get; set; }

        public override string ToString()
        {
            return string.Format(Constants.UsCultureInfo,
                                 "Id: {0}{1}" +
                                 "AddressType: {2}{1}" +
                                 "ExtendedAddress: {3}{1}" +
                                 "Locality: {4}{1}" +
                                 "PostalCode: {5}{1}" +
                                 "Region: {6}{1}" +
                                 "StreetAddress: {7}{1}",
                                 Id,
                                 Environment.NewLine,
                                 AddressType,
                                 ExtendedAddress,
                                 Locality,
                                 PostalCode,
                                 Region,
                                 StreetAddress);
        }
    }
}
