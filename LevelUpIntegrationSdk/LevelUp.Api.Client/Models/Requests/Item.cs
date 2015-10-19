//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="Item.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace LevelUp.Api.Client.Models.Requests
{
    /// <summary>
    /// Class representing an item that comprises part of an order to be sent to LevelUp
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Item
    {
        private Item()
        {
            //Private constructor for deserialization
        }

        /// <summary>
        /// Constructor for a LevelUp item object
        /// </summary>
        /// <param name="name">Name of the item</param>
        /// <param name="description">Description for the item</param>
        /// <param name="sku">Stock keeping unit number of the item</param>
        /// <param name="upc">Universal product code of the item</param>
        /// <param name="category">Item category</param>
        /// <param name="chargedPriceCents">Amount charged for the ordered item in cents. e.g. 1 order of shrimp may
        /// have 10 shrimp each with a unit price of 100¢ ($1) each making the charged price for this item 1000¢ ($10)</param>
        /// <param name="standardPriceCents">Price of individiual units within the item in cents. e.g. 1 order of shrimp
        /// may have 10 shrimp each with a unit price of 100¢ ($1) each making the charged price 1000¢ ($10). 
        /// This differs from charged price in that charged price should include any discounts or additions</param>
        /// <param name="quantity">Number of identical items represented by this object. Default is 1. 
        /// e.g. If 7 people order a hamburger each with each order being identical, this could be represented as 7 
        /// instances of hamburger items each with quantity equal to 1 or one instance of a hamburger item with 
        /// quantity equal to 7. The correct choice will depend on the POS system implementation of items.</param>
        /// <param name="modifiers"> The modifiers of the current item.
        /// e.g. If the current item is a house salad, the modifiers of that salad might be "chicken" or "cheese".  The
        /// "cheese" modifier might have its own modifier of "mozzarella".</param>
        public Item(string name,
                    string description,
                    string sku,
                    string upc,
                    string category,
                    int chargedPriceCents,
                    int? standardPriceCents,
                    int quantity = 1,
                    IList<Item> modifiers = null)
        {
            this.ItemContainer = new ItemContainer(name,
                                                   description,
                                                   sku,
                                                   upc,
                                                   category,
                                                   chargedPriceCents,
                                                   standardPriceCents,
                                                   quantity,
                                                   modifiers);
        }

        /// <summary>
        /// The name of the item
        /// </summary>
        public string Name { get { return ItemContainer.Name; } }

        /// <summary>
        /// The item description
        /// </summary>
        public string Description { get { return ItemContainer.Description; } }

        /// <summary>
        /// Stock keeping unit number of the item
        /// </summary>
        public string Sku { get { return ItemContainer.Sku; } }
        
        /// <summary>
        /// Universal product code of the item
        /// </summary>
        public string Upc { get { return ItemContainer.Upc; } }

        /// <summary>
        /// Item category. E.g. "Entrees" or "Alcohol" or "Sandwiches"
        /// </summary>
        public string Category { get { return ItemContainer.Category; } }

        /// <summary>
        /// Price in cents (¢) charged for the item
        /// e.g. 1 order of shrimp may  have 10 shrimp each with a unit price of 100¢ ($1) each 
        /// making the charged price for this item 1000¢ ($10)
        /// </summary>
        public int ChargedPrice { get { return ItemContainer.ChargedPriceInCents; } }

        /// <summary>
        /// Price in cents (¢) charged for each unit that comprises the item. 
        /// This should usually be the same as ChargedPrice
        /// e.g. 1 order of shrimp may have 10 shrimp each with a unit price of 100¢ ($1) each
        /// making the charged price 1000¢ ($10)
        /// </summary>
        public int? StandardPrice { get { return ItemContainer.StandardPriceInCents; } }

        /// <summary>
        /// Number of identical units represented by this item. Default is 1. 
        /// e.g. If 7 people order a hamburger each with each order being identical, this could be represented as 7 
        /// instances of hamburger items each with quantity equal to 1 or one instance of a hamburger item with 
        /// quantity equal to 7. The correct choice will depend on the POS system implementation of items.
        /// </summary>
        public int Quantity { get { return ItemContainer.Quantity; } }

        #region Obsolete Properties

        /// <summary>
        /// Price in cents (¢) charged for each unit that comprises the item. 
        /// This should usually be the same as ChargedPrice
        /// e.g. 1 order of shrimp may have 10 shrimp each with a unit price of 100¢ ($1) each
        /// making the charged price 1000¢ ($10)
        /// </summary>
        [Obsolete("Use StandardPrice property instead of UnitPrice")]
        public int? UnitPrice { get { return ItemContainer.StandardPriceInCents; } }

        #endregion Obsolete Properties

        /// <summary>
        /// The modifiers of the current item.
        /// e.g. If the current item is a house salad, the modifiers of that salad might be "chicken" or "cheese".  The
        /// "cheese" modifier might have its own modifier of "mozzarella".
        /// </summary>
        public IList<Item> Modifiers { get { return ItemContainer.Modifiers; } }

        /// <summary>
        /// This container is used to aid in correct JSON serialization
        /// </summary>
        [JsonProperty(PropertyName = "item")]
        private ItemContainer ItemContainer { get; set; }
    }
}
