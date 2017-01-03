#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="MerchantFundedCreditResponse.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using System.Globalization;
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Responses
{
    /// <summary>
    /// Class representing a detail response from a Merchant Funded Credit Query
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [LevelUpSerializableModel("merchant_funded_credit")]
    [JsonConverter(typeof(LevelUpModelSerializer))]
    public class MerchantFundedCreditResponse : IResponse
    {
        /// <summary>
        /// Private constructor for deserialization
        /// </summary>
        private MerchantFundedCreditResponse() { }

        /// <summary>
        /// Internal constructor for testing
        /// </summary>
        internal MerchantFundedCreditResponse(int? discountAmount, int? giftCardAmount, int? totalAmount)
        {
            DiscountAmountInternal = discountAmount;
            GiftCardAmountInternal = giftCardAmount;
            TotalAmountInternal = totalAmount;
        }

        /// <summary>
        /// The amount of available merchant funded credit available for discount in cents
        /// </summary>
        [JsonIgnore]
        public int DiscountAmount { get { return DiscountAmountInternal.GetValueOrDefault(0); } }

        /// <summary>
        /// The amount of available gift card credit in cents
        /// </summary>
        [JsonIgnore]
        public int GiftCardAmount { get { return GiftCardAmountInternal.GetValueOrDefault(0); } }

        /// <summary>
        /// The total amount of credit available to the customer in cents (discount + gift card)
        /// </summary>
        [JsonIgnore]
        public int TotalAmount { get { return TotalAmountInternal.GetValueOrDefault(0); } }


        [JsonProperty(PropertyName = "discount_amount")]
        private int? DiscountAmountInternal { get; set; }

        [JsonProperty(PropertyName = "gift_card_amount")]
        private int? GiftCardAmountInternal { get; set; }

        [JsonProperty(PropertyName = "total_amount")]
        private int? TotalAmountInternal { get; set; }

        public override string ToString()
        {
            return string.Format(new CultureInfo("en-US"),
                                 "Discount Amount: {1}{0}" +
                                 "Gift Card Amount: {2}{0}" +
                                 "Total Amount: {3}{0}",
                                 Environment.NewLine,
                                 DiscountAmount,
                                 GiftCardAmount,
                                 TotalAmount);
        }

    }
}
