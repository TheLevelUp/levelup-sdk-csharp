#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="CompletedOrderResponse.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
    /// Class representing a response to an order request made to LevelUp
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [ObjectEnvelope("order")]
    [JsonConverter(typeof(EnvelopeSerializer))]
    public class CompletedOrderResponse : Response
    {
        /// <summary>
        /// Private constructor for deserialization
        /// </summary>
        private CompletedOrderResponse() { }

        public CompletedOrderResponse(int spendAmount, int tipAmount, int total, string orderIdentifier,
            int giftCardTotalAmount, int giftCardTipAmount)
        {
            SpendAmount = spendAmount;
            TipAmount = tipAmount;
            Total = total;
            OrderIdentifier = orderIdentifier;
            GiftCardTotalAmount = giftCardTotalAmount;
            GiftCardTipAmount = giftCardTipAmount;
        }

        /// <summary>
        /// Amount in cents spent on purchase not including tip. 
        /// </summary>
        [JsonProperty(PropertyName = "spend_amount")]
        public int SpendAmount { get; private set; }

        /// <summary>
        /// Amount in cents spent as tip/gratuity
        /// </summary>
        [JsonProperty(PropertyName = "tip_amount")]
        public int TipAmount { get; private set; }

        /// <summary>
        /// Total amount in cents spent on purchase plus tip/gratuity
        /// </summary>
        [JsonProperty(PropertyName = "total_amount")]
        public int Total { get; private set; }

        /// <summary>
        /// ProposedOrderIdentifier that uniquely identifies the order
        /// </summary>
        [JsonProperty(PropertyName = "uuid")]
        public string OrderIdentifier { get; private set; }

        /// <summary>
        /// Total amount of merchant-funded gift card credit applied (this includes tip)
        /// </summary>
        [JsonProperty(PropertyName = "gift_card_total_amount")]
        public int GiftCardTotalAmount { get; private set; }

        /// <summary>
        /// Amount of the gift card total that was applied to the tip
        /// </summary>
        [JsonProperty(PropertyName = "gift_card_tip_amount")]
        public int GiftCardTipAmount { get; private set; }
    }
}
