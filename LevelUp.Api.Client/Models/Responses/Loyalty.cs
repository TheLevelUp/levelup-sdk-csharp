#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="Loyalty.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
    /// Class representing a LevelUp loyalty between a merchant and a user
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [ObjectEnvelope("loyalty")]
    [JsonConverter(typeof(EnvelopeSerializer))]
    public class Loyalty : Response
    {
        /// <summary>
        /// Private constructor for deserialization
        /// </summary>
        private Loyalty() { }

        public Loyalty(int merchantEarnAmount, int merchantId, bool merchantloyaltyEnabled, int merchantSpendAmount,
            int ordersCount, int potentialCreditAmount, decimal progressPercentage, int savingsAmount,
            int spendRemainingAmount, int totalVolumeAmount)
        {
            MerchantEarnAmount = merchantEarnAmount;
            MerchantId = merchantId;
            MerchantLoyaltyEnabled = merchantloyaltyEnabled;
            MerchantSpendAmount = merchantSpendAmount;
            OrdersCount = ordersCount;
            PotentialCreditAmount = potentialCreditAmount;
            ProgressPercentage = progressPercentage;
            SavingsAmount = savingsAmount;
            SpendRemainingAmount = spendRemainingAmount;
            TotalVolumeAmount = totalVolumeAmount;
        }

        /// <summary>
        /// The merchant's loyalty reward
        /// </summary>
        [JsonProperty(PropertyName = "merchant_earn_amount")]
        public int MerchantEarnAmount { get; private set; }

        /// <summary>
        /// The ID of the merchant
        /// </summary>
        [JsonProperty(PropertyName = "merchant_id")]
        public int MerchantId { get; private set; }

        /// <summary>
        /// Whether the merchant is running a loyalty program
        /// </summary>
        [JsonProperty(PropertyName = "merchant_loyalty_enabled")]
        public bool MerchantLoyaltyEnabled { get; private set; }

        /// <summary>
        /// The merchant's loyalty reward goal
        /// </summary>
        [JsonProperty(PropertyName = "merchant_spend_amount")]
        public int MerchantSpendAmount { get; private set; }

        /// <summary>
        /// The total number of orders ever placed by the user at the merchant
        /// </summary>
        [JsonProperty(PropertyName = "orders_count")]
        public int OrdersCount { get; private set; }

        /// <summary>
        /// The amount of outstanding credit the user could have at this merchant. This value includes credit 
        /// already gained by claiming campaigns, and potential credit from campaigns the user is eligible 
        /// for, but has not yet claimed
        /// </summary>
        [JsonProperty(PropertyName = "potential_credit_amount")]
        public int PotentialCreditAmount { get; private set; }

        /// <summary>
        /// The user's progress toward the merchant's loyalty reward goal
        /// </summary>
        [JsonProperty(PropertyName = "progress_percentage")]
        public decimal ProgressPercentage { get; private set; }

        /// <summary>
        /// The total amount of credit ever applied at this merchant on the user's behalf
        /// </summary>
        [JsonProperty(PropertyName = "savings_amount")]
        public int SavingsAmount { get; private set; }

        /// <summary>
        /// The amount of money the user needs to spend in order to reach the merchant's loyalty reward goal
        /// </summary>
        [JsonProperty(PropertyName = "spend_remaining_amount")]
        public int SpendRemainingAmount { get; private set; }

        /// <summary>
        /// The total amount of money the user has ever spent at this merchant. This value includes tips
        /// </summary>
        [JsonProperty(PropertyName = "total_volume_amount")]
        public int TotalVolumeAmount { get; private set; }

        /// <summary>
        /// The ID of the user
        /// </summary>
        [JsonProperty(PropertyName = "user_id")]
        public int UserId { get; private set; }
    }
}
