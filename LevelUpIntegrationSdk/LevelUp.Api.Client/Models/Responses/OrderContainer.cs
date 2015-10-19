//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="OrderContainer.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using Newtonsoft.Json;
using System;
using System.Globalization;

namespace LevelUp.Api.Client.Models.Responses
{
    [JsonObject]
    internal class OrderContainer : OrderContainerBase
    {
        [JsonProperty(PropertyName = "created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty(PropertyName = "merchant_funded_credit_amount")]
        public int MerchantFundedCreditAmount { get; set; }

        [JsonProperty(PropertyName = "earn_amount")]
        public int EarnAmount { get; set; }

        [JsonProperty(PropertyName = "loyalty_id")]
        public int LoyaltyId { get; set; }

        [JsonProperty(PropertyName = "refunded_at")]
        public string RefundedAt { get; set; }

        [JsonProperty(PropertyName = "transacted_at")]
        public string TransactedAt { get; set; }

        [JsonProperty(PropertyName = "location_id")]
        public int LocationId { get; set; }

        [JsonProperty(PropertyName = "user_display_name")]
        public string UserName { get; set; }

        public override string ToString()
        {
            return string.Format(new CultureInfo("en-Us"),
                                 "{0}" +
                                 "User name: {1}{2}" +
                                 "Created at: {3}{2}" +
                                 "Time of transaction: {4}{2}" +
                                 "Time of refund: {5}{2}" +
                                 "Merchant funded credit amount: {6}{2}" +
                                 "Amount earned: {7}{2}" +
                                 "Loyalty id: {8}{2}" +
                                 "Location of transaction: {9}{2}",
                                 base.ToString(),
                                 UserName,
                                 Environment.NewLine,
                                 CreatedAt,
                                 TransactedAt ?? "NULL",
                                 RefundedAt ?? "NULL",
                                 MerchantFundedCreditAmount,
                                 EarnAmount,
                                 LoyaltyId,
                                 LocationId);
        }
    }
}
