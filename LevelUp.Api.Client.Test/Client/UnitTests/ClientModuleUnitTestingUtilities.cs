#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="ClientModuleUnitTestingUtilities.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using LevelUp.Api.Client.ClientInterfaces;
using LevelUp.Api.Client.Models.Requests;
using LevelUp.Api.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;

namespace LevelUp.Api.Client.Test.Client
{
    internal static class ClientModuleUnitTestingUtilities
    {
        /// <summary>
        /// Gets a LevelUpClient Module interface that will mimic, for any rest call, the provided 
        /// expected RestResponse.  Callers are expected to populate this expected response, including
        /// the json content string, based on what they expect to come over the wire according to the
        /// LevelUp platform API docs. This method will target the LevelUp Sandbox environment.
        /// </summary>
        /// <typeparam name="T">The specific type of ILevelUpClientModule to return.</typeparam>
        /// <typeparam name="U">The expected request model object type.</typeparam>
        /// <param name="expectedResponse">The desired RestResponse that the caller would want platform 
        ///     to produce.  Note that this response will be applied to every rest command. </param>
        /// <param name="expectedRequestBody">If the caller wishes to verify the body data that is
        ///     used in the request, they can do that here.</param>
        /// <param name="expectedAccessToken">If the caller wishes to verify authorization functionality 
        ///     for a specific command, they can provide an expected access token, without which the returned
        ///     rest response will contain an unauthorized status code.</param>
        /// <param name="expectedRequestUrl">If the caller wishes to verify the endpoint that is
        ///     used in the request, they can do that here.</param>
        internal static T GetMockedLevelUpModule<T, U>(RestResponse expectedResponse,
                                                       string expectedRequestBody,
                                                       string expectedAccessToken = null,
                                                       string expectedRequestUrl = null)
            where T : ILevelUpClientModule
            where U : Request
        {
            return GetMockedLevelUpModule<T, U>(expectedResponse,
                                                expectedRequestBody,
                                                expectedAccessToken,
                                                expectedRequestUrl,
                                                LevelUpEnvironment.Sandbox);
        }

        /// <summary>
        /// Gets a LevelUpClient Module interface that will mimic, for any rest call, the provided 
        /// expected RestResponse.  Callers are expected to populate this expected response, including
        /// the json content string, based on what they expect to come over the wire according to the
        /// LevelUp platform API docs.
        /// </summary>
        /// <typeparam name="T">The specific type of ILevelUpClientModule to return.</typeparam>
        /// <typeparam name="U">The expected request model object type.</typeparam>
        /// <param name="expectedResponse">The desired RestResponse that the caller would want platform 
        ///     to produce.  Note that this response will be applied to every rest command. </param>
        /// <param name="expectedRequestBody">If the caller wishes to verify the body data that is
        ///     used in the request, they can do that here.</param>
        /// <param name="expectedAccessToken">If the caller wishes to verify authorization functionality 
        ///     for a specific command, they can provide an expected access token, without which the returned
        ///     rest response will contain an unauthorized status code.</param>
        /// <param name="expectedRequestUrl">If the caller wishes to verify the endpoint that is
        ///     used in the request, they can do that here.</param>
        /// <param name="environmentToUse">Environment to use to build URI of the request. Default value is Sandbox</param>
        internal static T GetMockedLevelUpModule<T, U>(RestResponse expectedResponse,
                                                       string expectedRequestBody,
                                                       string expectedAccessToken,
                                                       string expectedRequestUrl,
                                                       LevelUpEnvironment environmentToUse)
            where T : ILevelUpClientModule
            where U : Request
        {
            // This is not a very smart mock, in that every type of request will generate the same 
            // response -- GET/POST/PUT/DELETE all just call this delegate.
            var returnsExpectedResponse = new Func<string, string, string, string, IRestResponse>(
                (string url, string body, string accessToken, string userAgent) =>
                    {
                        if (!string.IsNullOrEmpty(expectedRequestUrl) && expectedRequestUrl != url)
                        {
                            Assert.Fail("The specified expected url [{0}] does not match " +
                                        "the url generated in the code [{1}].", expectedRequestUrl, url);
                        }

                        if (!string.IsNullOrEmpty(expectedRequestBody))
                        {
                            Type Q = typeof(U).GetProperty("Body")?.PropertyType ?? typeof(U);
                            TestUtilities.VerifyJsonIsEquivalent(expectedRequestBody, body, Q);
                        }

                        if (expectedAccessToken != null && !string.IsNullOrEmpty(accessToken) &&
                            !accessToken.Contains(expectedAccessToken))
                        {
                            Assert.Fail("The specified expected access token [{0}] does not match " +
                                        "the access token generated in code [{1}].", expectedAccessToken, accessToken);
                        }

                        return expectedResponse;
                    });

            return GenerateMockLevelUpModuleWhereEveryHttpVerbDoesTheSameThing<T>(returnsExpectedResponse,
                                                                                  environmentToUse);
        }

