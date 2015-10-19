//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="GiftCardValueAddContainer.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
    [JsonObject(MemberSerialization.OptIn)]
    internal class GiftCardValueAddContainer : GiftCardValue
    {
        private GiftCardValueAddContainer() : base()
        {
            //private constructor for deserialization
        }

        public GiftCardValueAddContainer(string qrCodeToken,
                                         int amountInCents,
                                         int locationId,
                                         string identifierFromMerchant = null,
                                         IList<string> tenderTypes = null,
                                         string levelUpOrderId = null)
            : base(qrCodeToken, amountInCents)
        {
            LocationId = locationId;
            Identifier = identifierFromMerchant;
            LevelUpOrderId = levelUpOrderId;
            TenderTypes = tenderTypes;
        }

        [JsonProperty(PropertyName = "identifier_from_merchant")]
        public string Identifier { get; set; }

        [JsonProperty(PropertyName = "order_uuid")]
        public string LevelUpOrderId { get; set; }

        [JsonProperty(PropertyName = "location_id")]
        public int LocationId { get; set; }

        [JsonProperty(PropertyName = "tender_types")]
        public IList<string> TenderTypes { get; set; }
    }
}
