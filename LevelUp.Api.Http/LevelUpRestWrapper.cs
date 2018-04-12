#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="LevelUpRestWrapper.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using System;
using System.Collections.Generic;
using System.Net;
using LevelUp.Api.Client.Models.Responses;
using Newtonsoft.Json;
using RestSharp;

namespace LevelUp.Api.Http
{
    public class LevelUpRestWrapper
    {
        public delegate void ResponseAction(IRestResponse response);
        private readonly IRestfulService RestService;

        public AgentIdentifier Identifier { get; }

        public LevelUpRestWrapper(IRestfulService restService, AgentIdentifier identifier)
        {
            RestService = restService;
            Identifier = identifier;
        }

        public TResponse Get<TResponse>(
            string uri,
            string accessTokenHeader = null,
            IDictionary<HttpStatusCode, ResponseAction> actions = null)
        {
            IRestResponse response = RestService.Get(uri,accessTokenHeader, Identifier.ToString());

            return DeserializeResponse<TResponse>(HandleResponse(response, actions));
        }

        public PagedList<TResponse> GetWithPaging<TResponse>(
            string uri,
            int currentPageNumber,
            string accessTokenHeader = null)
        {
            string nextPageUrl = uri;

            PagedList<TResponse>.EndpointInvoker endpointInvoker = (url) =>
            {
                IRestResponse response = RestService.Get(url, accessTokenHeader, Identifier.ToString());

                ThrowExceptionOnBadResponseCode(response);

                return response;
            };

            return new PagedList<TResponse>(currentPage: currentPageNumber,
                                            endpointInvoker: endpointInvoker,
                                            pageUrl: nextPageUrl);
        }

        public void Post<TRequest>(
            TRequest request,
            string uri,
            string accessTokenHeader = null,
            IDictionary<HttpStatusCode, ResponseAction> actions = null)
        {
            Post(JsonConvert.SerializeObject(request), uri, accessTokenHeader, actions);
        }

        public TResponse Post<TResponse>(
            string uri,
            string accessTokenHeader = null,
            IDictionary<HttpStatusCode, ResponseAction> actions = null)
        {
            string body = string.Empty;

            return DeserializeResponse<TResponse>(Post(body, uri, accessTokenHeader, actions));
        }

        public TResponse Post<TRequest, TResponse>(
            TRequest request,
            string uri,
            string accessTokenHeader = null,
            IDictionary<HttpStatusCode, ResponseAction> actions = null)
        {
            return DeserializeResponse<TResponse>(
                Post(JsonConvert.SerializeObject(request), uri, accessTokenHeader, actions));
        }

        public TResponse Put<TRequest, TResponse>(
            TRequest request,
            string uri,
            string accessTokenHeader = null,
            IDictionary<HttpStatusCode, ResponseAction> actions = null)
        {
            return DeserializeResponse<TResponse>(
                Put(JsonConvert.SerializeObject(request), uri, accessTokenHeader, actions));
        }

        #region Helper Methods

        private string Post(
            string body,
            string uri,
            string accessTokenHeader = null,
            IDictionary<HttpStatusCode, ResponseAction> actions = null)
        {
            IRestResponse response = RestService.Post(uri, body, accessTokenHeader, Identifier.ToString());

            return HandleResponse(response, actions);
        }

        private string Put(
            string body,
            string uri,
            string accessTokenHeader = null,
            IDictionary<HttpStatusCode, ResponseAction> actions = null)
        {
            IRestResponse response = RestService.Put( uri, body, accessTokenHeader, Identifier.ToString());

            return HandleResponse(response, actions);
        }

        public TResponse Delete<TResponse>(
            string uri,
            string accessTokenHeader = null,
            IDictionary<HttpStatusCode, ResponseAction> actions = null)
        {
            IRestResponse response = RestService.Delete(uri, accessTokenHeader, Identifier.ToString());

            return DeserializeResponse<TResponse>(HandleResponse(response, actions));
        }

        public void Delete(
            string uri,
            string accessTokenHeader = null,
            IDictionary<HttpStatusCode, ResponseAction> actions = null)
        {
            IRestResponse response = RestService.Delete(uri, accessTokenHeader, Identifier.ToString());
            HandleResponse(response, actions);
        }

        private string HandleResponse(
            IRestResponse response,
            IDictionary<HttpStatusCode, ResponseAction> actions = null)
        {
            if (actions != null && actions.ContainsKey(response.StatusCode))
            {
                actions[response.StatusCode](response);
            }

            ThrowExceptionOnBadResponseCode(response);

            return response.Content;
        }

        /// <summary>
        /// Throws LevelUpApi.LevelUpApiException when the HTTP status code of the response is not 200 OK
        /// </summary>
        /// <param name="response">Response to check the response for.</param>
        protected void ThrowExceptionOnBadResponseCode(IRestResponse response)
        {
            List<HttpStatusCode> VALID_STATUS_CODES = new List<HttpStatusCode> { HttpStatusCode.OK,
                                                                                 HttpStatusCode.NoContent };

            if (VALID_STATUS_CODES.Contains(response.StatusCode))
            {
                return;
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new LevelUpApiException($"Error {(int) response.StatusCode} ({response.StatusCode}). " +
                    $"Endpoint \"{response.ResponseUri}\" not found!");
            }

            List<ErrorResponse> errors = DeserializeResponse<List<ErrorResponse>>(response.Content);
            throw new LevelUpApiException(errors, response.StatusCode, response.ErrorException);
        }

        internal static TResponse DeserializeResponse<TResponse>(string value)
        {
            try
            {
                return JsonConvert.DeserializeObject<TResponse>(value);
            }
            catch (JsonReaderException ex)
            {
                throw new LevelUpApiException("Failed to parse the content returned from platform into the " +
                    $"specified response object of type {typeof(TResponse)}{Environment.NewLine}Response Body:" +
                    $"{Environment.NewLine}{value}", ex);
            }
        }

        #endregion
    }
}
