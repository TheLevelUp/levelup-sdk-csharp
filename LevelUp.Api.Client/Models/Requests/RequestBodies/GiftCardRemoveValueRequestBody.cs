﻿#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="GiftCardRemoveValueRequestBody.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using System;
using JsonEnvelopeSerializer;
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Requests
{
    /// <summary>
    /// Class representing a LevelUp gift card remove value request
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [ObjectEnvelope("gift_card_value_removal")]
    [JsonConverter(typeof(EnvelopeSerializer))]
    public class GiftCardRemoveValueRequestBody
    {
        private GiftCardRemoveValueRequestBody()
        {
            // Private constructor for deserialization
        }

        /// <summary>
        /// Creates a destroy gift card value request to reverse a specific gift card add value transaction. 
        /// </summary>
        /// <param name="giftCardQrData">The qr code of the target card or account</param>
        /// <param name="uuid">GiftCard addition uuid to reverse</param>
        public GiftCardRemoveValueRequestBody(string giftCardQrData, Guid uuid)
        {
            GiftCardQrData = giftCardQrData;
            Uuid = uuid;
        }

        /// <summary>
        /// Creates a destroy gift card value request to destroy giftcard value by certain amount.
        /// </summary>
        /// <param name="giftCardQrData">The qr code of the target card or account</param>
        /// <param name="amountInCents">The amount of value to destroy in US Cents</param>
        public GiftCardRemoveValueRequestBody(string giftCardQrData, int amountInCents)
        {
            AmountInCents = amountInCents;
            GiftCardQrData = giftCardQrData;
        }

        /// <summary>
        /// The amount of value to be removed from the gift card in US Cents. 
        /// This must be a positive amount and should not exceed the amount available on the gift card.
        /// This amount value should be null if this request is being used with an UUID value.
        /// </summary>
        [JsonProperty(PropertyName = "value_amount")]
        public int? AmountInCents { get; private set; }


        /// <summary>
        /// The Uuid of "GiftCardAddValue" response to be reversed.
        /// This will reverse the GiftCardAddValue call, and it is idempotent
        /// </summary>
        [JsonProperty(PropertyName = "uuid")]
        public Guid? Uuid { get; set; }

        /// <summary>
        /// QR Data identifying the gift card or user account
        /// </summary>
        [JsonProperty(PropertyName = "payment_token_data")]
        public string GiftCardQrData { get; private set; }
    }
}
