//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="LevelUpRestWrapper.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using RestSharp;

namespace LevelUp.Api.Http
{
    public class LevelUpRestWrapper : IRestfulService
    {
        private const string DEFAULT_USER_AGENT = "LevelUp-C#IntegrationSDK/v1.1";

        public IRestResponse Execute(string url, IRestRequest request, string userAgent = "")
        {
            return HttpRest.MakeRequest(url, request, userAgent);
        }

        #region REST Methods

        #region DELETE

        public IRestResponse Delete(string url,
                                    string accessToken = "")
        {
            return Delete(url, accessToken, DEFAULT_USER_AGENT);
        }

        public IRestResponse Delete(string url,
                                    string accessToken,
                                    string userAgent)
        {
            return Execute(url, BuildLevelUpRequest(Method.DELETE, string.Empty, accessToken), userAgent);
        }

        #endregion DELETE

        #region GET

        public IRestResponse Get(string url,
                                 string accessToken = "")
        {
            return Get(url, accessToken, DEFAULT_USER_AGENT);
        }

        public IRestResponse Get(string url,
                                 string accessToken,
                                 string userAgent)
        {
            return Execute(url, BuildLevelUpRequest(Method.GET, string.Empty, accessToken), userAgent);
        }

        #endregion GET

        #region POST

        public IRestResponse Post(string url,
                                  string body,
                                  string accessToken = "")
        {
            return Post(url, body, accessToken, DEFAULT_USER_AGENT);
        }

        public IRestResponse Post(string url,
                                  string body,
                                  string accessToken,
                                  string userAgent)
        {
            return Execute(url, BuildLevelUpRequest(Method.POST, body, accessToken), userAgent);
        }

        #endregion POST

        #region PUT

        public IRestResponse Put(string url,
                                 string body,
                                 string accessToken = "")
        {
            return Put(url, body, accessToken, DEFAULT_USER_AGENT);
        }

        public IRestResponse Put(string url,
                                 string body,
                                 string accessToken,
                                 string userAgent)
        {
            return Execute(url, BuildLevelUpRequest(Method.PUT, body, accessToken), userAgent);
        }

        #endregion PUT

        #endregion REST Methods

        #region Helper Methods

        private IRestRequest BuildLevelUpRequest(Method requestMethod = Method.GET,
                                                 string body = "",
                                                 string accessToken = "")
        {
            return BuildLevelUpRequest(requestMethod, null, body, accessToken);
        }

        private IRestRequest BuildLevelUpRequest(Method requestMethod = Method.GET,
                                                 Dictionary<string, string> headers = null,
                                                 string body = "",
                                                 string accessToken = "")
        {
            AddLevelUpStandardHeaders(ref headers);
            AddAccessTokenHeader(ref headers, accessToken);

            return HttpRest.BuildRequest(requestMethod, headers, null, body);
        }

        private void AddAccessTokenHeader(ref Dictionary<string, string> headers,
                                          string levelUpAccessToken)
        {
            const string authorizationHeaderString = "Authorization";

            if (!string.IsNullOrEmpty(levelUpAccessToken))
            {
                string formattedAccessTokenString = string.Format("token {0}", levelUpAccessToken);

                if (null == headers)
                {
                    headers = new Dictionary<string, string>();
                }

                if (headers.ContainsKey(authorizationHeaderString))
                {
                    headers[authorizationHeaderString] = formattedAccessTokenString;
                }
                else
                {
                    headers.Add(authorizationHeaderString, formattedAccessTokenString);
                }
            }
        }

        private void AddLevelUpStandardHeaders(ref Dictionary<string, string> headers)
        {
            const string contentTypeHeaderString = "Content-Type";
            const string acceptHeaderString = "Accept";

            if (null == headers)
            {
                headers = new Dictionary<string, string>();
            }

            if (headers.ContainsKey(acceptHeaderString))
            {
                headers[acceptHeaderString] = HttpRest.ContentTypeStrings[HttpRest.ContentType.Json];
            }
            else
            {
                headers.Add(acceptHeaderString, HttpRest.ContentTypeStrings[HttpRest.ContentType.Json]);
            }

            if (headers.ContainsKey(contentTypeHeaderString))
            {
                headers[contentTypeHeaderString] = HttpRest.ContentTypeStrings[HttpRest.ContentType.Json];
            }
            else
            {
                headers.Add(contentTypeHeaderString, HttpRest.ContentTypeStrings[HttpRest.ContentType.Json]);
            }
        }

        #endregion Helper Methods
    }
}
