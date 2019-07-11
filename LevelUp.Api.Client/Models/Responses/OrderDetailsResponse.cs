#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="OrderDetailsResponse.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
    /// Class representing a detailed response to a LevelUp order request
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [ObjectEnvelope("order")]
    [JsonConverter(typeof(EnvelopeSerializer))]
    public class OrderDetailsResponse : Response
    {
        /// <summary>
        /// Private constructor for deserialization
        /// </summary>
        private OrderDetailsResponse() { }

        public OrderDetailsResponse(int id, string orderIdentifier, string identifierFromMerchant, int spendAmount, int tipAmount,
            int total, string createdAt, int merchantFundedCredit, int earnAmount, int loyaltyId, string timeOfRefund,
            string userName, string timeOfTransaction, int locationId, int merchantId, string merchantName, int balanceAmount,
            int contributionAmount, int creditAppliedAmount, int creditEarnedAmount, int displaybalanceAmount, int exemptionAmount,
            int requestedSpendAmount, int requestedTipAmount, int requestedTotalAmount, int taxAmount)
        {
            Id = id;
            OrderIdentifier = orderIdentifier;
            IdentifierFromMerchant = identifierFromMerchant;
            SpendAmount = spendAmount;
            TipAmount = tipAmount;
            Total = total;
            CreatedAt = createdAt;
            MerchantFundedCredit = merchantFundedCredit;
            EarnAmount = earnAmount;
            LoyaltyId = loyaltyId;
            TimeOfRefund = timeOfRefund;
            UserName = userName;
            TimeOfTransaction = timeOfTransaction;
            LocationId = locationId;
            MerchantId = merchantId;
            MerchantName = merchantName;
            BalanceAmount = balanceAmount;
            ContributionAmount = contributionAmount;
            CreditAppliedAmount = creditAppliedAmount;
            CreditEarnedAmount = creditEarnedAmount;
            DisplaybalanceAmount = displaybalanceAmount;
            ExemptionAmount = exemptionAmount;
            RequestedSpendAmount = requestedSpendAmount;
            RequestedTipAmount = requestedTipAmount;
            RequestedTotalAmount = requestedTotalAmount;
            TaxAmount = taxAmount;
        }

    /// <summary>
    /// Order number in LevelUp
    /// </summary>
    [JsonProperty(PropertyName = "id")]
        public int Id { get; private set; }

        /// <summary>
        /// ProposedOrderIdentifier which uniquely identifies the order
        /// </summary>
        [JsonProperty(PropertyName = "uuid")]
        public string OrderIdentifier { get; private set; }

        /// <summary>
        /// Indentifier from merchant's POS (usually the ticket number)
        /// </summary>
        [JsonProperty(PropertyName = "identifier_from_merchant")]
        public string IdentifierFromMerchant { get; private set; }

        /// <summary>
        /// Amount in cents spent on the order not including tip
        /// </summary>
        [JsonProperty(PropertyName = "spend_amount")]
        public int SpendAmount { get; private set; }

        /// <summary>
        /// Amount in cents of the tip spent on the order
        /// </summary>
        [JsonProperty(PropertyName = "tip_amount")]
        public int TipAmount { get; private set; }

        /// <summary>
        /// Total amount in cents spent on the order
        /// </summary>
        [JsonProperty(PropertyName = "total_amount")]
        public int Total { get; private set; }

        /// <summary>
        /// Time the order was created in LevelUp system
        /// UTC time ISO standard 8601  YYYY-MM-DDTHH:MM:SSZ
        /// </summary>
        [JsonProperty(PropertyName = "created_at")]
        public string CreatedAt { get; private set; }

        /// <summary>
        /// Amount of credit the merchant sponsored
        /// </summary>
        [JsonProperty(PropertyName = "merchant_funded_credit_amount")]
        public int MerchantFundedCredit { get; private set; }

        /// <summary>
        /// Customer loyalty earned in this purchase
        /// </summary>
        [JsonProperty(PropertyName = "earn_amount")]
        public int EarnAmount { get; private set; }

        /// <summary>
        /// User identifier 
        /// </summary>
        [JsonProperty(PropertyName = "loyalty_id")]
        public int LoyaltyId { get; private set; }

        /// <summary>
        /// Time the refund was processed on LevelUp servers. 
        /// UTC time ISO standard 8601  YYYY-MM-DDTHH:MM:SSZ
        /// If the order has not been refunded, this value will be NULL
        /// </summary>
        [JsonProperty(PropertyName = "refunded_at")]
        public string TimeOfRefund { get; private set; }

        /// <summary>
        /// User display name. User "First_name Last_initial"
        /// </summary>
        [JsonProperty(PropertyName = "user_display_name")]
        public string UserName { get; private set; }

        /// <summary>
        /// Time the transaction was processed at the merchant location. 
        /// UTC time ISO standard 8601  YYYY-MM-DDTHH:MM:SSZ
        /// </summary>
        [JsonProperty(PropertyName = "transacted_at")]
        public string TimeOfTransaction { get; private set; }

        /// <summary>
        /// ID of the location where the order was placed
        /// </summary>
        [JsonProperty(PropertyName = "location_id")]
        public int LocationId { get; private set; }

        /// <summary>
        /// ID of the Merchant where the order was placed
        /// </summary>
        [JsonProperty(PropertyName = "merchant_id")]
        public int MerchantId { get; private set; }

        /// <summary>
        /// Name of the Merchant where the order was placed
        /// </summary>
        [JsonProperty(PropertyName = "merchant_name")]
        public string MerchantName { get; private set; }

        /// <summary>
        /// Balance amount
        /// </summary>
        [JsonProperty(PropertyName = "balance_amount")]
        public int BalanceAmount { get; private set; }

        /// <summary>
        /// Contribution amount
        /// </summary>
        [JsonProperty(PropertyName = "contribution_amount")]
        public int ContributionAmount { get; private set; }

        /// <summary>
        /// Credit applied amount
        /// </summary>
        [JsonProperty(PropertyName = "credit_applied_amount")]
        public int CreditAppliedAmount { get; private set; }

        /// <summary>
        /// Credit earned amount
        /// </summary>
        [JsonProperty(PropertyName = "credit_earned_amount")]
        public int CreditEarnedAmount { get; private set; }

        /// <summary>
        /// Display balance amount
        /// </summary>
        [JsonProperty(PropertyName = "display_balance_amount")]
        public int DisplaybalanceAmount { get; private set; }

        /// <summary>
        /// Exemption amount
        /// </summary>
        [JsonProperty(PropertyName = "exemption_amount")]
        public int ExemptionAmount { get; private set; }

        /// <summary>
        /// Requested spend amount
        /// </summary>
        [JsonProperty(PropertyName = "requested_spend_amount")]
        public int RequestedSpendAmount { get; private set; }

        /// <summary>
        /// Requested tip amount
        /// </summary>
        [JsonProperty(PropertyName = "requested_tip_amount")]
        public int RequestedTipAmount { get; private set; }

        /// <summary>
        /// Requested total amount
        /// </summary>
        [JsonProperty(PropertyName = "requested_total_amount")]
        public int RequestedTotalAmount { get; private set; }

        /// <summary>
        /// Tax amount
        /// </summary>
        [JsonProperty(PropertyName = "tax_amount")]
        public int TaxAmount { get; private set; }
    }
}
