//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="DetachedRefundContainer.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

namespace LevelUp.Api.Client.Models.Requests
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class DetachedRefundContainer
    {
        internal DetachedRefundContainer() { }

        internal DetachedRefundContainer(int locationId, 
                                         string qrPaymentData, 
                                         int creditAmountCents, 
                                         string register, 
                                         string cashier, 
                                         string identifierFromMerchant,
                                         string managerConfirmation,
                                         string customerFacingReason,
                                         string internalReason)
        {
            LocationId = locationId;
            PaymentData = qrPaymentData;
            CreditAmountCents = creditAmountCents;
            Register = register;
            Cashier = cashier;
            Identifier = identifierFromMerchant;
            ManagerConfirmation = managerConfirmation;
            CustomerFacingReason = customerFacingReason;
            InternalReason = internalReason;
        }

        [JsonProperty(PropertyName = "cashier")]
        public string Cashier { get; set; }

        [JsonProperty(PropertyName = "credit_amount")]
        public int CreditAmountCents { get; set; }

        [JsonProperty(PropertyName = "customer_facing_reason")]
        public string CustomerFacingReason { get; set; }

        [JsonProperty(PropertyName = "identifier_from_merchant")]
        public string Identifier { get; set; }

        [JsonProperty(PropertyName = "internal_reason")]
        public string InternalReason { get; set; }

        [JsonProperty(PropertyName = "location_id")]
        public int LocationId { get; set; }

        [JsonProperty(PropertyName = "manager_confirmation")]
        public string ManagerConfirmation { get; set; }

        [JsonProperty(PropertyName = "payment_token_data")]
        public string PaymentData { get; set; }

        [JsonProperty(PropertyName = "register")]
        public string Register { get; set; }
    }
}
