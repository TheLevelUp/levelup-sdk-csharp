#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="ICreateDetachedRefund.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using LevelUp.Api.Client.Models.Requests;
using LevelUp.Api.Client.Models.Responses;
using LevelUp.Api.Http;

namespace LevelUp.Api.Client.ClientInterfaces
{
    public interface ICreateDetachedRefund : ILevelUpClientModule
    {
        /// <summary>
        /// Creates a new detached refund.  A detached refund is a way of granting credit to a customer in cases where 
        /// the point of sale does not support voiding payments.
        /// </summary>
        /// <remarks>
        /// Warning: this credit is not attached to the original order should ONLY be used if standard refunds cannot 
        /// be implemented or used in certain cases.
        /// </remarks>
        /// <param name="accessToken">The LevelUp access token associated with the merchant account obtained from 
        /// the Authenticate() method</param>
        /// <param name="locationId">LevelUp Location ID that is granting the refund</param>
        /// <param name="qrPaymentData">The customer's scanned QR code payment data</param>
        /// <param name="creditAmountCents">The amount of credit to give to the refundee</param>
        /// <param name="register">An identifier indicating which register where the refund is from.</param>
        /// <param name="cashier">The name of the cashier or server who handled the refund if available.</param>
        /// <param name="identifierFromMerchant">The POS-specific order ID or number for the check, which will be 
        /// used to resolve possible duplicate refunds. This should be the POS internal order number in most cases.</param>
        /// <param name="managerConfirmation">The manager confirmation code, if the merchant requires one.</param>
        /// <param name="customerFacingReason">Customer facing reason for the refund.  The email confirmation to the
        /// user will contain the reason.</param>
        /// <param name="internalReason">Internal reason for the refund.  The customer will not be see this reason.</param>
        /// <returns>Information about the newly created detached refund</returns>
        /// <exception cref="LevelUpApiException"> The returned HTTP status code for the request was something other 
        /// than 200 (OK)</exception>
        DetachedRefundResponse CreateDetachedRefund(string accessToken,
            int locationId,
            string qrPaymentData,
            int creditAmountCents,
            string register = null,
            string cashier = null,
            string identifierFromMerchant = null,
            string managerConfirmation = null,
            string customerFacingReason = null,
            string internalReason = null);

        /// <summary>
        /// Creates a new detached refund.  A detached refund is a way of granting credit to a customer in cases where 
        /// the point of sale does not support voiding payments.
        /// </summary>
        /// <remarks>
        /// Warning: this credit is not attached to the original order should ONLY be used if standard refunds cannot 
        /// be implemented or used in certain cases.
        /// </remarks>
        /// <param name="accessToken">The LevelUp access token associated with the merchant account obtained from 
        /// the Authenticate() method</param>
        /// <param name="detachedRefund">Data about the detached refund to create</param>
        /// <returns>Information about the newly created detached refund</returns>
        /// <exception cref="LevelUpApiException"> The returned HTTP status code for the request was something other 
        /// than 200 (OK) </exception>
        DetachedRefundResponse CreateDetachedRefund(string accessToken, DetachedRefundRequestBody detachedRefund);
    }
}
