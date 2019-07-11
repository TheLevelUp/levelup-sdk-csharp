#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="IRetrievePaymentTokenFunctionalTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using NUnit.Framework;
using RestSharp;

namespace LevelUp.Api.Client.Tests.Client.FunctionalTests
{
    [TestFixture]
    public class IRetrievePaymentTokenFunctionalTests
    {
        [Test]
        public void GetPaymentTokenShouldSucceed()
        {
            const string expectedRequestUrl = ClientModuleFunctionalTestingUtilities.SANDBOX_URL_PREFIX  + "/v15/payment_token";

            RestResponse expectedResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = 
                "{" +
                    "\"payment_token\": {" +
                        "\"data\": \"LU020000000THISISFAKE000\"," +
                        "\"id\": 323" +
                    "}" +
                "}"
            };

            IRetrievePaymentToken client = ClientModuleFunctionalTestingUtilities.GetMockedLevelUpModule<IRetrievePaymentToken>(
                expectedResponse, expectedRequestUrl: expectedRequestUrl);
            var paymentToken = client.GetPaymentToken("not_checking_this");
            Assert.AreEqual(paymentToken.Id, 323);
            Assert.AreEqual(paymentToken.Data, "LU020000000THISISFAKE000");
        }
    }
}
