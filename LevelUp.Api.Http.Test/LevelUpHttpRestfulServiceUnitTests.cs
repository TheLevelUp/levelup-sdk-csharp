#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="LevelUpHttpRestfulServiceUnitTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

extern alias ThirdParty;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThirdParty.RestSharp;

namespace LevelUp.Api.Http.Test
{
    [TestClass]
    public class LevelUpHttpRestfulServiceUnitTests
    {
        const string MyUrl = "http://test.thelevelup.com/";
        const string UAgent = "I'm a unit test!";
        const string AccessTokenHeader = "important_auth_info";
        readonly string Body = "message body" + System.Environment.NewLine + "for testing";

        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
        public void GetShouldSucceed()
        {
            var service = new LevelUpHttpRestfulService((url, request, userAgent) =>
            {
                CheckUrlAndAgent(url, MyUrl, userAgent, UAgent);
                CheckHeaders(request, Method.GET, AccessTokenHeader);

                return new RestResponse();
            });

            service.Get(MyUrl, AccessTokenHeader, UAgent);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
        public void GetShouldNotHaveAnIssueWithNullParams()
        {
            var service = new LevelUpHttpRestfulService((url, request, userAgent) =>
            {
                CheckUrlAndAgent(url, null, userAgent, null);
                Assert.AreEqual(request.Method, Method.GET);
                Assert.AreEqual(request.Parameters.Count(x => x.Name == "Authorization"), 0);

                return new RestResponse();
            });

            service.Get(null, null, null);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
        public void PostShouldSucceed()
        {
            var service = new LevelUpHttpRestfulService((url, request, userAgent) =>
            {
                CheckUrlAndAgent(url, MyUrl, userAgent, UAgent);
                CheckHeaders(request, Method.POST, AccessTokenHeader);

                Assert.AreEqual(request.Parameters.First(x => x.Name == "application/json").Value, Body);

                return new RestResponse();
            });

            service.Post(MyUrl, Body, AccessTokenHeader, UAgent);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
        public void DeleteShouldSucceed()
        {
            var service = new LevelUpHttpRestfulService((url, request, userAgent) =>
            {
                CheckUrlAndAgent(url, MyUrl, userAgent, UAgent);
                CheckHeaders(request, Method.DELETE, AccessTokenHeader);

                return new RestResponse();
            });

            service.Delete(MyUrl, AccessTokenHeader, UAgent);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
        public void PutShouldSucceed()
        {
            var service = new LevelUpHttpRestfulService((url, request, userAgent) =>
            {
                CheckUrlAndAgent(url, MyUrl, userAgent, UAgent);
                CheckHeaders(request, Method.PUT, AccessTokenHeader);

                return new RestResponse();
            });

            service.Put(MyUrl, Body, AccessTokenHeader, UAgent);
        }

        private static void CheckUrlAndAgent(string generatedUrl, string expectedUrl, string generatedUserAgent, string expectedUserAgent)
        {
            Assert.AreEqual(expectedUrl, generatedUrl);
            Assert.AreEqual(expectedUserAgent, generatedUserAgent);
        }

        private static void CheckHeaders(IRestRequest generatedRequest, Method expectedMethodType, string expectedAccessTokenHeader)
        {
            Assert.AreEqual(generatedRequest.Method, expectedMethodType);
            Assert.AreEqual(generatedRequest.Parameters.First(x => x.Name == "Authorization").Value, expectedAccessTokenHeader);
            Assert.AreEqual(generatedRequest.Parameters.First(x => x.Name == "Accept").Value, "application/json");
            Assert.AreEqual(generatedRequest.Parameters.First(x => x.Name == "Content-Type").Value, "application/json");
        }
    }
}