//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="DetachedRefund.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

namespace LevelUp.Api.Client.Models.Requests
{
    /// <summary>
    /// Request to create a detached refund
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class DetachedRefund
    {
        private DetachedRefund()
        {
            // for serialization purposes
           DetachedRefundContainer = new DetachedRefundContainer(); 
        }

        /// <summary>
        /// Constructor a detached refund
        /// </summary>
        /// <param name="locationId">The identification number for the origin of the detached refund</param>
        /// <param name="qrPaymentData">The customer's QR code payment data as a string</param>
        /// <param name="creditAmountCents">The amount of credit to give the user</param>
        /// <param name="register">An identifier indicating which register where the refund is from. 
        /// Default is null</param>
        /// <param name="cashier">The name of the cashier or server who handled the refund if available. 
        /// Default is null</param>
        /// <param name="identifierFromMerchant">An unique order identifier specific to the POS system which will be 
        /// used to resolved possible duplicate refunds. This should be the POS internal order number in most cases. 
        /// e.g. An order number that servers would call for the customer to pick up their order. 
        /// Default is null</param>
        /// <param name="managerConfirmation">Confirmation from the manager.  Default is null</param>
        /// <param name="customerFacingReason">Customer facing reason for the refund.  The email confirmation to the
        /// user will contain the reason.  Default is null</param>
        /// <param name="internalReason">Internal reason for the refund.  The customer will not be see this reason.
        /// Default is null</param>
        public DetachedRefund(int locationId, 
                              string qrPaymentData, 
                              int creditAmountCents, 
                              string register = null, 
                              string cashier = null, 
                              string identifierFromMerchant = null, 
                              string managerConfirmation = null, 
                              string customerFacingReason = null, 
                              string internalReason = null)
        {
            DetachedRefundContainer = new DetachedRefundContainer(locationId,
                                                                  qrPaymentData,
                                                                  creditAmountCents,
                                                                  register,
                                                                  cashier,
                                                                  identifierFromMerchant,
                                                                  managerConfirmation,
                                                                  customerFacingReason,
                                                                  internalReason);
        }

        public string Cashier { get { return DetachedRefundContainer.Cashier; } }

        public int CreditAmountCents { get { return DetachedRefundContainer.CreditAmountCents; } }

        public string CustomerFacingReason { get { return DetachedRefundContainer.CustomerFacingReason; } }

        public string Identifier { get { return DetachedRefundContainer.Identifier; } }

        public string InternalReason { get { return DetachedRefundContainer.InternalReason; } }

        public int LocationId { get { return DetachedRefundContainer.LocationId; } }

        public string ManagerConfirmation { get { return DetachedRefundContainer.ManagerConfirmation; } }

        public string PaymentData { get { return DetachedRefundContainer.PaymentData; } }

        public string Register { get { return DetachedRefundContainer.Register; } }

        /// <summary>
        /// This container is used to aid in correct JSON serialization
        /// </summary>
        [JsonProperty(PropertyName = "detached_refund")]
        private DetachedRefundContainer DetachedRefundContainer { get; set; }
    }
}
