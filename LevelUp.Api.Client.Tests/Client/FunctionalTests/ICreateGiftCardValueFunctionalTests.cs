#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="ICreateGiftCardValueFunctionalTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using System.Net;
using LevelUp.Api.Client.ClientInterfaces;
using LevelUp.Api.Client.Models.Requests;
using NUnit.Framework;
using RestSharp;

namespace LevelUp.Api.Client.Tests.Client.FunctionalTests
{
    [TestFixture]
    public class ICreateGiftCardValueFunctionalTests
    {
        [Test]
        public void GiftCardAddValueShouldSucceed()
        {
            const string accessToken = "abc";
            const int merchant_id = 3554;
            const string payment_token_data = "LU020000000THISISFAKE000000000LU";
            const int value_amount = 1000;
            const int location_id = 1234;
            const string order_uuid = "a7e23820d56802321bb64ab3b58dfe6c";
            const string identifier_from_merchant = "012345";
            const string tender_types = "cash";

            string expectedRequestUrl = string.Format(ClientModuleFunctionalTestingUtilities.SANDBOX_URL_PREFIX  + "/v15/merchants/{0}/gift_card_value_additions", merchant_id);

            string expectedRequestbody = string.Format(
                "{{" +
                    "\"gift_card_value_addition\": {{" +
                        "\"payment_token_data\": \"{0}\"," +
                        "\"value_amount\": {1}," +
                        "\"location_id\": {2}," +
                        "\"order_uuid\": \"{3}\"," +
                        "\"tender_types\": [\"{4}\"], " +
                        "\"identifier_from_merchant\": \"{5}\" " +
                    "}}" +
                "}}", payment_token_data, value_amount, location_id, order_uuid, tender_types, identifier_from_merchant);

            RestResponse expectedResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = "{" +
                              "\"gift_card_value_addition\": {" +
                                  "\"added_value_amount\": 1000," +
                                  "\"new_value_at_merchant_amount\": 1000," +
                                  "\"old_value_at_merchant_amount\": 0," +
                              "}" +
                          "}"
            };

            ICreateGiftCardValue client = ClientModuleFunctionalTestingUtilities.GetMockedLevelUpModule<ICreateGiftCardValue, GiftCardAddValueRequest>(
                expectedResponse, expectedRequestbody, expectedAccessToken: accessToken, expectedRequestUrl: expectedRequestUrl);
            
            var valueAddition = client.GiftCardAddValue(accessToken, 
                                                        merchant_id, 
                                                        location_id, 
                                                        payment_token_data,
                                                        value_amount, 
                                                        identifier_from_merchant, 
                                                        new List<string>(new[] {tender_types}), 
                                                        order_uuid);

            Assert.AreEqual(valueAddition.AmountAddedInCents, 1000);
            Assert.AreEqual(valueAddition.NewGiftCardAmountInCents, 1000);
            Assert.AreEqual(valueAddition.PreviousGiftCardAmountInCents, 0);
        }
    }
}
