#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="IManageProposedOrders.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using LevelUp.Api.Client.Models.Requests;
using LevelUp.Api.Client.Models.Responses;
using LevelUp.Api.Http;

namespace LevelUp.Api.Client.ClientInterfaces
{
    public interface IManageProposedOrders : ILevelUpClientModule
    {
        /// <summary>
        /// Place an order and pay through LevelUp.  This method uses a two-step order creation workflow,
        /// wherein the client is expected to create a proposed order, apply any applicable discount 
        /// specified by the LevelUp platform, then complete the proposed order to close out the sum on 
        /// the check.
        /// </summary>
        /// <param name="accessToken">The LevelUp access token associated with the merchant account obtained from 
        /// the Authenticate() method</param>
        /// <param name="locationId">LevelUp Location ID for the store in which the order is being placed.</param>
        /// <param name="qrPaymentData">The customer's scanned QR code payment data</param>
        /// <param name="spendAmountCents">The amount due on the check in cents</param>
        /// <param name="taxAmountCents">The tax amount that will be paid by the current pending order.</param>
        /// <param name="exemptionAmountCents">The portion the spendAmount that is exempted from being used to
        /// earn and redeem credit.</param>
        /// <param name="register">An identifier indicating which register placed the order.</param>
        /// <param name="cashier">The name of the cashier or server who handled the order if available.</param>
        /// <param name="identifierFromMerchant">An unique order identifier specific to the POS system which will be 
        /// used to resolved possible duplicate orders. This should be the POS internal order number in most cases.</param>
        /// <param name="receiptMessageHtml">Limited HTML (a, br, p, strong) to include on the user's email 
        /// receipt (1000 character limit). </param>
        /// <param name="partialAuthorizationAllowed">Parameter indicating whether orders should be partially authorized.
        /// That is, if a customer is only able to pay $5 and $10 is requested for payment, setting this value to true
        /// will instruct the LevelUp platform to return authorization for $5. It will then be up to the user to pay for
        /// the remaining $5 balance on the check via an alternative method of payment (cash, etc.).  Setting the value 
        /// to false will reject the order, and the user would have to cover the full $10 with an alternate payment 
        /// method.</param>
        /// <param name="items">A list of items that comprise the order</param>
        /// <returns>A response object providing a unique ID associated with the pending order.</returns>
        /// <exception cref="LevelUpApiException"> The returned HTTP status code for the request was something other 
        /// than 200 (OK)</exception>
        ProposedOrderResponse CreateProposedOrder(  string accessToken,
                                                    int locationId,
                                                    string qrPaymentData,
                                                    int spendAmountCents,
                                                    int taxAmountCents,
                                                    int exemptionAmountCents,
                                                    string register = null,
                                                    string cashier = null,
                                                    string identifierFromMerchant = null,
                                                    string receiptMessageHtml = null,
                                                    bool partialAuthorizationAllowed = true,
                                                    IList<Item> items = null);

        /// <summary>
        /// Complete a previously created proposed order through LevelUp.  This method uses a two-step order 
        /// creation workflow, wherein the client is expected to create a proposed order, apply any applicable 
        /// discount specified by the LevelUp platform, then complete the proposed order to close out the sum on 
        /// the check.
        /// </summary>
        /// <param name="accessToken">The LevelUp access token associated with the merchant account obtained from 
        /// the Authenticate() method</param>
        /// <param name="locationId">LevelUp Location ID for the store in which the order is being placed.</param>
        /// <param name="qrPaymentData">The customer's scanned QR code payment data</param>
        /// <param name="proposedOrderUuid">The unique ID that was returned when the order was first proposed.</param>
        /// <param name="spendAmountCents">The amount due on the check in cents</param>
        /// <param name="taxAmountCents">The tax amount that will be paid by the current pending order.</param>
        /// <param name="exemptionAmountCents">The portion the spendAmount that is exempted from being used to
        /// earn and redeem credit.</param>
        /// <param name="appliedDiscountAmountCents">The amount in cents of discount that was applied by the POS to the
        /// check after proposing the order.</param>
        /// <param name="register">An identifier indicating which register placed the order.</param>
        /// <param name="cashier">The name of the cashier or server who handled the order if available.</param>
        /// <param name="identifierFromMerchant">An unique order identifier specific to the POS system which will be 
        /// used to resolved possible duplicate orders. This should be the POS internal order number in most cases.</param>
        /// <param name="receiptMessageHtml">Limited HTML (a, br, p, strong) to include on the user's email 
        /// receipt (1000 character limit). </param>
        /// <param name="partialAuthorizationAllowed">Parameter indicating whether orders should be partially authorized.
        /// That is, if a customer is only able to pay $5 and $10 is requested for payment, setting this value to true
        /// will instruct the LevelUp platform to return authorization for $5. It will then be up to the user to pay for
        /// the remaining $5 balance on the check via an alternative method of payment (cash, etc.).  Setting the value 
        /// to false will reject the order, and the user would have to cover the full $10 with an alternate payment 
        /// method.</param>
        /// <param name="items">A list of items that comprise the order</param>
        /// <returns>A response object indicating whether the order was charged successfully and 
        /// the final amount paid including the customer specified tip amount.</returns>
        /// <exception cref="LevelUpApiException"> The returned HTTP status code for the request was something other 
        /// than 200 (OK)</exception>
        CompletedOrderResponse CompleteProposedOrder(   string accessToken,
                                                        int locationId,
                                                        string qrPaymentData,
                                                        string proposedOrderUuid,
                                                        int spendAmountCents,
                                                        int taxAmountCents,
                                                        int exemptionAmountCents,
                                                        int appliedDiscountAmountCents,
                                                        string register = null,
                                                        string cashier = null,
                                                        string identifierFromMerchant = null,
                                                        string receiptMessageHtml = null,
                                                        bool partialAuthorizationAllowed = true,
                                                        IList<Item> items = null);
    }
}
