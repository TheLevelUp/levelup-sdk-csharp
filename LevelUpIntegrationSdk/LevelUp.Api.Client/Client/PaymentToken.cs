//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="PaymentToken.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using LevelUp.Api.Client.Models.Responses;
using Newtonsoft.Json;
using RestSharp;

namespace LevelUp.Api.Client
{
    public partial class LevelUpClient
    {
        /// <summary>
        /// Retrieve a user's active payment token
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method</param>
        /// <returns>Current payment token for the user</returns>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        public PaymentToken GetPaymentToken(string accessToken)
        {
            string paymentTokenUri = _endpoints.PaymentToken();

            IRestResponse response = _restService.Get(paymentTokenUri, accessToken, Identifier.ToString());

            ThrowExceptionOnBadResponseCode(response);

            return JsonConvert.DeserializeObject<PaymentToken>(response.Content);
        }
    }
}
