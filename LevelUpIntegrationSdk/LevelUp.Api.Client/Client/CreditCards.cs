//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="CreditCards.cs" company="SCVNGR, Inc. d/b/a LevelUp">
//   Copyright(c) 2014 SCVNGR, Inc. d/b/a LevelUp. All rights reserved.
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

using LevelUp.Api.Client.Models.Requests;
using LevelUp.Api.Client.Models.Responses;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;

namespace LevelUp.Api.Client
{
    public partial class LevelUpClient
    {
        /// <summary>
        /// Associates a new credit card with the user's account.  If the new credit card is the user's only payment
        ///  instrument, it will be automatically promoted. If the user has existing payment instruments, no automatic 
        /// promotion will take place; you must call the PromoteCreditCard method
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method</param>
        /// <param name="creditCard">The credit card to be added</param>
        /// <returns>Information about the newly added credit card</returns>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        public CreditCard CreateCreditCard(string accessToken, CreditCardRequest creditCard)
        {
            string createCreditCardUri = _endpoints.CreditCards();

            // Create the body content of the order request
            string body = JsonConvert.SerializeObject(creditCard);

            IRestResponse response = _restService.Post(createCreditCardUri, body, accessToken, Identifier.ToString());

            ThrowExceptionOnBadResponseCode(response);

            return JsonConvert.DeserializeObject<CreditCard>(response.Content);
        }

        /// <summary>
        /// Deletes a credit card
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method</param>
        /// <param name="creditCardId">The id of the credit card</param>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        public void DeleteCreditCard(string accessToken, int creditCardId)
        {
            string createCreditCardUri = _endpoints.CreditCards(creditCardId);

            IRestResponse response = _restService.Delete(createCreditCardUri, accessToken, Identifier.ToString());

            ThrowExceptionOnBadResponseCode(response);
        }

        /// <summary>
        /// Returns a list of the current user's active credit cards. Inactive cards include deleted cards and 
        /// duplicate cards. These records will not appear in the list.
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method</param>
        /// <returns>List of credit cards associated with the user's account</returns>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        public IList<CreditCard> ListCreditCards(string accessToken)
        {
            string createCreditCardUri = _endpoints.CreditCards();

            IRestResponse response = _restService.Get(createCreditCardUri, accessToken, Identifier.ToString());

            ThrowExceptionOnBadResponseCode(response);

            return JsonConvert.DeserializeObject<List<CreditCard>>(response.Content);
        }

        /// <summary>
        /// Promotes a user's credit card so that it will be used as their preferred payment instrument. Only one 
        /// credit card at a time may be promoted. Promoting a card will demote any other promoted card.
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method</param>
        /// <param name="creditCardId">The id of the credit card</param>
        /// <returns>Information about the promoted credit card</returns>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        public CreditCard PromoteCreditCard(string accessToken, int creditCardId)
        {
            string createCreditCardUri = _endpoints.CreditCards(creditCardId);

            // Create the body content of the order request
            string body = JsonConvert.SerializeObject(createCreditCardUri);

            IRestResponse response = _restService.Put(createCreditCardUri, body, accessToken, Identifier.ToString());

            ThrowExceptionOnBadResponseCode(response);

            return JsonConvert.DeserializeObject<CreditCard>(response.Content);
        }
    }
}
