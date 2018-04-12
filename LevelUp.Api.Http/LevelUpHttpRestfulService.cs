#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="LevelUpHttpRestfulService.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using System.Collections.Generic;
using RestSharp;

namespace LevelUp.Api.Http
{
    public class LevelUpHttpRestfulService : IRestfulService
    {
        // To enable unit testing, the creator of the object can specify a mock method to use in place of
        // RestSharpUtils.MakeRequest to use as part of the various REST methods.
        internal delegate IRestResponse ExecuteRequest(string url, IRestRequest request, string userAgent);
        private ExecuteRequest _executeFunc;

        public LevelUpHttpRestfulService()
        {
            _executeFunc = RestSharpUtils.MakeRequest;
        }

        /// <summary>
        /// Internal constructor for unit testing.
        /// </summary>
        internal LevelUpHttpRestfulService(ExecuteRequest executeFunc)
        {
            _executeFunc = executeFunc;
        }

        public IRestResponse Execute(string url, IRestRequest request, string userAgent = "")
        {
            return _executeFunc(url, request, userAgent);
        }

        #region REST Methods

        public IRestResponse Delete(string url, string accessTokenHeader, string userAgent)
        {
            IRestRequest request = BuildLevelUpRequest( requestMethod: Method.DELETE,
                                                        body: string.Empty,
                                                        accessTokenHeader: accessTokenHeader);

            return _executeFunc(url, request, userAgent);
        }

        public IRestResponse Get(string url, string accessTokenHeader, string userAgent)
        {
            IRestRequest request = BuildLevelUpRequest( requestMethod: Method.GET,
                                                        body: string.Empty,
                                                        accessTokenHeader: accessTokenHeader);

            return _executeFunc(url, request, userAgent);
        }

        public IRestResponse Post(string url, string body, string accessTokenHeader, string userAgent)
        {
            IRestRequest request = BuildLevelUpRequest( requestMethod: Method.POST,
                                                        body: body,
                                                        accessTokenHeader: accessTokenHeader);

            return _executeFunc(url, request, userAgent);
        }

        public IRestResponse Put(string url, string body, string accessTokenHeader, string userAgent)
        {
            IRestRequest request = BuildLevelUpRequest( requestMethod: Method.PUT,
                                                        body: body,
                                                        accessTokenHeader: accessTokenHeader);

            return _executeFunc(url, request, userAgent);
        }

        #endregion REST Methods

        #region Helper Methods

        private static IRestRequest BuildLevelUpRequest(Method requestMethod,
                                                        string body,
                                                        string accessTokenHeader,
                                                        Dictionary<string, string> headers = null)
        {
            headers = headers ?? new Dictionary<string, string>();

            AddJsonHeaders(headers);

            AddAccessTokenHeader(headers, accessTokenHeader);

            return RestSharpUtils.BuildRequest(requestMethod, headers, null, body);
        }

        private static void AddAccessTokenHeader(Dictionary<string, string> headers, string accessTokenString)
        {
            if (!string.IsNullOrEmpty(accessTokenString))
            {
                headers["Authorization"] = accessTokenString;
            }
        }

        private static void AddJsonHeaders(Dictionary<string, string> headers)
        {
            headers["Accept"] = RestSharpUtils.ContentTypeStrings[RestSharpUtils.ContentType.Json];
            headers["Content-Type"] = RestSharpUtils.ContentTypeStrings[RestSharpUtils.ContentType.Json];
        }

        #endregion Helper Methods
    }
}
