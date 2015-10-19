//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="GiftCardAddValueRequest.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using System.Collections.Generic;
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Requests
{
    /// <summary>
    /// Class representing a LevelUp gift card add value request
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class GiftCardAddValueRequest
    {
        private GiftCardAddValueRequest()
        {
            //Private constructor for deserialization
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
        public GiftCardAddValueRequest(string giftCardQrData,
                                       int amountInCents,
                                       int locationId,
                                       string identifierFromMerchant = null,
                                       IList<string> tenderTypes = null,
                                       string levelUpOrderId = null)
            : this(new GiftCardValueAddContainer(giftCardQrData,
                                                 amountInCents,
                                                 locationId,
                                                 identifierFromMerchant,
                                                 tenderTypes,
                                                 levelUpOrderId))
        {
        }

        /// <summary>
        /// The amount of value to be loaded onto the gift card in US Cents. This must be a positive amount.
        /// </summary>
        public int AmountInCents
        {
            get { return GiftCardValue.AmountInCents; }
            set { GiftCardValue.AmountInCents = value; }
        }

        /// <summary>
        /// If there was a LevelUp tender that paid for the check that includes the gift card add value,
        /// The LevelUp order id UUID should be passed here
        /// </summary>
        public string AssociatedLevelUpOrderId
        {
            get { return GiftCardValue.LevelUpOrderId; }
            set { GiftCardValue.LevelUpOrderId = value; }
        }

        /// <summary>
        /// Creates a add value request for a LevelUp gift card. 
        /// </summary>
        /// <param name="giftCardValue">An object containing the qr data identifying the target card or account
        /// as well as the amount to add in cents</param>
        private GiftCardAddValueRequest(GiftCardValueAddContainer giftCardValue)
        {
            GiftCardValue = giftCardValue;
        }

        /// <summary>
        /// QR Data identifying the gift card or user account
        /// </summary>
        public string GiftCardQrData
        {
            get { return GiftCardValue.PaymentData; }
            set { GiftCardValue.PaymentData = value; }
        }

        /// <summary>
        /// A string which uniquely identifies the check on which the gift card is purchased within the 
        /// merchant's POS system. Using this value, the merchant should be able to find the check associated 
        /// with this value add operation days or weeks after the check has been closed
        /// </summary>
        public string IdentifierFromMerchant
        {
            get { return GiftCardValue.Identifier; }
            set { GiftCardValue.Identifier = value; }
        }

        /// <summary>
        /// The LevelUp location id from whence this add value request originates
        /// </summary>
        public int LocationId
        {
            get { return GiftCardValue.LocationId; }
            set { GiftCardValue.LocationId = value; }
        }

        /// <summary>
        /// A collection of tenders used to pay for the check which includes this value add operation
        /// </summary>
        public IList<string> TenderTypes
        {
            get { return GiftCardValue.TenderTypes; }
            set { GiftCardValue.TenderTypes = value; }
        }

        [JsonProperty(PropertyName = "gift_card_value_addition")]
        private GiftCardValueAddContainer GiftCardValue { get; set; }
    }
}
