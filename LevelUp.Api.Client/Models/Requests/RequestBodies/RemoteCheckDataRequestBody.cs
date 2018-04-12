#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="RemoteCheckDataRequestBody.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using System.Collections.Generic;
using JsonEnvelopeSerializer;
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Requests
{
    [JsonObject(MemberSerialization.OptIn)]
    [ObjectEnvelope("check")]
    [JsonConverter(typeof(EnvelopeSerializer))]
    public class RemoteCheckDataRequestBody
    {
        private RemoteCheckDataRequestBody()
        {
            // Private constructor for deserialization
        }

        /// <summary>
        /// A model of a request to update the check data in the remote datastore
        /// </summary>
        /// <param name="locationId">The LevelUp location id that is the source of this request</param>
        /// <param name="spendAmountCents">Amount in cents to charge the customer</param>
        /// <param name="taxAmountCents">Tax portion in cents of this spend amount.
        /// NOTE: This will not always be the same as the total tax on the check</param>
        /// <param name="exemptionAmountCents">Exemption amount in cents relevant to this spend.
        /// NOTE: This will not always be the same as the total exemption amount for this check</param>
        /// <param name="identifierFromMerchant">An identifier for the check used by the merchant's system.
        /// This will usually be a check number or check id. It should uniquely identify the check for this merchant</param>
        /// <param name="register">The register identifier or name for this transaction</param>
        /// <param name="cashier">The cashier name for this transaction</param>
        /// <param name="partialAuthorizationAllowed">A flag to allow LevelUp to respond with an authorized amount
        /// that is less than the requested spend amount. If this is set to false and the customer is not able to pay
        /// for the full spend amount, the payment will be rejected outright</param>
        /// <param name="items">A list of the items on the check</param>
        public RemoteCheckDataRequestBody(int locationId,
                                            int spendAmountCents,
                                            int taxAmountCents,
                                            int exemptionAmountCents,
                                            string identifierFromMerchant = null,
                                            string register = null,
                                            string cashier = null,
                                            bool partialAuthorizationAllowed = true,
                                            IList<Item> items = null)
        {
            LocationId = locationId;
            SpendAmountCents = spendAmountCents;
            TaxAmountCents = taxAmountCents;
            ExemptionAmountCents = exemptionAmountCents;
            Register = register;
            Cashier = cashier;
            PartialAuthorizationAllowed = partialAuthorizationAllowed;
            IdentifierFromMerchant = identifierFromMerchant;
            ItemsInternal = items;
        }

        [JsonProperty(PropertyName = "cashier")]
        public string Cashier { get; private set; }

        [JsonProperty(PropertyName = "exemption_amount")]
        public int ExemptionAmountCents { get; private set; }

        [JsonProperty(PropertyName = "location_id")]
        public int LocationId { get; private set; }

        [JsonProperty(PropertyName = "partial_authorization_allowed")]
        public bool PartialAuthorizationAllowed { get; private set; }

        [JsonProperty(PropertyName = "register")]
        public string Register { get; private set; }

        [JsonProperty(PropertyName = "spend_amount")]
        public int SpendAmountCents { get; private set; }

        [JsonProperty(PropertyName = "tax_amount")]
        public int TaxAmountCents { get; private set; }

        [JsonProperty(PropertyName = "identifier_from_merchant")]
        public string IdentifierFromMerchant { get; private set; }

        [JsonIgnore]
        public IList<Item> Items
        {
            get { return Item.CopyList(ItemsInternal); }
        }

        [JsonProperty(PropertyName = "items")]
        private IList<Item> ItemsInternal { get; set; }
    }
}
