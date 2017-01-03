#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="CreateCreditCardRequest.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using LevelUp.Api.Client.Models.RequestVisitors;
using LevelUp.Api.Http;

namespace LevelUp.Api.Client.Models.Requests
{
    /// <summary>
    /// Request to add a new credit to a user's account.  The credit card information must be encrypted with
    /// LevelUp's Braintree public key
    /// </summary>
    public class CreateCreditCardRequest : Request
    {
        protected override LevelUpApiVersion _applicableAPIVersionsBitmask
        {
            get { return LevelUpApiVersion.v14 | LevelUpApiVersion.v15; }
        }

        /// <summary>
        /// A Serializable http body for the CreateCreditCardRequest
        /// </summary>
        public CreateCreditCardRequestBody Body { get { return _body; } }
        private readonly CreateCreditCardRequestBody _body;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="accessToken">App access token for the request</param>
        /// <param name="encryptedNumber">The card number, encrypted with LevelUp's Braintree public key</param>
        /// <param name="encryptedExpirationMonth">The month in which the card expires, encrypted with LevelUp's Braintree 
        /// public key</param>
        /// <param name="encryptedExpirationYear">The year in which the card expires, encrypted with LevelUp's Braintree 
        /// public key</param>
        /// <param name="encryptedCvv">The card verification value, encrypted with LevelUp's Braintree public key</param>
        /// <param name="postalCode">The postal code portion of the card's billing address (not encrypted)</param>
        public CreateCreditCardRequest(string accessToken, string encryptedNumber, string encryptedExpirationMonth,
            string encryptedExpirationYear, string encryptedCvv, string postalCode)
            : base(accessToken)
        {
            _body = new CreateCreditCardRequestBody(encryptedNumber, encryptedExpirationMonth, encryptedExpirationYear, encryptedCvv, postalCode);
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
