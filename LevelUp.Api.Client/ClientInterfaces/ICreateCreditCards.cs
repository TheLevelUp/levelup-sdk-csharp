#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="ICreateCreditCards.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
    public interface ICreateCreditCards : ILevelUpClientModule
    {
        /// <summary>
        /// Associates a new credit card with the user's account.  Regardless of whether or not the user has an existing
        /// payment instrument associated with their account, this new card will become the primary payment method 
        /// for that user.
        /// </summary>
        /// <remarks>
        /// All credit card data must be encrypted client-side before it is sent over the API. For instructions on
        /// how to do this, email developer@thelevelup.com and we can point you to some resources for your specific 
        /// use-case.
        /// </remarks>
        /// <param name="accessToken">Access token associated with the user's account.</param>
        /// <param name="encryptedNumber">The card number, encrypted with LevelUp's Braintree public key</param>
        /// <param name="encryptedExpirationMonth">The month in which the card expires, encrypted with LevelUp's 
        /// Braintree public key</param>
        /// <param name="encryptedExpirationYear">The year in which the card expires, encrypted with LevelUp's Braintree 
        /// public key</param>
        /// <param name="encryptedCvv">The card verification value, encrypted with LevelUp's Braintree public key</param>
        /// <param name="postalCode">The postal code portion of the card's billing address (not encrypted)</param>
        /// <returns>Information about the newly added credit card</returns>
        /// <exception cref="LevelUpApiException"> The returned HTTP status code for the request was something other 
        /// than 200 (OK) </exception>
        CreditCard CreateCreditCard(string accessToken, string encryptedNumber, string encryptedExpirationMonth,
            string encryptedExpirationYear, string encryptedCvv, string postalCode);

        /// <summary>
        /// Associates a new credit card with the user's account.  Regardless of whether or not the user has an existing
        /// payment instrument associated with their account, this new card will become the primary payment method 
        /// for that user.
        /// </summary>
        /// <remarks>
        /// All credit card data must be encrypted client-side before it is sent over the API. For instructions on
        /// how to do this, email developer@thelevelup.com and we can point you to some resources for your specific 
        /// use-case.
        /// </remarks>
        /// <param name="accessToken">Access token associated with the user's account.</param>
        /// <param name="createCreditCard">The credit card to be added</param>
        /// <returns>Information about the newly added credit card</returns>
        /// <exception cref="LevelUpApiException">The returned HTTP status code for the request was something other 
        /// than 200 (OK)</exception>
        CreditCard CreateCreditCard(string accessToken, CreateCreditCardRequestBody createCreditCard);

    }
}
