#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="DetachedRefundResponse.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using System;
using JsonEnvelopeSerializer;
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Responses
{
    /// <summary>
    /// Class representing a LevelUp response to a detached refund request
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [ObjectEnvelope("detached_refund")]
    [JsonConverter(typeof(EnvelopeSerializer))]
    public class DetachedRefundResponse : Response
    {
        /// <summary>
        /// Private constructor for deserialization
        /// </summary>
        private DetachedRefundResponse() { }

        public DetachedRefundResponse(string cashier, int creditAmountCents, string customerFacingReason, string identifier, 
            string internalReason, int locationId, string register, DateTime refundedAt, int userId)
        {
            Cashier = cashier;
            CreditAmountCents = creditAmountCents;
            CustomerFacingReason = customerFacingReason;
            Identifier = identifier;
            InternalReason = internalReason;
            LocationId = locationId;
            Register = register;
            RefundedAt = refundedAt;
            UserId = userId;
        }

        [JsonProperty(PropertyName = "cashier")]
        public string Cashier { get; private set; }

        [JsonProperty(PropertyName = "credit_amount")]
        public int CreditAmountCents { get; private set; }

        [JsonProperty(PropertyName = "customer_facing_reason")]
        public string CustomerFacingReason { get; private set; }

        [JsonProperty(PropertyName = "identifier_from_merchant")]
        public string Identifier { get; private set; }

        [JsonProperty(PropertyName = "internal_reason")]
        public string InternalReason { get; private set; }

        [JsonProperty(PropertyName = "location_id")]
        public int LocationId { get; private set; }

        [JsonProperty(PropertyName = "register")]
        public string Register { get; private set; }

        [JsonProperty(PropertyName = "refunded_at")]
        public DateTime RefundedAt { get; private set; }

        [JsonProperty(PropertyName = "loyalty_id")]
        public int UserId { get; private set; }
    }
}
