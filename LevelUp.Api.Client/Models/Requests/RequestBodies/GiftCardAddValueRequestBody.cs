#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="GiftCardAddValueRequestBody.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
    /// <summary>
    /// Class representing a LevelUp gift card add value request
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [ObjectEnvelope("gift_card_value_addition")]
    [JsonConverter(typeof(EnvelopeSerializer))]
    public class GiftCardAddValueRequestBody
    {
        private GiftCardAddValueRequestBody()
        {
            // Private constructor for deserialization
        }

        /// <summary>
        /// Creates a add value request for a LevelUp gift card
        /// </summary>
        /// <param name="giftCardQrData">The qr code of the target card or account</param>
        /// <param name="amountInCents">The amount of value to add in US Cents</param>
        /// <param name="locationId">The location id of the store where the value add originates</param>
        /// <param name="identifierFromMerchant">The check identifier for the check on which the gift
        /// card is purchased</param>
        /// <param name="tenderTypes">A collection of tender types used to pay for the check on which
        /// the gift card is purchased</param>
        /// <param name="levelUpOrderId">If applicable, a LevelUp order Id associated with the purchase of
        /// the gift card for which value is added</param>
        /// <param name="externalIdentifier"></param>
        public GiftCardAddValueRequestBody(string giftCardQrData,
                                       int amountInCents,
                                       int locationId,
                                       string identifierFromMerchant = null,
                                       IList<string> tenderTypes = null,
                                       string levelUpOrderId = null,
                                       string externalIdentifier = null)
        {
            AmountInCents = amountInCents;
            AssociatedLevelUpOrderId = levelUpOrderId;
            GiftCardQrData = giftCardQrData;
            IdentifierFromMerchant = identifierFromMerchant;
            LocationId = locationId;
            TenderTypesInternal = tenderTypes;
            GiftCardExternalIdentifier = externalIdentifier;
        }

        /// <summary>
        /// The amount of value to be loaded onto the gift card in US Cents. This must be a positive amount.
        /// </summary>
        [JsonProperty(PropertyName = "value_amount")]
        public int AmountInCents { get; private set; }

        /// <summary>
        /// If there was a LevelUp tender that paid for the check that includes the gift card add value,
        /// The LevelUp order id ProposedOrderIdentifier should be passed here
        /// </summary>
        [JsonProperty(PropertyName = "order_uuid")]
        public string AssociatedLevelUpOrderId { get; private set; }

        /// <summary>
        /// QR Data identifying the gift card or user account
        /// </summary>
        [JsonProperty(PropertyName = "payment_token_data")]
        public string GiftCardQrData { get; private set; }

        /// <summary>
        /// A string which uniquely identifies the check on which the gift card is purchased within the 
        /// merchant's POS system. Using this value, the merchant should be able to find the check associated 
        /// with this value add operation days or weeks after the check has been closed
        /// </summary>
        [JsonProperty(PropertyName = "identifier_from_merchant")]
        public string IdentifierFromMerchant { get; private set; }

        /// <summary>
        /// The LevelUp location id from whence this add value request originates
        /// </summary>
        [JsonProperty(PropertyName = "location_id")]
        public int LocationId { get; private set; }

        /// <summary>
        /// Giftcard's external id, used for idempotency of gift card add value request
        /// </summary>
        [JsonProperty(PropertyName = "external_identifier")]
        public string GiftCardExternalIdentifier { get; private set; }

        /// <summary>
        /// A collection of tenders used to pay for the check which includes this value add operation
        /// </summary>
        [JsonIgnore]
        public IList<string> TenderTypes
        {
            get { return (TenderTypesInternal != null) ? new List<string>(TenderTypesInternal) : null; }
        }

        [JsonProperty(PropertyName = "tender_types")]
        private IList<string> TenderTypesInternal { get; set; }

    }
}
