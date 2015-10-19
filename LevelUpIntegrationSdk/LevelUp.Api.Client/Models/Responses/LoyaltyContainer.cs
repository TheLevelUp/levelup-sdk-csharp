//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="LoyaltyContainer.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using System.Globalization;
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Responses
{
    /// <summary>
    /// Class to hold the results from the loyalty end point
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class LoyaltyContainer
    {
        [JsonProperty(PropertyName = "merchant_earn_amount")]
        public int MerchantEarnAmount { get; set; }

        [JsonProperty(PropertyName = "merchant_id")]
        public int MerchantId { get; set; }

        [JsonProperty(PropertyName = "merchant_loyalty_enabled")]
        public bool MerchantLoyaltyEnabled { get; set; }

        [JsonProperty(PropertyName = "merchant_spend_amount")]
        public int MerchantSpendAmount { get; set; }

        [JsonProperty(PropertyName = "orders_count")]
        public int OrdersCount { get; set; }

        [JsonProperty(PropertyName = "potential_credit_amount")]
        public int PotentialCreditAmount { get; set; }

        [JsonProperty(PropertyName = "progress_percentage")]
        public int ProgressPercentage { get; set; }

        [JsonProperty(PropertyName = "savings_amount")]
        public int SavingsAmount { get; set; }

        [JsonProperty(PropertyName = "spend_remaining_amount")]
        public int SpendRemainingAmount { get; set; }

        [JsonProperty(PropertyName = "total_volume_amount")]
        public int TotalVolumeAmount { get; set; }

        [JsonProperty(PropertyName = "user_id")]
        public int UserId { get; set; }

        public override string ToString()
        {
            return string.Format(new CultureInfo("en-US"),
                        "MerchantEarnAmount: {0}{1}" +
                        "MerchantId: {2}{1}" +
                        "MerchantLoyaltyEnabled: {3}{1}" +
                        "MerchantSpendAmount: {4}{1}" +
                        "PotentialCreditAmount: {5}{1}" +
                        "ProgressPercentage: {6}{1}" +
                        "SavingsAmount: {7}{1}" +
                        "SpendRemainingAmount: {8}{1}" +
                        "TotalVolumeAmount: {9}{1}" +
                        "UserId: {10}{1}",
                        MerchantEarnAmount,
                        Environment.NewLine,
                        MerchantId,
                        MerchantLoyaltyEnabled ? "Y" : "N",
                        MerchantSpendAmount,
                        PotentialCreditAmount,
                        ProgressPercentage,
                        SavingsAmount,
                        SpendRemainingAmount,
                        TotalVolumeAmount,
                        UserId);
        }
    }
}