        /// <summary>
        /// Gets a LevelUpClient Module interface that will mimic, for any rest call, the provided 
        /// expected RestResponse.  Callers are expected to populate this expected response, including
        /// the json content string, based on what they expect to come over the wire according to the
        /// LevelUp platform API docs. This method will target the LevelUp Sandbox environment.
        /// </summary>
        /// <typeparam name="T">The specific type of ILevelUpClientModule to return.</typeparam>
        /// <param name="expectedResponse">The desired RestResponse that the caller would want platform 
        /// to produce.  Note that this response will be applied to every rest command. </param>
        /// <param name="expectedRequestUrl">If the caller wishes to verify the endpoint that is
        /// used in the request, they can do that here.</param>
        /// <param name="expectedAccessToken">If the caller wishes to verify authorization functionality 
        /// for a specific command, they can provide an expected access token, without which the returned
        /// rest response will contain an unauthorized status code.</param>
        internal static T GetMockedLevelUpModule<T>(RestResponse expectedResponse,
                                                    string expectedRequestUrl = null,
                                                    string expectedAccessToken = null)
            where T : ILevelUpClientModule
        {
            return GetMockedLevelUpModule<T>(expectedResponse,
                                             expectedRequestUrl,
                                             expectedAccessToken,
                                             LevelUpEnvironment.Sandbox);
        }

        /// <summary>
        /// Gets a LevelUpClient Module interface that will mimic, for any rest call, the provided 
        /// expected RestResponse.  Callers are expected to populate this expected response, including
        /// the json content string, based on what they expect to come over the wire according to the
        /// LevelUp platform API docs.
        /// </summary>
        /// <typeparam name="T">The specific type of ILevelUpClientModule to return.</typeparam>
        /// <param name="expectedResponse">The desired RestResponse that the caller would want platform 
        /// to produce.  Note that this response will be applied to every rest command. </param>
        /// <param name="expectedRequestUrl">If the caller wishes to verify the endpoint that is
        /// used in the request, they can do that here.</param>
        /// <param name="expectedAccessToken">If the caller wishes to verify authorization functionality 
        /// for a specific command, they can provide an expected access token, without which the returned
        /// rest response will contain an unauthorized status code.</param>
        /// <param name="environmentToUse">Environment to use to build URI of the request.</param>
        internal static T GetMockedLevelUpModule<T>(RestResponse expectedResponse,
                                                    string expectedRequestUrl,
                                                    string expectedAccessToken,
                                                    LevelUpEnvironment environmentToUse)
            where T : ILevelUpClientModule
        {
            return GetMockedLevelUpModule<T, Request>(expectedResponse,
                                                      null,
                                                      expectedAccessToken: expectedAccessToken,
                                                      expectedRequestUrl: expectedRequestUrl,
                                                      environmentToUse: environmentToUse);
        }

        /// <summary>
        /// Gets a LevelUpClient Module interface that will mimic, for any rest call, the provided expected RestResponses
        /// using a paging format.  This mock will handle adding paging link headers to the returned RestResponse 
        /// objects and serving up the subsequent RestResponse when those links are used.  This method will target the 
        /// LevelUp Sandbox environment.
        /// </summary>
        /// <typeparam name="T">A LevelUpModule type that uses paging.</typeparam>
        /// <param name="expectedResponses">A collection of expected response objects, which will each be treated
        /// as a page, in the correct paging order.</param>
        /// <param name="requestUrlBase">The url that is expected to retrieve the first page.</param>
        internal static T GetMockedLevelUpModuleWithPaging<T>(RestResponse[] expectedResponses,
                                                              string requestUrlBase)
            where T : IQueryOrders
        {
            return GetMockedLevelUpModuleWithPaging<T>(expectedResponses, requestUrlBase, LevelUpEnvironment.Sandbox);
        }
        
