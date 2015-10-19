//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="Loyalties.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using LevelUp.Api.Client.Models.Responses;
using Newtonsoft.Json;
using RestSharp;

namespace LevelUp.Api.Client
{
    public partial class LevelUpClient
    {
        /// <summary>
        /// Gets details about a loyalty - i.e. the relationship between a user and a merchant. 
        /// If a user has no existing loyalty with the merchant, an "empty" loyalty is returned
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method</param>
        /// <param name="merchantId">Identifies the merchant to get the loyalty info for</param>
        /// <returns>Detailed info about the loyalty</returns>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        public Loyalty GetLoyalty(string accessToken, int merchantId)
        {
            string uri = _endpoints.Loyalty(merchantId);

            IRestResponse response = _restService.Get(uri, accessToken, Identifier.ToString());

            ThrowExceptionOnBadResponseCode(response);

            return JsonConvert.DeserializeObject<Loyalty>(response.Content);
        }
    }
}
