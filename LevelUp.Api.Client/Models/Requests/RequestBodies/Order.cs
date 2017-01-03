#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="Order.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Requests
{
    /// <summary>
    /// Class representing a LevelUp order request object
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [LevelUpSerializableModel("order")]
    [JsonConverter(typeof(LevelUpModelSerializer))]
    public class Order
    {
        private Order()
        {
            // Private constructor for deserialization
        }

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
        [Obsolete("Recommended to use version of constructor that takes in the exemption & applied_credit amount.")]
        public Order(int locationId,
                     string qrPaymentData,
                     int spendAmountCents,
                     string register = null,
                     string cashier = null,
                     string identifierFromMerchant = null,
                     IList<Item> items = null) 
            : this(locationId,
                   qrPaymentData,
                   spendAmountCents,
                   null,
                   0,
                   register,
                   cashier,
                   identifierFromMerchant,
                   items)
        {  
        }

        /// <summary>
        /// Constructor for a LevelUp order item
        /// </summary>
        /// <param name="locationId">The identification number for the origin of the order</param>
        /// <param name="qrPaymentData">The customer's QR code payment data as a string</param>
        /// <param name="spendAmountCents">The amount due on the check in cents</param>
        /// <param name="exemptionAmountCents">The portion the spendAmount that is exempted from being used to
        /// earn and redeem credit.</param>
        /// <param name="register">An identifier indicating which register placed the order. Default is null</param>
        /// <param name="cashier">The name of the cashier or server who handled the order if available. Default is null</param>
        /// <param name="identifierFromMerchant">An unique order identifier specific to the POS system which will be 
        /// used to resolved possible duplicate orders. This should be the POS internal order number in most cases. 
        /// e.g. An order number that servers would call for the customer to pick up their order. Default is null</param>
        /// <param name="items">A list of items that comprise the order</param>
        [Obsolete("Recommended to use version of constructor that takes in the exemption & applied_credit amount.")]
        public Order(int locationId,
                     string qrPaymentData,
                     int spendAmountCents,
                     int exemptionAmountCents = 0,
                     string register = null,
                     string cashier = null,
                     string identifierFromMerchant = null,
                     IList<Item> items = null) 
            : this(locationId,
                   qrPaymentData,
                   spendAmountCents,
                   null,
                   exemptionAmountCents,
                   register,
                   cashier,
                   identifierFromMerchant,
                   items)
        {  
        }

        /// <summary>
        /// Constructor for a LevelUp order item
        /// </summary>
        /// <param name="locationId">The identification number for the origin of the order</param>
        /// <param name="qrPaymentData">The customer's QR code payment data as a string</param>
        /// <param name="spendAmountCents">The amount due on the check in cents</param>
        /// <param name="appliedDiscountAmountCents">The amount of LevelUp discount applied to the order</param>
        /// <param name="exemptionAmountCents">The portion the spendAmount that is exempted from being used to
        /// earn and redeem credit.</param>
        /// <param name="register">An identifier indicating which register placed the order. Default is null</param>
        /// <param name="cashier">The name of the cashier or server who handled the order if available. Default is null</param>
        /// <param name="identifierFromMerchant">An unique order identifier specific to the POS system which will be 
        /// used to resolved possible duplicate orders. This should be the POS internal order number in most cases. 
        /// e.g. An order number that servers would call for the customer to pick up their order. Default is null</param>
        /// <param name="items">A list of items that comprise the order</param>
        [Obsolete("Recommended to use version of constructor that takes in the availableGiftCardAmountCents argument.")]
        public Order(int locationId,
                     string qrPaymentData,
                     int spendAmountCents,
                     int? appliedDiscountAmountCents = null,
                     int exemptionAmountCents = 0,
                     string register = null,
                     string cashier = null,
                     string identifierFromMerchant = null,
                     IList<Item> items = null)
            : this(locationId,
                   qrPaymentData,
                   spendAmountCents,
                   appliedDiscountAmountCents,
                   null,
                   exemptionAmountCents,
                   register,
                   cashier,
                   identifierFromMerchant,
                   items)
        {
        }

        /// <summary>
        /// Constructor for a LevelUp order item
        /// </summary>
        /// <param name="locationId">The identification number for the origin of the order</param>
        /// <param name="qrPaymentData">The customer's QR code payment data as a string</param>
        /// <param name="spendAmountCents">The amount due on the check in cents</param>
        /// <param name="appliedDiscountAmountCents">The amount of LevelUp discount applied to the order</param>
        /// <param name="availableGiftCardAmountCents">The amount of LevelUp gift card credit available to the customer placing the order</param>
        /// <param name="exemptionAmountCents">The portion the spendAmount that is exempted from being used to
        /// earn and redeem credit.</param>
        /// <param name="register">An identifier indicating which register placed the order. Default is null</param>
        /// <param name="cashier">The name of the cashier or server who handled the order if available. Default is null</param>
        /// <param name="identifierFromMerchant">An unique order identifier specific to the POS system which will be 
        /// used to resolved possible duplicate orders. This should be the POS internal order number in most cases. 
        /// e.g. An order number that servers would call for the customer to pick up their order. Default is null</param>
        /// <param name="items">A list of items that comprise the order</param>
        [Obsolete("Recommended to use version of constructor that takes in partialAuthorizationAllowed argument.")]
        public Order(int locationId,
                     string qrPaymentData,
                     int spendAmountCents,
                     int? appliedDiscountAmountCents = null,
                     int? availableGiftCardAmountCents = null,
                     int exemptionAmountCents = 0,
                     string register = null,
                     string cashier = null,
                     string identifierFromMerchant = null,
                     IList<Item> items = null)
            : this(locationId,
                   qrPaymentData,
                   spendAmountCents,
                   appliedDiscountAmountCents,
                   availableGiftCardAmountCents,
                   exemptionAmountCents,
                   register,
                   cashier,
                   identifierFromMerchant,
                   true,
                   items)
        {
        }

        /// <summary>
        /// Constructor for a LevelUp order
        /// </summary>
        /// <param name="locationId">The identification number for the origin of the order</param>
        /// <param name="qrPaymentData">The customer's QR code payment data as a string</param>
        /// <param name="spendAmountCents">The amount due on the check in cents</param>
        /// <param name="appliedDiscountAmountCents">The amount of LevelUp discount applied to the order</param>
        /// <param name="availableGiftCardAmountCents">The amount of LevelUp gift card credit available to the customer placing the order</param>
        /// <param name="exemptionAmountCents">The portion the spendAmount that is exempted from being used to
        /// earn and redeem credit.</param>
        /// <param name="register">An identifier indicating which register placed the order. Default is null</param>
        /// <param name="cashier">The name of the cashier or server who handled the order if available. Default is null</param>
        /// <param name="identifierFromMerchant">An unique order identifier specific to the POS system which will be 
        /// used to resolved possible duplicate orders. This should be the POS internal order number in most cases. 
        /// e.g. An order number that servers would call for the customer to pick up their order. Default is null</param>
        /// <param name="partialAuthorizationAllowed">Parameter indicating whether orders should be partially authorized.
        /// That is. If a customer is only able to pay $5 and $10 is requested for payment, setting this value to true
        /// will instruct LevelUp to return authorization for $5. Setting the value to false will reject the order</param>
        /// <param name="items">A list of items that comprise the order</param>
        public Order(int locationId,
                     string qrPaymentData,
                     int spendAmountCents,
                     int? appliedDiscountAmountCents = null,
                     int? availableGiftCardAmountCents = null,
                     int exemptionAmountCents = 0,
                     string register = null,
                     string cashier = null,
                     string identifierFromMerchant = null,
                     bool partialAuthorizationAllowed = true,
                     IList<Item> items = null)
        {
            LocationId = locationId;
            PaymentData = qrPaymentData;
            SpendAmountCents = spendAmountCents;
            AppliedDiscountAmountCents = appliedDiscountAmountCents;
            AvailableGiftCardAmountCents = availableGiftCardAmountCents;
            ExemptionAmountCents = exemptionAmountCents;
            Register = register;
            Cashier = cashier;
            Identifier = identifierFromMerchant;
            PartialAuthorizationAllowed = partialAuthorizationAllowed;
            ItemsInternal = items;
            AppliedCreditAmountCents = appliedDiscountAmountCents;    
        }

        [JsonIgnore]
        public int? AppliedCreditAmountCents { get; private set; }

        [JsonProperty(PropertyName = "applied_discount_amount")]
        public int? AppliedDiscountAmountCents { get; private set; }

        [JsonProperty(PropertyName = "available_gift_card_amount")]
        public int? AvailableGiftCardAmountCents { get; private set; }

        [JsonProperty(PropertyName = "cashier")]
        public string Cashier { get; private set; }

        [JsonProperty(PropertyName = "exemption_amount")]
        public int? ExemptionAmountCents { get; private set; }

        [JsonProperty(PropertyName = "location_id")]
        public int LocationId { get; private set; }

        [JsonProperty(PropertyName = "partial_authorization_allowed")]
        public bool PartialAuthorizationAllowed { get; private set; }

        [JsonProperty(PropertyName = "register")]
        public string Register { get; private set; }

        [JsonProperty(PropertyName = "payment_token_data")]
        public string PaymentData { get; private set; }

        [JsonProperty(PropertyName = "spend_amount")]
        public int SpendAmountCents { get; private set; }

        [JsonProperty(PropertyName = "identifier_from_merchant")]
        public string Identifier { get; private set; }

        [JsonIgnore]
        public IList<Item> Items
        {
            get { return Item.CopyList(ItemsInternal); }
        }

        [JsonProperty(PropertyName = "items")]
        private IList<Item> ItemsInternal { get; set; }
    }
}
