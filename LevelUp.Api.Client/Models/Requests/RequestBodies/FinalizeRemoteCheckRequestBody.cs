#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="FinalizeRemoteCheckRequestBody.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

namespace LevelUp.Api.Client.Models.Requests
{
    [JsonObject(MemberSerialization.OptIn)]
    [LevelUpSerializableModel("order")]
    [JsonConverter(typeof(LevelUpModelSerializer))]
    public class FinalizeRemoteCheckRequestBody
    {
        private FinalizeRemoteCheckRequestBody()
        {
            // Private constructor for deserialization
        }

        /// <summary>
        /// Creates a request to finalize an order where the check data is stored remotely
        /// This request performs the same function as a create order request and is useful
        /// for building an integration for a full service environment
        /// </summary>
        /// <param name="spendAmountCents">The total amount of payment requested (includes appliedDiscountAmount) in cents</param>
        /// <param name="taxAmountCents">The tax amount in cents relevant to this order</param>
        /// <param name="appliedDiscountAmountCents">The discount in cents applied to the check as part of this order operation</param>
        public FinalizeRemoteCheckRequestBody(int spendAmountCents,
                                          int taxAmountCents = 0,
                                          int? appliedDiscountAmountCents = null)
        {
            SpendAmountCents = spendAmountCents;
            AppliedDiscountAmountCents = appliedDiscountAmountCents;
            TaxAmountCents = taxAmountCents;
        }

        [JsonProperty(PropertyName = "applied_discount_amount")]
        public int? AppliedDiscountAmountCents { get; private set; }

        [JsonProperty(PropertyName = "tax_amount")]
        public int TaxAmountCents { get; private set; }

        [JsonProperty(PropertyName = "spend_amount")]
        public int SpendAmountCents { get; private set; }
    }
}
