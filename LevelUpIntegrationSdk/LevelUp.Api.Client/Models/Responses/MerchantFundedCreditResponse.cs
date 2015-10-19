//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="MerchantFundedCreditResponse.cs" company="SCVNGR, Inc. d/b/a LevelUp">
//   Copyright(c) 2015 SCVNGR, Inc. d/b/a LevelUp. All rights reserved.
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

namespace LevelUp.Api.Client.Models.Responses
{
    /// <summary>
    /// Class representing a detail response from a Merchant Funded Credit Query
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class MerchantFundedCreditResponse
    {
        public MerchantFundedCreditResponse()
        {
            MerchantFundedCreditResponseContainer = new MerchantFundedCreditResponseContainer();
        }

        /// <summary>
        /// The amount of available merchant funded credit available for discount in cents
        /// </summary>
        public virtual int DiscountAmount
        {
            get
            {
                // DiscountAmount will replace TotalAmount once gift cards are implemented
                return MerchantFundedCreditResponseContainer.DiscountAmount ??
                       MerchantFundedCreditResponseContainer.TotalAmount.GetValueOrDefault(0);
            }
        }

        /// <summary>
        /// The amount of available gift card credit in cents
        /// </summary>
        public virtual int GiftCardAmount
        {
            get { return MerchantFundedCreditResponseContainer.GiftCardAmount.GetValueOrDefault(0); }
        }

        /// <summary>
        /// This container is used to aid in correct JSON serialization
        /// </summary>
        [JsonProperty(PropertyName = "merchant_funded_credit")]
        private MerchantFundedCreditResponseContainer MerchantFundedCreditResponseContainer { get; set; }

        public override string ToString()
        {
            return MerchantFundedCreditResponseContainer.ToString();
        }
    }
}
