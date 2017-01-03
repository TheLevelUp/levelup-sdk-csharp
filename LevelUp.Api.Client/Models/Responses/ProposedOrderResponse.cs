#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="ProposedOrderResponse.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Responses
{
    [JsonObject(MemberSerialization.OptIn)]
    [LevelUpSerializableModel("proposed_order")]
    [JsonConverter(typeof(LevelUpModelSerializer))]
    public class ProposedOrderResponse : IResponse
    {
        /// <summary>
        /// Private constructor for deserialization
        /// </summary>
        private ProposedOrderResponse() { }

        /// <summary>
        /// Internal constructor for testing
        /// </summary>
        internal ProposedOrderResponse(string proposedOrderIdentifier, int discountAmountCents)
        {
            ProposedOrderIdentifier = proposedOrderIdentifier;
            DiscountAmountCents = discountAmountCents;
        }
        
        [JsonProperty(PropertyName = "uuid")]
        public string ProposedOrderIdentifier { get; private set; }

        [JsonProperty(PropertyName = "discount_amount")]
        public int DiscountAmountCents { get; private set; }

        public override string ToString()
        {
            return string.Format("ProposedOrderIdentifier: {0}{1}Discount Amount: {2}¢", ProposedOrderIdentifier, System.Environment.NewLine, DiscountAmountCents);
        }
    }
}
