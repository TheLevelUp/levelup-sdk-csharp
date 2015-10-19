//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="OrderRequestContainer.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using System.Collections.Generic;

namespace LevelUp.Api.Client.Models.Requests
{
    [JsonObject]
    internal class OrderRequestContainer : CheckDataContainer
    {
        public OrderRequestContainer(int locationId,
                                     string qrPaymentData,
                                     int spendAmountCents,
                                     int? appliedDiscountAmountCents,
                                     int? availableGiftCardAmountCents,
                                     int exemptionAmountCents,
                                     string register,
                                     string cashier,
                                     string identifierFromMerchant,
                                     bool partialAuthorizationAllowed,
                                     IList<Item> items)
            : base(qrPaymentData, identifierFromMerchant, items)
        {
            LocationId = locationId;
            SpendAmountCents = spendAmountCents;
            AppliedCreditAmountCents = AppliedDiscountAmountCents = appliedDiscountAmountCents;
            AvailableGiftCardAmountCents = availableGiftCardAmountCents;
            ExemptionAmountCents = exemptionAmountCents;
            Register = register;
            Cashier = cashier;
            PartialAuthorizationAllowed = partialAuthorizationAllowed;
        }

        [JsonIgnore]
        public int? AppliedCreditAmountCents { get; set; }

        [JsonProperty(PropertyName = "applied_discount_amount")]
        public int? AppliedDiscountAmountCents { get; set; }

        [JsonProperty(PropertyName = "available_gift_card_amount")]
        public int? AvailableGiftCardAmountCents { get; set; }

        [JsonProperty(PropertyName = "cashier")]
        public string Cashier { get; set; }

        [JsonProperty(PropertyName = "exemption_amount")]
        public int ExemptionAmountCents { get; set; }

        [JsonProperty(PropertyName = "location_id")]
        public int LocationId { get; set; }

        [JsonProperty(PropertyName = "partial_authorization_allowed")]
        public bool PartialAuthorizationAllowed { get; set; }

        [JsonProperty(PropertyName = "register")]
        public string Register { get; set; }

        [JsonProperty(PropertyName = "spend_amount")]
        public int SpendAmountCents { get; set; }
    }
}
