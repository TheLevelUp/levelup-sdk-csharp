using RestSharp;
using System;
using System.Collections.Generic;

namespace LevelUpApi
{
    internal static class HttpRest
    {
        public enum ContentType
        {
            Xml,
            UrlEncoded,
            Json,
        }

        internal static readonly Dictionary<ContentType, string> ContentTypeStrings = new Dictionary<ContentType, string>
            {
                {ContentType.Xml, "application/xml"},
                {ContentType.UrlEncoded, "application/x-www-form-urlencoded"},
                {ContentType.Json, "application/json"},
            };

        internal static IRestRequest BuildRequest(Method requestMethod = Method.GET,
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

        internal static string CreateCombinedUrl(string baseUrl, string urlSuffix)
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

        internal static IRestResponse MakeRequest(string uri, IRestRequest request, string userAgent = "")
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