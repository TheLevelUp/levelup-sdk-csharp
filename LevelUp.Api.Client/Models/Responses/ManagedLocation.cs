#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="ManagedLocation.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
    /// Class representing a managed LevelUp merchant location.  Note that this location schema
    /// is different from the one in a standard merchant location query.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [ObjectEnvelope("location")]
    [JsonConverter(typeof(EnvelopeSerializer))]
    public class ManagedLocation : Response
    {
        /// <summary>
        /// Private constructor for deserialization
        /// </summary>
        private ManagedLocation() { }

        public ManagedLocation(int locationId, int merchantId, string name, string merchantName, 
            string address, string tipPreference)
        {
            LocationId = locationId;
            Name = name;
            TipPreference = tipPreference;
        }

        /// <summary>
        /// Identification number for the location
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public int LocationId { get; private set; }

        /// <summary>
        /// Identification number for the merchant
        /// </summary>
        [JsonProperty(PropertyName = "merchant_id")]
        public int MerchantId { get; private set; }

        /// <summary>
        /// Address of the location
        /// </summary>
        [JsonProperty(PropertyName = "address")]
        public string Address { get; private set; }

        /// <summary>
        /// Name of the location
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; private set; }

        /// <summary>
        /// Name of the merchant
        /// </summary>
        [JsonProperty(PropertyName = "merchant_name")]
        public string MerchantName { get; private set; }

        /// <summary>
        /// Tip preference for the location
        /// </summary>
        [JsonProperty(PropertyName = "tip_preference")]
        public string TipPreference { get; private set; }
    }
}
