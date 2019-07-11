#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="GiftCardCreditQueryRequest.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
    public class GrantMerchantFundedCreditRequest : Request
    {
        protected override LevelUpApiVersion _applicableAPIVersionsBitmask => LevelUpApiVersion.v15;

        /// <summary>
        /// A Serializable http body for the GrantMerchantFundedCreditRequest
        /// </summary>
        public GrantMerchantFundedCreditRequestBody Body { get; }

        /// <summary>
        /// Creates a add value request for a merchant funded credit through LevelUp
        /// </summary>
        public GrantMerchantFundedCreditRequest(
            string accessToken, 
            string email,
            int durationInSeconds,
            int merchantId,
            string message,
            int valueAmount,
            bool global)
            : base(accessToken)
        {
            Body = new GrantMerchantFundedCreditRequestBody(email, durationInSeconds, merchantId, message, valueAmount, global);
        }
    }
}
