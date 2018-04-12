#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="GiftCardCreditQueryRequestBody.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

namespace LevelUp.Api.Client.Models.Requests
{
    /// <summary>
    /// Class representing a LevelUp gift card credit query request
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [ObjectEnvelope("get_merchant_funded_gift_card_credit")]
    [JsonConverter(typeof(EnvelopeSerializer))]
    public class GiftCardCreditQueryRequestBody
    {
        private GiftCardCreditQueryRequestBody()
        {
            // Private constructor for deserialization
        }

        /// <summary>
        /// Creates a credit query request for a LevelUp gift card
        /// </summary>
        /// <param name="giftCardQrData">The qr code of the target card or account</param>
        public GiftCardCreditQueryRequestBody(string giftCardQrData)
        {
            GiftCardQrData = giftCardQrData;
        }

        /// <summary>
        /// QR Data identifying the gift card or user account
        /// </summary>
        [JsonProperty(PropertyName = "payment_token_data")]
        public string GiftCardQrData { get; private set; }
    }
}
