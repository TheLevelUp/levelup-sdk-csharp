#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="GetRemoteCheckDataResponse.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
    [JsonObject(MemberSerialization.OptIn)]
    [ObjectEnvelope("check")]
    [JsonConverter(typeof(EnvelopeSerializer))]
    public class GetRemoteCheckDataResponse : Response
    {
        /// <summary>
        /// Private constructor for deserialization
        /// </summary>
        private GetRemoteCheckDataResponse()
        {
            PendingOrderContainer = new PendingOrderResponseContainer(null, null);
        }

        public GetRemoteCheckDataResponse(string orderIdentifier, int? discountToApply)
        {
            PendingOrderContainer = new PendingOrderResponseContainer(orderIdentifier, discountToApply);
        }

        [JsonObject(MemberSerialization.OptIn)]
        private class PendingOrderResponseContainer
        {
            public PendingOrderResponseContainer(string orderIdentifier, int? discountToApply)
            {
                OrderIdentifier = orderIdentifier;
                DiscountToApply = discountToApply;
            }

            [JsonProperty(PropertyName = "uuid")]
            public string OrderIdentifier { get; private set; }

            [JsonProperty(PropertyName = "discount_amount")]
            public int? DiscountToApply { get; private set; }
        }

        [JsonIgnore]
        public string OrderIdentifier { get { return PendingOrderContainer.OrderIdentifier; } }

        [JsonIgnore]
        public int? DiscountToApply { get { return PendingOrderContainer.DiscountToApply; } }

        [JsonProperty(PropertyName = "pending_order")]
        private PendingOrderResponseContainer PendingOrderContainer { get; set; }
    }
}
