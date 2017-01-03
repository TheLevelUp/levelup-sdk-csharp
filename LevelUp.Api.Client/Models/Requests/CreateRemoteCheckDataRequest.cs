#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="CreateRemoteCheckDataRequest.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
    public class CreateRemoteCheckDataRequest : Request
    {
        protected override LevelUpApiVersion _applicableAPIVersionsBitmask
        {
            get { return LevelUpApiVersion.v15; }
        }

        /// <summary>
        /// A Serializable http body for the CreateRemoteCheckDataRequest
        /// </summary>
        public RemoteCheckDataRequestBody Body { get { return _body; } }
        private readonly RemoteCheckDataRequestBody _body;

        /// <summary>
        /// A model of a request to update the check data in the remote datastore
        /// </summary>
        /// <param name="accessToken">App access token for the request</param>
        /// <param name="locationId">The LevelUp location id that is the source of this request</param>
        /// <param name="spendAmountCents">Amount in cents to charge the customer</param>
        /// <param name="taxAmountCents">Tax portion in cents of this spend amount.
        /// NOTE: This will not always be the same as the total tax on the check</param>
        /// <param name="exemptionAmountCents">Exemption amount in cents relevant to this spend.
        /// NOTE: This will not always be the same as the total exemption amount for this check</param>
        /// <param name="identifierFromMerchant">An identifier for the check used by the merchant's system.
        /// This will usually be a check number or check id. It should uniquely identify the check for this merchant</param>
        /// <param name="register">The register identifier or name for this transaction</param>
        /// <param name="cashier">The cashier name for this transaction</param>
        /// <param name="partialAuthorizationAllowed">A flag to allow LevelUp to respond with an authorized amount
        /// that is less than the requested spend amount. If this is set to false and the customer is not able to pay
        /// for the full spend amount, the payment will be rejected outright</param>
        /// <param name="items">A list of the items on the check</param>
        public CreateRemoteCheckDataRequest(string accessToken,
                                            int locationId,
                                            int spendAmountCents,
                                            int taxAmountCents,
                                            int exemptionAmountCents,
                                            string identifierFromMerchant = null,
                                            string register = null,
                                            string cashier = null,
                                            bool partialAuthorizationAllowed = true,
                                            IList<Item> items = null)
            : base(accessToken)
        {
            _body = new RemoteCheckDataRequestBody(locationId, spendAmountCents, taxAmountCents,
                exemptionAmountCents, identifierFromMerchant, register, cashier, partialAuthorizationAllowed, items);
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