        /// <summary>
        /// Gets a LevelUpClient Module interface that will mimic, for any rest call, the provided expected RestResponses
        /// using a paging format.  This mock will handle adding paging link headers to the returned RestResponse 
        /// objects and serving up the subsequent RestResponse when those links are used.
        /// </summary>
        /// <typeparam name="T">A LevelUpModule type that uses paging.</typeparam>
        /// <param name="expectedResponses">A collection of expected response objects, which will each be treated
        /// as a page, in the correct paging order.</param>
        /// <param name="requestUrlBase">The url that is expected to retrieve the first page.</param>
        /// <param name="environmentToUse">Environment to use to build URI of the request.</param>
        internal static T GetMockedLevelUpModuleWithPaging<T>(RestResponse[] expectedResponses,
                                                              string requestUrlBase,
                                                              LevelUpEnvironment environmentToUse)
            where T : IQueryOrders
        {
            var len = expectedResponses.Length;

            if (len < 1)
            {
                throw new ArgumentException("This mock requires that the caller specify at least one expected response.");
            }

            var GetFullUrlForPage = new Func<int, string>(x => string.Format("{0}?page={1}", requestUrlBase, x));

            // Dictionary is indexed by the incoming url and contains a tuple that houses a response for that particular 
            // incoming url and the url of the next page.
            var responses = new Dictionary<string, Tuple<RestResponse, string>>();

            // If the request does not specify a page number, we will return the first page of results, with any applicable link to page 2
            string urlForPageTwo = len > 1 ? GetFullUrlForPage(2) : null;
            responses.Add(requestUrlBase, new Tuple<RestResponse, string>(expectedResponses[0], urlForPageTwo));

            for (int i = 0; i < len; i++)
            {
                string pageUrl = GetFullUrlForPage(i + 1);
                string nextPageUrl = (i + 2 <= len) ? GetFullUrlForPage(i + 2) : null;
                responses.Add(pageUrl, new Tuple<RestResponse, string>(expectedResponses[i], nextPageUrl));
            }

            var returnsExpectedResponse = new Func<string, string, string, string, IRestResponse>(
                (string url, string body, string accessToken, string userAgent) =>
                    {
                        if (!responses.ContainsKey(url))
                        {
                            if (!url.ToLower().Contains(requestUrlBase.ToLower()))
                            {
                                Assert.Fail("The specified url [" + url +
                                            "] is unknown to the mocked LevelUpClient object.");
                            }

                            // If the specified page does not exist in the mock's record, but the base 
                            // url is valid, we'll return a 204 as per the platform api docs.
                            return new RestResponse {StatusCode = HttpStatusCode.NoContent};
                        }

                        Tuple<RestResponse, string> responseData = responses[url];

                        RestResponse retval = responseData.Item1;
                        if (responseData.Item2 != null)
                        {
                            var header = new Parameter
                                {
                                    Type = ParameterType.HttpHeader,
                                    Name = "Link",
                                    Value = string.Format("<{0}>; rel=\"next\"", responseData.Item2)
                                };
                            retval.Headers.Add(header);
                        }
                        return retval;
                    });

            return GenerateMockLevelUpModuleWhereEveryHttpVerbDoesTheSameThing<T>(returnsExpectedResponse,
                                                                                  environmentToUse);
        }

        private static T GenerateMockLevelUpModuleWhereEveryHttpVerbDoesTheSameThing<T>(
            Func<string, string, string, string, IRestResponse> expectedResponseForAnyCall,
            LevelUpEnvironment targetEnvironment)
            where T : ILevelUpClientModule
        {
            var service = GenerateMockIRestfulService(
                expectedResponseForAnyCall,
                expectedResponseForAnyCall,
                (a, b, c) => expectedResponseForAnyCall(a, null, b, c),  // GET and DELETE return the same specified RestResponse 
                (a, b, c) => expectedResponseForAnyCall(a, null, b, c)); // object, they just don't provide a body.

            return LevelUpClientFactory.Create<T>(new AgentIdentifier("SDK Unit Tests", "", "", ""),
                                                  targetEnvironment,
                                                  service.Object);
        }

        private static Mock<IRestfulService> GenerateMockIRestfulService(
            Func<string, string, string, string, IRestResponse> PostResponse,
            Func<string, string, string, string, IRestResponse> PutResponse,
            Func<string, string, string, IRestResponse> GetResponse,
            Func<string, string, string, IRestResponse> DeleteResponse)
        {
            var service = new Mock<IRestfulService>();
            service.Setup(x => x.Post(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                   .Returns<string, string, string, string>(PostResponse);
            service.Setup(x => x.Put(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                   .Returns(PutResponse);
            service.Setup(x => x.Delete(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                   .Returns(DeleteResponse);
            service.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                   .Returns(GetResponse);
            return service;
        }
    }
}