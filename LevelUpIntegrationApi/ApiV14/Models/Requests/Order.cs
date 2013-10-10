using Newtonsoft.Json;
using System.Collections.Generic;

namespace LevelUpApi.Models.Requests
{
    /// <summary>
    /// Class representing a LevelUp order request object
    /// </summary>
    [JsonObject]
    public class Order
    {
        /// <summary>
        /// Constructor for a LevelUp order item
        /// </summary>
        /// <param name="locationId">The identification number for the origin of the order</param>
        /// <param name="qrPaymentData">The customer's QR code payment data as a string</param>
        /// <param name="spendAmountCents">The amount due on the check in cents</param>
        /// <param name="register">An identifier indicating which register placed the order. Default is null</param>
        /// <param name="cashier">The name of the cashier or server who handled the order if available. Default is null</param>
        /// <param name="identifierFromMerchant">An unique order identifier specific to the POS system which will be 
        /// used to resolved possible duplicate orders. This should be the POS internal order number in most cases. 
        /// e.g. An order number that servers would call for the customer to pick up their order. Default is null</param>
        /// <param name="items">A list of items that comprise the order</param>
        public Order(int locationId,
                     string qrPaymentData,
                     int spendAmountCents,
                     string register = null,
                     string cashier = null,
                     string identifierFromMerchant = null,
                     IList<Item> items = null)
        {
            this.OrderContainer = new OrderRequestContainer(locationId,
                                                            qrPaymentData,
                                                            spendAmountCents,
                                                            register,
                                                            cashier,
                                                            identifierFromMerchant,
                                                            items);
        }

        /// <summary>
        /// This container is used to aid in correct JSON serialization
        /// </summary>
        [JsonProperty(PropertyName = "order")]
        private OrderRequestContainer OrderContainer { get; set; }
    }
}
