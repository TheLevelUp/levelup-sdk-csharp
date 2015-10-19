//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="OrderDetailsResponse.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

namespace LevelUp.Api.Client.Models.Responses
{
    /// <summary>
    /// Class representing a detailed response to a LevelUp order request
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class OrderDetailsResponse
    {
        public OrderDetailsResponse()
        {
            OrderResponseContainer = new OrderContainer();
        }

        /// <summary>
        /// UUID which uniquely identifies the order
        /// </summary>
        public virtual string Identifier { get { return OrderResponseContainer.Identifier; } }

        /// <summary>
        /// Amount in cents spent on the order not including tip
        /// </summary>
        public virtual int SpendAmount { get { return OrderResponseContainer.SpendAmount; } }

        /// <summary>
        /// Amount in cents of the tip spent on the order
        /// </summary>
        public virtual int TipAmount { get { return OrderResponseContainer.TipAmount; } }

        /// <summary>
        /// Total amount in cents spent on the order
        /// </summary>
        public virtual int Total { get { return OrderResponseContainer.Total; } }

        /// <summary>
        /// Time the order was created in LevelUp system
        /// UTC time ISO standard 8601  YYYY-MM-DDTHH:MM:SSZ
        /// </summary>
        public virtual string CreatedAt { get { return OrderResponseContainer.CreatedAt; } }

        /// <summary>
        /// Amount of credit the merchant sponsored
        /// </summary>
        public virtual int MerchantFundedCredit { get { return OrderResponseContainer.MerchantFundedCreditAmount; } }

        /// <summary>
        /// Customer loyalty earned in this purchase
        /// </summary>
        public virtual int EarnAmount { get { return OrderResponseContainer.EarnAmount; } }

        /// <summary>
        /// User identifier 
        /// </summary>
        public virtual int LoyaltyId { get { return OrderResponseContainer.LoyaltyId; } }

        /// <summary>
        /// Time the refund was processed on LevelUp servers. 
        /// UTC time ISO standard 8601  YYYY-MM-DDTHH:MM:SSZ
        /// If the order has not been refunded, this value will be NULL
        /// </summary>
        public virtual string TimeOfRefund { get { return OrderResponseContainer.RefundedAt; } }

        /// <summary>
        /// User display name. User "First_name Last_initial"
        /// </summary>
        public virtual string UserName { get { return OrderResponseContainer.UserName; } }

        /// <summary>
        /// Time the transaction was processed at the merchant location. 
        /// UTC time ISO standard 8601  YYYY-MM-DDTHH:MM:SSZ
        /// </summary>
        public virtual string TimeOfTransaction { get { return OrderResponseContainer.TransactedAt; } }

        /// <summary>
        /// ID of the location where the order was placed
        /// </summary>
        public virtual int LocationId { get { return OrderResponseContainer.LocationId; } }

        /// <summary>
        /// This container is used to aid in correct JSON serialization
        /// </summary>
        [JsonProperty(PropertyName = "order")]
        private OrderContainer OrderResponseContainer { get; set; }

        public override string ToString()
        {
            return OrderResponseContainer.ToString();
        }
    }
}
