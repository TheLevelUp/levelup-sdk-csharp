//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="GiftCardValue.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

namespace LevelUp.Api.Client.Models.Requests
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GiftCardValue
    {
        protected GiftCardValue()
        {
            //private constructor for deserialization
        }

        public GiftCardValue(string qrPaymentData, int amountInCents)
        {
            PaymentData = qrPaymentData;
            AmountInCents = amountInCents;
        }

        [JsonProperty(PropertyName = "payment_token_data")]
        public string PaymentData { get; set; }

        [JsonProperty(PropertyName = "value_amount")]
        public int AmountInCents { get; set; }
    }
}
