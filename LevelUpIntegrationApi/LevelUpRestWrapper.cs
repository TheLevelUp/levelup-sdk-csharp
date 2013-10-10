using RestSharp;
using System.Collections.Generic;

namespace LevelUpApi
{
    internal static class LevelUpRestWrapper
    {
        private const string DEFAULT_USER_AGENT = "LevelUp-C#IntegrationSDK/v0.1";

        #region GET

        internal static IRestResponse Get(string url,
                                          string accessToken = "")
        {
            return LevelUpRestWrapper.Get(url, accessToken, DEFAULT_USER_AGENT);
        }

        internal static IRestResponse Get(string url,
                                          string accessToken,
                                          string userAgent)
        {
            return HttpRest.MakeRequest(url, BuildLevelUpRequest(Method.GET, string.Empty, accessToken), userAgent);
        }

        #endregion GET

        #region POST

        internal static IRestResponse Post(string url,
                                           string body,
                                           string accessToken = "")
        {
            return LevelUpRestWrapper.Post(url, body, accessToken, DEFAULT_USER_AGENT);
        }

        internal static IRestResponse Post(string url,
                                           string body,
                                           string accessToken,
                                           string userAgent)
        {
            return HttpRest.MakeRequest(url, BuildLevelUpRequest(Method.POST, body, accessToken), userAgent);
        }

        #endregion POST

        private static IRestRequest BuildLevelUpRequest(Method requestMethod = Method.GET,
                                                        string body = "",
                                                        string accessToken = "")
        {
            return LevelUpRestWrapper.BuildLevelUpRequest(requestMethod, null, body, accessToken);
        }

        private static IRestRequest BuildLevelUpRequest(Method requestMethod = Method.GET,
                                                        Dictionary<string, string> headers = null,
                                                        string body = "",
                                                        string accessToken = "")
        {
            AddLevelUpStandardHeaders(ref headers, accessToken);

            return HttpRest.BuildRequest(requestMethod, headers, null, body);
        }

        private static void AddAccessTokenHeader(ref Dictionary<string, string> headers,
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

        private static void AddLevelUpStandardHeaders(ref Dictionary<string, string> headers,
                                                      string levelUpAccessToken = "")
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

            AddAccessTokenHeader(ref headers, levelUpAccessToken);
        }
    }
}
