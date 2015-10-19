//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="CheckDataContainer.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using System.Collections.Generic;

namespace LevelUp.Api.Client.Models.Requests
{
    [JsonObject]
    internal class CheckDataContainer
    {
        public CheckDataContainer(string paymentData,
                                  string identifierFromMerchant,
                                  IList<Item> items)
        {
            this.PaymentData = paymentData;
            this.Identifier = identifierFromMerchant;
            this.Items = items;
        }

        [JsonProperty(PropertyName = "identifier_from_merchant")]
        public string Identifier { get; set; }

        [JsonProperty(PropertyName = "items")]
        public IList<Item> Items { get; set; }

        [JsonProperty(PropertyName = "payment_token_data")]
        public string PaymentData { get; set; }
    }
}
