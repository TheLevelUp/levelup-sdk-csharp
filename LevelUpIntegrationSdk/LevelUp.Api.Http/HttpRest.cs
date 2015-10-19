//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="HttpRest.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using System;
using System.Collections.Generic;
using RestSharp;

namespace LevelUp.Api.Http
{
    public static class HttpRest
    {
        public enum ContentType
        {
            Xml,
            UrlEncoded,
            Json,
        }

        public static readonly Dictionary<ContentType, string> ContentTypeStrings = new Dictionary<ContentType, string>
            {
                {ContentType.Xml, "application/xml"},
                {ContentType.UrlEncoded, "application/x-www-form-urlencoded"},
                {ContentType.Json, "application/json"},
            };

        public static IRestRequest BuildRequest(Method requestMethod = Method.GET,
                                                Dictionary<string, string> headers = null,
                                                Dictionary<string, string> parameters = null,
                                                string body = "",
                                                ContentType bodyContentType = ContentType.Json)
        {
            RestRequest request = new RestRequest(requestMethod);

            if (null != headers)
            {
                foreach (string key in headers.Keys)
                {
                    request.AddHeader(key, headers[key]);
                }
            }

            if (null != parameters)
            {
                foreach (string key in parameters.Keys)
                {
                    request.AddParameter(key, parameters[key]);
                }
            }

            if (!string.IsNullOrEmpty(body))
            {
                request.AddParameter(ContentTypeStrings[bodyContentType], body, ParameterType.RequestBody);
            }

            return request;
        }

        public static string CreateCombinedUrl(string baseUrl, string urlSuffix)
        {
            Uri combinedUri;
            if (!Uri.TryCreate(new Uri(baseUrl), urlSuffix, out combinedUri))
            {
                throw new ArgumentException(string.Format("Unable to create URI from {0} and {1}!",
                                                          baseUrl,
                                                          urlSuffix));
            }

            return combinedUri.ToString();
        }

        public static string CreateCombinedUrlFormatted(string baseUrl,
                                                        string urlSuffixFormatted,
                                                        params object[] args)
        {
            return CreateCombinedUrl(baseUrl, string.Format(urlSuffixFormatted, args));
        }

        public static IRestResponse MakeRequest(string uri, IRestRequest request, string userAgent = "")
        {
            Uri temp;
            if (!Uri.TryCreate(uri, UriKind.Absolute, out temp))
            {
                throw new ArgumentException(string.Format("{0} is not a well formed URI. Cannot make request!", uri));
            }

            RestClient client = new RestClient(uri);

            //If a user agent is set, override default RestSharp user agent
            if (!string.IsNullOrEmpty(userAgent))
            {
                client.UserAgent = userAgent;
            }

            return client.Execute(request) as RestResponse;
        }
    }
}
