#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="RefundResponse.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
    /// Class representing a LevelUp response to a refund request
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [ObjectEnvelope("order")]
    [JsonConverter(typeof(EnvelopeSerializer))]
    public class RefundResponse : Response
    {
        /// <summary>
        /// Private constructor for deserialization
        /// </summary>
        private RefundResponse() { }

        public RefundResponse(string createdAt, int merchantFundedCreditAmount, int earnAmount, string orderIdentifier,
            int loyaltyId, int spendAmount, string timeOfRefund, int tipAmount, int totalAmount, int locationId,
            string userName, string timeOfTransaction)
        {
            CreatedAt = createdAt;
            MerchantFundedCreditAmount = merchantFundedCreditAmount;
            EarnAmount = earnAmount;
            OrderIdentifier = orderIdentifier;
            LoyaltyId = loyaltyId;
            SpendAmount = spendAmount; 
            TimeOfRefund = timeOfRefund;
            TipAmount = tipAmount;
            TotalAmount = totalAmount;
            LocationId = locationId;
            UserName = userName;
            TimeOfTransaction = timeOfTransaction;
        }
        
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
        public int MerchantFundedCreditAmount { get; private set; }

        /// <summary>
        /// Customer loyalty earned in this purchase
        /// </summary>
        [JsonProperty(PropertyName = "earn_amount")]
        public int EarnAmount { get; private set; }

        /// <summary>
        /// ProposedOrderIdentifier that uniquely identifies the order
        /// </summary>
        [JsonProperty(PropertyName = "uuid")]
        public string OrderIdentifier { get; private set; }

        /// <summary>
        /// User identifier 
        /// </summary>
        [JsonProperty(PropertyName = "loyalty_id")]
        public int LoyaltyId { get; private set; }

        /// <summary>
        /// Amount spent in cents not including tip
        /// </summary>
        [JsonProperty(PropertyName = "spend_amount")]
        public int SpendAmount { get; private set; }

        /// <summary>
        /// Time the refund was processed on LevelUp servers. 
        /// UTC time ISO standard 8601  YYYY-MM-DDTHH:MM:SSZ
        /// If the order has not been refunded, this value will be NULL
        /// </summary>
        [JsonProperty(PropertyName = "refunded_at")]
        public string TimeOfRefund { get; private set; }

        /// <summary>
        /// Tip amount in cents
        /// </summary>
        [JsonProperty(PropertyName = "tip_amount")]
        public int TipAmount { get; private set; }

        /// <summary>
        /// Total amount spent in cents including cents
        /// </summary>
        [JsonProperty(PropertyName = "total_amount")]
        public int TotalAmount { get; private set; }

        /// <summary>
        /// ID of the location where transaction occurred
        /// </summary>
        [JsonProperty(PropertyName = "location_id")]
        public int LocationId { get; private set; }

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
    }
}
