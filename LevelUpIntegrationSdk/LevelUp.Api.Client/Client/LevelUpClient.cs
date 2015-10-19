//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="LevelUpClient.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using System.IO;
using LevelUp.Api.Http;
using LevelUp.Api.Client.Models;
using LevelUp.Api.Client.Models.Requests;
using LevelUp.Api.Client.Models.Responses;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace LevelUp.Api.Client
{
    public partial class LevelUpClient : ILevelUpClient
    {
        // Link header format is '<nextPageURL>; rel="next"'
        // Regex pattern matches the url between the brackets as long as it is not whitespace
        private const string LINK_HEADER_REGEX_MATCH_PATTERN = @"(?<=\<)[^\s]*(?=\>)";
        private const string DEFAULT_PATH_TO_URI_CONFIG = "LevelUpBaseUri.config";
        private static readonly Regex NEXT_PAGE_REG_EX = new Regex(LINK_HEADER_REGEX_MATCH_PATTERN);

        private readonly LevelUpEndpoints _endpoints;

        private readonly IRestfulService _restService;

        #region Constructors

        internal LevelUpClient(AgentIdentifier identifier,
                               IRestfulService restService,
                               string pathToConfigFile = DEFAULT_PATH_TO_URI_CONFIG)
        {
            Identifier = identifier;
            _restService = restService;

            string baseUriFromConfig = string.IsNullOrEmpty(pathToConfigFile)
                                           ? ReadBaseUri()
                                           : ReadBaseUri(pathToConfigFile);

            _endpoints = string.IsNullOrEmpty(baseUriFromConfig)
                              ? new LevelUpEndpoints()
                              : new LevelUpEndpoints(baseUriFromConfig);
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Base URL for this version of the LevelUp API
        /// </summary>
        public string ApiUrlBase { get { return _endpoints.BaseUri; } }

        /// <summary>
        /// Identifying information for the group or individual developing this software
        /// </summary>
        public AgentIdentifier Identifier { get; set; }

        #endregion Properties

        /// <summary>
        /// Obtain a LevelUp access token 
        /// </summary>
        /// <param name="apiKey">Your LevelUp API key which LevelUp will have sent to you. 
        /// This is your API key</param>
        /// <param name="username">Your LevelUp username</param>
        /// <param name="password">Your LevelUp password</param>
        /// <returns>A LevelUp access token object which includes a .Token member which stores 
        /// the access token you will need to use for subsequent LevelUp calls</returns>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        public AccessToken Authenticate(string apiKey, string username, string password)
        {
            string accessTokenUri = _endpoints.Authentication();

            AccessTokenRequest luTokenRequest = new AccessTokenRequest(apiKey, username, password);

            string body = JsonConvert.SerializeObject(luTokenRequest);

            IRestResponse response = _restService.Post(accessTokenUri, body, string.Empty, Identifier.ToString());

            ThrowExceptionOnBadResponseCode(response);

            return JsonConvert.DeserializeObject<AccessToken>(response.Content);
        }

        /// <summary>
        /// Gets the location details for the specified location id
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method</param>
        /// <param name="locationId">Identifies the location for which to return the details</param>
        /// <returns>Detailed location info for the location specified</returns>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        public LocationDetails GetLocationDetails(string accessToken, int locationId)
        {
            string locationDetailsUri = _endpoints.LocationDetails(locationId);

            IRestResponse response = _restService.Get(locationDetailsUri, accessToken, Identifier.ToString());

            // Special case 404 error since this means the location does not exist, is not visible, 
            // or the merchant owner of the location is not live
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new LevelUpApiException(string.Format("Cannot get location details for location {0}." +
                                                            " This location may not exist, not be visible," +
                                                            " or the merchant owner may not be live.",
                                                            locationId),
                                              response.StatusCode,
                                              response.ErrorException);
            }

            ThrowExceptionOnBadResponseCode(response);

            return JsonConvert.DeserializeObject<LocationDetails>(response.Content);
        }

        /// <summary>
        /// Gets the order details for a given order. This the merchant facing order data so the merchant id is required as well
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method</param>
        /// <param name="merchantId">Identifies the merchant on whose behalf you are querying. 
        /// If the access token specified does not belong to the merchant specified, this method will throw an exception</param>
        /// <param name="orderIdentifier">Identifies the order for which to return the details. This should be the order UUID</param>
        /// <returns>Details for the specified order</returns>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        public OrderDetailsResponse GetMerchantOrderDetails(string accessToken, int merchantId, string orderIdentifier)
        {
            string orderDetailsUri = _endpoints.OrderDetails(merchantId, orderIdentifier);

            IRestResponse response = _restService.Get(orderDetailsUri, accessToken, Identifier.ToString());

            ThrowExceptionOnBadResponseCode(response);

            return JsonConvert.DeserializeObject<OrderDetailsResponse>(response.Content);
        }

        /// <summary>
        /// Lists all locations for a specified merchant
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method</param>
        /// <param name="merchantId">Identifies the merchant for which to return a list of locations</param>
        /// <returns>A list of locations for the specified merchant.</returns>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        public IList<Location> ListLocations(string accessToken, int merchantId)
        {
            string locationsUri = _endpoints.Locations(merchantId);

            IRestResponse response = _restService.Get(locationsUri, accessToken, Identifier.ToString());

            ThrowExceptionOnBadResponseCode(response);

            return JsonConvert.DeserializeObject<List<Location>>(response.Content);
        }

        /// <summary>
        /// Checks for and reads a file that should contain the base Uri to use.
        /// Expected file format: 1 line with format = "https://api.thelevelup.com/"
        /// </summary>
        /// <param name="pathToFile">The path to the file containing the base Uri</param>
        /// <returns>the contents of the file or null if the file does not exist or the first line is empty.</returns>
        private string ReadBaseUri(string pathToFile = DEFAULT_PATH_TO_URI_CONFIG)
        {
            string firstLine = null;

            if (!string.IsNullOrEmpty(pathToFile) &&
                File.Exists(pathToFile))
            {
                string[] contents = File.ReadAllLines(pathToFile);

                if (null != contents && contents.Length > 0)
                {
                    firstLine = contents[0].ToLowerInvariant().Trim();
                }
            }

            return !string.IsNullOrEmpty(firstLine) ? firstLine : null;
        }

        private string ParseNextPageUrl(IEnumerable<Parameter> responseHeadersToParse)
        {
            foreach (Parameter p in responseHeadersToParse)
            {
                if ("Link".Equals(p.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    string linkHeader = p.Value as string;
                    if (!string.IsNullOrEmpty(linkHeader))
                    {
                        return NEXT_PAGE_REG_EX.Match(linkHeader).Value;
                    }
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Throws LevelUp.Api.Client.LevelUpApiException when the HTTP status code of the response is not 200 OK
        /// </summary>
        /// <param name="response">Response to check the response for.</param>
        private void ThrowExceptionOnBadResponseCode(IRestResponse response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new LevelUpApiException(string.Format("Error {0} ({1}). Endpoint \"{2}\" not found!",
                                                                (int) response.StatusCode, 
                                                                response.StatusCode,
                                                                response.ResponseUri));
                }

                List<ErrorResponse> errors = JsonConvert.DeserializeObject<List<ErrorResponse>>(response.Content);
                throw LevelUpApiException.Initialize(errors, response.StatusCode, response.ErrorException);
            }
        }
    }
}
