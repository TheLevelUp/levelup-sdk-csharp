#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="AccessToken.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
    /// Class representing a LevelUp access token response
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [ObjectEnvelope("access_token")]
    [JsonConverter(typeof(EnvelopeSerializer))]
    public class AccessToken : Response
    {
        /// <summary>
        /// Private constructor for deserialization
        /// </summary>
        private AccessToken() { }

        public AccessToken(string token, int userId, int? merchantId)
        {
            Token = token;
            UserId = userId;
            MerchantId = merchantId;
        }

        /// <summary>
        /// The LevelUp access token. This is required for further interaction with the LevelUp API
        /// </summary>
        [JsonProperty(PropertyName = "token")]
        public string Token { get; private set; }
        
        /// <summary>
        /// Identifier for the requesting user account
        /// </summary>
        [JsonProperty(PropertyName = "user_id")]
        public int UserId { get; private set; }

        /// <summary>
        /// Merchant identifier. This will have a value if the requesting account is a LevelUp merchant
        /// </summary>
        [JsonProperty(PropertyName = "merchant_id")]
        public int? MerchantId { get; private set; }

        /// <summary>
        /// Returns true if the requesting account is a LevelUp merchant
        /// </summary>
        [JsonIgnore]
        public bool IsMerchant { get { return MerchantId.HasValue && MerchantId.Value > 0; } }
    }
}
