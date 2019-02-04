#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="CompleteProposedOrderRequest.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using System.Collections.Generic;
using LevelUp.Api.Http;

namespace LevelUp.Api.Client.Models.Requests
{
    public class CompleteProposedOrderRequest : Request
    {
        protected override LevelUpApiVersion _applicableAPIVersionsBitmask
        {
            get { return LevelUpApiVersion.v15; }
        }

        /// <summary>
        /// A Serializable http body for the CompleteProposedOrderRequest
        /// </summary>
        public CompleteProposedOrderRequestBody Body { get; }

        /// <summary>
        /// A request that completes a proposed order as part of a two-step order workflow (create 
        /// proposed order, complete proposed order)
        /// </summary>
        /// <param name="accessToken">App access token for the request</param>
        /// <param name="locationId">The identification number for the origin of the order</param>
        /// <param name="qrPaymentData">The customer's QR code payment data as a string</param>
        /// <param name="proposedOrderUuid"></param>
        /// <param name="spendAmountCents">The amount due on the check in cents</param>
        /// <param name="taxAmountCents">The tax amount that will be paid by the current pending order.</param>
        /// <param name="exemptionAmountCents">The portion the spendAmount that is exempted from being used to
        /// earn and redeem credit.</param>
        /// <param name="appliedDiscountAmountCents"/>
        /// <param name="register">An identifier indicating which register placed the order. Default is null</param>
        /// <param name="cashier">The name of the cashier or server who handled the order if available. Default is null</param>
        /// <param name="identifierFromMerchant">An unique order identifier specific to the POS system which will be 
        /// used to resolved possible duplicate orders. This should be the POS internal order number in most cases. 
        /// e.g. An order number that servers would call for the customer to pick up their order. Can be null</param>
        /// <param name="receiptMessageHtml">Limited HTML (a, br, p, strong) to include on the user's email 
        /// receipt (1000 character limit). </param>
        /// <param name="partialAuthorizationAllowed">Parameter indicating whether orders should be partially authorized.
        /// That is. If a customer is only able to pay $5 and $10 is requested for payment, setting this value to true
        /// will instruct LevelUp to return authorization for $5. Setting the value to false will reject the order</param>
        /// <param name="discountOnly">Parameter indicating whether the order is only authorized for discounts. Setting
        /// this value to true will instruct LevelUp to retrieve available discounts and count the order for applicable
        /// campaigns, but otherwise not pay the balance due on the bill through LevelUp. Setting this value to false
        /// will instruct LevelUp that discounts and payment through LevelUp are both applicable.</param>
        /// <param name="items">A list of items that comprise the order</param>
        public CompleteProposedOrderRequest(string accessToken,
                                    int locationId,
                                    string qrPaymentData,
                                    string proposedOrderUuid,
                                    int spendAmountCents,
                                    int? taxAmountCents,
                                    int exemptionAmountCents,
                                    int? appliedDiscountAmountCents,
                                    string register,
                                    string cashier,
                                    string identifierFromMerchant,
                                    string receiptMessageHtml,
                                    bool partialAuthorizationAllowed,
                                    bool discountOnly,
                                    IList<Item> items)
            : base(accessToken)
        {
            Body = new CompleteProposedOrderRequestBody(locationId, qrPaymentData, proposedOrderUuid, spendAmountCents, 
                taxAmountCents, exemptionAmountCents, appliedDiscountAmountCents, register, cashier, identifierFromMerchant, 
                receiptMessageHtml, partialAuthorizationAllowed, discountOnly, items);
        }
    }
}
