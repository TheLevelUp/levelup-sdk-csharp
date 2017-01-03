#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="FinalizeRemoteCheckResponse.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Responses
{
    [JsonObject(MemberSerialization.OptIn)]
    [LevelUpSerializableModel("order")]
    [JsonConverter(typeof(LevelUpModelSerializer))]
    public class FinalizeRemoteCheckResponse : IResponse
    {
        /// <summary>
        /// Private constructor for deserialization
        /// </summary>
        private FinalizeRemoteCheckResponse() { }

        /// <summary>
        /// Internal constructor for testing
        /// </summary>
        internal FinalizeRemoteCheckResponse(int spendAmount, int tipAmount, int totalAmount, string orderIdentifier,
            int giftCardCreditTotalAmount, int giftCardCreditTipAmount)
        {
            SpendAmount = spendAmount;
            TipAmount = tipAmount;
            TotalAmount = totalAmount;
            OrderIdentifier = orderIdentifier;
            GiftCardCreditTotalAmount = giftCardCreditTotalAmount;
            GiftCardCreditTipAmount = giftCardCreditTipAmount;
        }

        [JsonProperty(PropertyName = "spend_amount")]
        public int SpendAmount { get; private set; }

        [JsonProperty(PropertyName = "tip_amount")]
        public int TipAmount { get; private set; }

        [JsonProperty(PropertyName = "total_amount")]
        public int TotalAmount { get; private set; }

        [JsonProperty(PropertyName = "uuid")]
        public string OrderIdentifier { get; private set; }

        [JsonProperty(PropertyName = "gift_card_total_amount")]
        public int GiftCardCreditTotalAmount { get; private set; }

        [JsonProperty(PropertyName = "gift_card_tip_amount")]
        public int GiftCardCreditTipAmount { get; private set; }
        
        public override string ToString()
        {
            return string.Format("Id: {0}{1}" +
                                 "Spent: {2}¢{1}" +
                                 "Tip: {3}¢{1}" +
                                 "Total: {4}¢{1}" +
                                 "Total Gift Card Credit Applied: {5}¢{1}" +
                                 "Gift Card Tip Amount: {6}¢{1}",
                OrderIdentifier,
                Environment.NewLine,
                SpendAmount,
                TipAmount,
                TotalAmount,
                GiftCardCreditTotalAmount,
                GiftCardCreditTipAmount);
        }
    }
}
