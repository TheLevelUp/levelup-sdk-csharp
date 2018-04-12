#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="IAuthenticateFunctionalTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using System.Net;
using LevelUp.Api.Client.ClientInterfaces;
using LevelUp.Api.Client.Models.Requests;
using LevelUp.Api.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;

namespace LevelUp.Api.Client.Test.Client.FunctionalTests
{
    [TestClass]
    public class IAuthenticateFunctionalTests
    {
        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.FunctionalTests)]
        public void AuthenticationShouldSucceed()
        {
            const string testBaseUri = "https://127.0.0.1";
            const string expectedRequestUrl = testBaseUri + "/v15/access_tokens";

            const string testClientId = "1a2b3c4d5e6f7g8h9i0j";
            const string testMerchantUsername = "IAmTheUnitTester@thelevelup.com";
            const string testMerchantPassword = "ThisIsMyP@ssw0rd!!1!";

            string expectedRequestbody = string.Format("{{\"access_token\": {{" +
                                                       "\"api_key\": \"{0}\"" +
                                                       ",\"username\": \"{1}\"" +
                                                       ",\"password\": \"{2}\"" +
                                                       "}}}}",
                                                       testClientId,
                                                       testMerchantUsername,
                                                       testMerchantPassword);

            // This response format does not matter for this test.
            RestResponse expectedResponse = new RestResponse
                {
                    StatusCode = HttpStatusCode.OK,
                };

            IAuthenticate auth =
                ClientModuleFunctionalTestingUtilities.GetMockedLevelUpModule<IAuthenticate, AccessTokenRequest>(
                    expectedResponse,
                    expectedRequestbody,
                    expectedAccessToken: null,
                    expectedRequestUrl: expectedRequestUrl,
                    environmentToUse: new LevelUpEnvironment(testBaseUri));

            var token = auth.Authenticate(testClientId, testMerchantUsername, testMerchantPassword);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.FunctionalTests)]
        public void AuthenticationShouldSucceedForCode200OK()
        {
            const int expectedUserId = 54321;
            const int expectedmerchantId = 32225;

            RestResponse expectedResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = "{\"access_token\":" +
                          "{\"merchant_id\":" + expectedmerchantId +
                          ",\"token\":\"5564290-ee38925c6de8f4fd945e1606b232296f52f805563fb882fed1c6bq465dc367\"" + // this is not a real token
                          ",\"user_id\":" + expectedUserId + "}}"
            };

            IAuthenticate iAuth =
                ClientModuleFunctionalTestingUtilities.GetMockedLevelUpModule<IAuthenticate>(expectedResponse);

            var token = iAuth.Authenticate("This doesn't matter", "This has no bearing", "This can be whatever you want");

            Assert.IsNotNull(token, "The deserialization of this successful authentication response should have succeeded");
            Assert.AreEqual(token.MerchantId, expectedmerchantId);
            Assert.AreEqual(token.UserId, expectedUserId);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.FunctionalTests)]
        [ExpectedException((typeof(LevelUpApiException)), "No LevelUpAPI exception was thrown for a response with a return code of 404.")]
        public void AuthenticationShouldFailForErrorCode404NotFound()
        {
            const string testClientId = "1212whatsinthestew34noonesreallysure";
            const string testMerchantUsername = "WhoWasThatMaskedUnitTester@thelevelup.com";
            const string testMerchantPassword = "MyP@ssw0rdIsS3cr3t";

            RestResponse expected = new RestResponse{StatusCode = HttpStatusCode.NotFound};
            IAuthenticate auth = ClientModuleFunctionalTestingUtilities.GetMockedLevelUpModule<IAuthenticate>(expected);

            // Should throw exception for non-200 [OK] response
            auth.Authenticate(testClientId, testMerchantUsername, testMerchantPassword);
        }
    }
}
