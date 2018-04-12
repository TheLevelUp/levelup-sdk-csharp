#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="FinalizeRemoteCheckRequest.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using LevelUp.Api.Http;

namespace LevelUp.Api.Client.Models.Requests
{
    public class FinalizeRemoteCheckRequest : Request
    {
        protected override LevelUpApiVersion _applicableAPIVersionsBitmask
        {
            get { return LevelUpApiVersion.v15; }
        }

        public readonly string CheckUuid;

        /// <summary>
        /// A Serializable http body for the FinalizeRemoteCheckRequest
        /// </summary>
        public FinalizeRemoteCheckRequestBody Body { get; }

        /// <summary>
        /// Creates a request to finalize an order where the check data is stored remotely
        /// This request performs the same function as a create order request and is useful
        /// for building an integration for a full service environment
        /// </summary>
        /// <param name="accessToken">App access token for the request</param>
        /// <param name="checkUuid">The unique identifier for the check</param>
        /// <param name="spendAmountCents">The total amount of payment requested (includes appliedDiscountAmount) in cents</param>
        /// <param name="taxAmountCents">The tax amount in cents relevant to this order</param>
        /// <param name="appliedDiscountAmountCents">The discount in cents applied to the check as part of this order operation</param>
        public FinalizeRemoteCheckRequest(string accessToken, string checkUuid, int spendAmountCents, int taxAmountCents = 0, int? appliedDiscountAmountCents = null)
            : base(accessToken)
        {
            CheckUuid = checkUuid;
            Body = new FinalizeRemoteCheckRequestBody(spendAmountCents, taxAmountCents, appliedDiscountAmountCents);
        }
    }
}
