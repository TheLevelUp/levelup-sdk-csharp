#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="AccessTokenRequestBody.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

namespace LevelUp.Api.Client.Models.Requests
{
    /// <summary>
    /// Class representing a LevelUp merchant funded credit request object
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [ObjectEnvelope("merchant_funded_credit")]
    [JsonConverter(typeof(EnvelopeSerializer))]
    public class GrantMerchantFundedCreditRequestBody
    {
        private GrantMerchantFundedCreditRequestBody()
        {
            // Private constructor for deserialization
        }

        public GrantMerchantFundedCreditRequestBody(
            string email, 
            int durationInSeconds, 
            int merchantId, 
            string message,
            int valueAmount,
            bool global)
        {
            Email = email;
            DurationInSeconds = durationInSeconds;
            MerchantId = merchantId;
            Message = message;
            ValueAmount = valueAmount;
            Global = global;
        }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; private set; }

        [JsonProperty(PropertyName = "duration_in_seconds")]
        public int DurationInSeconds { get; private set; }

        [JsonProperty(PropertyName = "merchant_id")]
        public int MerchantId { get; private set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; private set; }

        [JsonProperty(PropertyName = "value_amount")]
        public int ValueAmount { get; private set; }

        [JsonProperty(PropertyName = "global")]
        public bool Global { get; private set; }
    }
}
