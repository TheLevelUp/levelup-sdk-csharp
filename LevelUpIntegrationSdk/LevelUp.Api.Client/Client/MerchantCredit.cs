//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="MerchantCredit.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using System.Collections.Generic;
using LevelUp.Api.Client.Models.Requests;
using LevelUp.Api.Client.Models.Responses;
using LevelUp.Api.Http;
using Newtonsoft.Json;
using RestSharp;

namespace LevelUp.Api.Client
{
    public partial class LevelUpClient
    {
        /// <summary>
        /// Gets the amount of credit the user has at the current location
        /// </summary>
        /// <param name="accessToken">The LevelUp accesstoken obtained from the Authenticate() method</param>
        /// <param name="locationId">Identifies the location for which to return the details</param>
        /// <param name="qrData">The customer's QR code payment data as a string</param>
        /// <returns>Details about the amount of credit available</returns>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        public MerchantFundedCreditResponse GetMerchantFundedCredit(string accessToken, int locationId, string qrData)
        {
            string merchantCreditQueryUri = _endpoints.MerchantCredit(locationId, qrData);

            IRestResponse response = _restService.Get(merchantCreditQueryUri, accessToken, Identifier.ToString());

            ThrowExceptionOnBadResponseCode(response);

            return JsonConvert.DeserializeObject<MerchantFundedCreditResponse>(response.Content);
        }

        /// <summary>
        /// Gets the amount of credit the user has at the current location based on the items on the check
        /// </summary>
        /// <param name="accessToken">The LevelUp accesstoken obtained from the Authenticate() method</param>
        /// <param name="locationId">Identifies the location for which to return the details</param>
        /// <param name="qrData">The customer's QR code payment data as a string</param>
        /// <param name="identifierFromMerchant">An unique order identifier specific to the POS system which will be 
        /// used to resolved possible duplicate orders. This should be the POS internal order number in most cases. 
        /// e.g. An order number that servers would call for the customer to pick up their order. Default is null</param>
        /// <param name="items">A list of items that comprise the order</param>
        public MerchantFundedCreditResponse GetMerchantFundedCredit(string accessToken,
                                                                    int locationId,
                                                                    string qrData,
                                                                    string identifierFromMerchant,
                                                                    IList<Item> items)
        {
            string merchantCreditQueryUri = _endpoints.MerchantCredit(locationId, qrData);

            string body = JsonConvert.SerializeObject(new MerchantCreditQuery(qrData, identifierFromMerchant, items));

            IRestResponse response = _restService.Post(merchantCreditQueryUri, body, accessToken, Identifier.ToString());

            ThrowExceptionOnBadResponseCode(response);

            return JsonConvert.DeserializeObject<MerchantFundedCreditResponse>(response.Content);
        }
    }
}
