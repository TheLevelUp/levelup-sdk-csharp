//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="MerchantFundedCreditResponseContainer.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using System.Globalization;
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Responses
{
    /// <summary>
    /// Class representing a detail response from a Merchant Funded Credit Query
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class MerchantFundedCreditResponseContainer
    {
        [JsonProperty(PropertyName = "discount_amount")]
        public int? DiscountAmount { get; set; }

        [JsonProperty(PropertyName = "gift_card_amount")]
        public int? GiftCardAmount { get; set; }

        [JsonProperty(PropertyName = "total_amount")]
        public int? TotalAmount { get; set; }

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
