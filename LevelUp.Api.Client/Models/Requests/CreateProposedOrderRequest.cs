#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="CreateProposedOrderRequest.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using LevelUp.Api.Client.Models.RequestVisitors;
using LevelUp.Api.Http;

namespace LevelUp.Api.Client.Models.Requests
{
    public class CreateProposedOrderRequest : Request
    {
        protected override LevelUpApiVersion _applicableAPIVersionsBitmask
        {
            get { return LevelUpApiVersion.v15; }
        }

        /// <summary>
        /// A Serializable http body for the CreateProposedOrderRequest
        /// </summary>
        public CreateProposedOrderRequestBody Body { get { return _body; } }
        private readonly CreateProposedOrderRequestBody _body;

        /// <summary>
        /// A request that creates a proposed order as part of a two-step order workflow (create 
        /// proposed order, complete proposed order)
        /// </summary>
        /// <param name="accessToken">App access token for the request</param>
        /// <param name="locationId">The identification number for the origin of the order</param>
        /// <param name="qrPaymentData">The customer's QR code payment data as a string</param>
        /// <param name="spendAmountCents">The amount due on the check in cents</param>
        /// <param name="taxAmountCents">The tax amount that will be paid by the current pending order.</param>
        /// <param name="exemptionAmountCents">The portion the spendAmount that is exempted from being used to
        /// earn and redeem credit.</param>
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
        /// <param name="items">A list of items that comprise the order</param>
        public CreateProposedOrderRequest(string accessToken,
                                    int locationId,
                                    string qrPaymentData,
                                    int spendAmountCents,
                                    int? taxAmountCents,
                                    int exemptionAmountCents,
                                    string register,
                                    string cashier,
                                    string identifierFromMerchant,
                                    string receiptMessageHtml,
                                    bool partialAuthorizationAllowed,
                                    IList<Item> items)
            : base(accessToken)
        {
            _body = new CreateProposedOrderRequestBody(locationId, qrPaymentData, spendAmountCents, taxAmountCents, 
                exemptionAmountCents, register, cashier, identifierFromMerchant, receiptMessageHtml, 
                partialAuthorizationAllowed, items);
        }


        /// <summary>
        /// Acceptance method for Request visitors.
        /// </summary>
        public override T Accept<T>(IRequestVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
