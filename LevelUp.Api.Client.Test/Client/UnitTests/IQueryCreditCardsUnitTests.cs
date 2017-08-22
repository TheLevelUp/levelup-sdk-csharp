#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="IQueryCreditCardsUnitTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;

namespace LevelUp.Api.Client.Test.Client
{
    [TestClass]
    public class IQueryCreditCardsUnitTests
    {
        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.UnitTests)]
        public void ListCreditCardsShouldSucceed()
        {
            string expectedRequestUrl = "https://sandbox.thelevelup.com/v15/credit_cards";

            RestResponse expectedResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content =
                    "[" +
                        "{" +
                            "\"credit_card\": {" +
                                "\"bin\": \"123456\"," +
                                "\"description\": \"JCB ending in 1234\"," +
                                "\"expiration_month\": 7," +
                                "\"expiration_year\": 2015," +
                                "\"id\": 305999," +
                                "\"last_4\": \"1234\"," +
                                "\"promoted\": true," +
                                "\"state\": \"active\"," +
                                "\"type\": \"JCB\"" +
                            "}" +
                        "}," +
                        "{" +
                            "\"credit_card\": {" +
                                "\"bin\": \"654321\"," +
                                "\"description\": \"JCB ending in 4321\"," +
                                "\"expiration_month\": 3," +
                                "\"expiration_year\": 2015," +
                                "\"id\": 999503," +
                                "\"last_4\": \"4321\"," +
                                "\"promoted\": false," +
                                "\"state\": \"active\"," +
                                "\"type\": \"JCB\"" +
                            "}" +
                        "}" +
                    "]"
            };

            IQueryCreditCards client = ClientModuleUnitTestingUtilities.GetMockedLevelUpModule<IQueryCreditCards>(
                expectedResponse, expectedAccessToken: "access_token", expectedRequestUrl: expectedRequestUrl);
            var cards = client.ListCreditCards("access_token");

            Assert.AreEqual(cards.Count, 2);
            Assert.AreEqual(cards[0].Bin, "123456");
            Assert.AreEqual(cards[1].Bin, "654321");
        }
    }
}
