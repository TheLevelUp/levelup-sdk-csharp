//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="ItemContainer.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
    internal class ItemContainer
    {
        public ItemContainer(string name,
                             string description,
                             string sku,
                             string upc,
                             string category,
                             int chargedPriceCents,
                             int? standardPriceCents,
                             int quantity = 1,
                             IList<Item> modifiers = null)
        {
            this.Name = name;
            this.Description = description;
            this.Sku = sku;
            this.Upc = upc;
            this.Category = category;
            this.ChargedPriceInCents = chargedPriceCents;
            this.StandardPriceInCents = standardPriceCents;
            this.Quantity = quantity;
            this.Modifiers = modifiers;
        }

        [JsonProperty(PropertyName = "charged_price_amount")]
        public int ChargedPriceInCents { get; set; }

        [JsonProperty(PropertyName = "children")]
        public IList<Item> Modifiers { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "quantity")]
        public int Quantity { get; set; }

        [JsonProperty(PropertyName = "sku")]
        public string Sku { get; set; }

        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; }

        [JsonProperty(PropertyName = "standard_price_amount")]
        public int? StandardPriceInCents { get; set; }

        [JsonProperty(PropertyName = "upc")]
        public string Upc { get; set; }
    }
}
