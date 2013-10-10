using Newtonsoft.Json;

namespace LevelUpApi.Models.Requests
{
    /// <summary>
    /// Class representing an item that comprises part of an order to be sent to LevelUp
    /// </summary>
    [JsonObject]
    public class Item
    {
        /// <summary>
        /// Constructor for a LevelUp item object
        /// </summary>
        /// <param name="name">Name of the item</param>
        /// <param name="description">Description for the item</param>
        /// <param name="sku">Stock keeping unit number of the item</param>
        /// <param name="upc">UPC code for the item</param>
        /// <param name="category">Item category</param>
        /// <param name="chargedPriceCents">Amount charged for the ordered item in cents. e.g. 1 order of shrimp may
        /// have 10 shrimp each with a unit price of 100¢ ($1) each making the charged price for this item 1000¢ ($10)</param>
        /// <param name="unitPriceCents">Price of individiual units within the item in cents. e.g. 1 order of shrimp
        /// may have 10 shrimp each with a unit price of 100¢ ($1) each making the charged price 1000¢ ($10)</param>
        /// <param name="quantity">Number of identical items represented by this object. Default is 1. 
        /// e.g. If 7 people order a hamburger each with each order being identical, this could be represented as 7 
        /// instances of hamburger items each with quantity equal to 1 or one instance of a hamburger item with 
        /// quantity equal to 7. The correct choice will depend on the POS system implementation of items.</param>
        public Item(string name,
                    string description,
                    string sku,
                    string upc,
                    string category,
                    int chargedPriceCents,
                    int? unitPriceCents,
                    int quantity = 1)
        {
            this.ItemContainer = new ItemContainer(name,
                                                   description,
                                                   sku,
                                                   upc,
                                                   category,
                                                   chargedPriceCents,
                                                   unitPriceCents,
                                                   quantity);
        }

        /// <summary>
        /// This container is used to aid in correct JSON serialization
        /// </summary>
        [JsonProperty(PropertyName = "item")]
        private ItemContainer ItemContainer { get; set; }
    }
}
