#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="GiftCardRemoveValueResponse.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
    [JsonObject(MemberSerialization.OptIn)]
    [ObjectEnvelope("gift_card_value_removal")]
    [JsonConverter(typeof(EnvelopeSerializer))]
    public class GiftCardRemoveValueResponse : Response
    {
        /// <summary>
        /// Private constructor for deserialization
        /// </summary>
        private GiftCardRemoveValueResponse() { }

        public GiftCardRemoveValueResponse(int amountRemovedInCents, int newGiftCardAmountInCents, int previousGiftCardAmountInCents)
        {
            AmountRemovedInCents = amountRemovedInCents;
            NewGiftCardAmountInCents = newGiftCardAmountInCents;
            PreviousGiftCardAmountInCents = previousGiftCardAmountInCents;
        }

        [JsonProperty(PropertyName = "removed_value_amount")]
        public int AmountRemovedInCents { get; private set; }

        [JsonProperty(PropertyName = "new_value_at_merchant_amount")]
        public int NewGiftCardAmountInCents { get; private set; }

        [JsonProperty(PropertyName = "old_value_at_merchant_amount")]
        public int PreviousGiftCardAmountInCents { get; private set; }
    }
}
