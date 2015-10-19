//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="DetachedRefundResponse.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Responses
{
    /// <summary>
    /// Class representing a LevelUp response to a detached refund request
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class DetachedRefundResponse
    {
        public DetachedRefundResponse()
        {
            DetachedRefundResponseContainer = new DetachedRefundResponseContainer();
        }

        public string Cashier { get { return DetachedRefundResponseContainer.Cashier; } }

        public int CreditAmountCents { get { return DetachedRefundResponseContainer.CreditAmountCents; } }

        public string CustomerFacingReason { get { return DetachedRefundResponseContainer.CustomerFacingReason; } }

        public string Identifier { get { return DetachedRefundResponseContainer.Identifier; } }

        public string InternalReason { get { return DetachedRefundResponseContainer.InternalReason; } }

        public int LocationId { get { return DetachedRefundResponseContainer.LocationId; } }

        public string Register { get { return DetachedRefundResponseContainer.Register; } }

        public DateTime RefundedAt {get { return DetachedRefundResponseContainer.RefundedAt; }}

        public int UserId {get { return DetachedRefundResponseContainer.UserId; }}

        /// <summary>
        /// Use this container to aid in correct JSON serialization
        /// </summary>
        [JsonProperty(PropertyName = "detached_refund")]
        private DetachedRefundResponseContainer DetachedRefundResponseContainer { get; set; }

        public override string ToString()
        {
            return DetachedRefundResponseContainer.ToString();
        }
    }
}
