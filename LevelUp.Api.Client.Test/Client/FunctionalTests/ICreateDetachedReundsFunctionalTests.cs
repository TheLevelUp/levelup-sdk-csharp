#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="ICreateDetachedReundsFunctionalTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using System.Net;
using LevelUp.Api.Client.ClientInterfaces;
using LevelUp.Api.Client.Models.Requests;
using LevelUp.Api.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;

namespace LevelUp.Api.Client.Test.Client.FunctionalTests
{
    [TestClass]
    public class ICreateDetachedReundsFunctionalTests
    {
        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.FunctionalTests)]
        public void CreateDetachedRefundShouldSucceed()
        {
            const string expectedRequestUrl = "https://sandbox.thelevelup.com/v15/detached_refunds";
            const string accessToken = "abcdef";

            const string cashier = "Andrew Jones";
            const string customer_facing_reason = "Sorry about your coffee!";
            const string identifier_from_merchant = "001001";
            const string internal_reason = "Andrew didn't like his coffee";
            const string manager_confirmation = "12345";
            const string payment_token_data = "LU020000029080KFZ02I9A8V030000LU";
            const string register = "03";
            const string refunded_at = "2014-01-01T00:00:00-04:00";
            const int credit_amount = 743;
            const int location_id = 1855;
            const int loyalty_id = 123;

            string expectedRequestBody = string.Format("{{" +
                                                       "\"detached_refund\": {{" +
                                                            "\"cashier\": \"{0}\"," +
                                                           "\"credit_amount\": {1}," +
                                                           "\"customer_facing_reason\": \"{2}\"," +
                                                           "\"identifier_from_merchant\": \"{3}\"," +
                                                           "\"internal_reason\": \"{4}\"," +
                                                           "\"location_id\": {5}," +
                                                           "\"manager_confirmation\": \"{6}\"," +
                                                           "\"payment_token_data\": \"{7}\"," +
                                                           "\"register\": \"{8}\"" +
                                                           "}}" +
                                                       "}}", 
                                                       cashier, credit_amount, customer_facing_reason, 
                                                       identifier_from_merchant, internal_reason, location_id,
                                                       manager_confirmation, payment_token_data, register);

            RestResponse expectedResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = string.Format("{{" +
                                            "\"detached_refund\": {{" +
                                                "\"cashier\": \"{0}\"," +
                                                "\"credit_amount\": {1}," +
                                                "\"customer_facing_reason\": \"{2}\"," +
                                                "\"identifier_from_merchant\": \"{3}\"," +
                                                "\"internal_reason\": \"{4}\"," +
                                                "\"location_id\": {5}," +
                                                "\"loyalty_id\": \"{6}\"," +
                                                "\"refunded_at\": \"{7}\"," +
                                                "\"register\": \"{8}\"" +
                                            "}}" +
                                        "}}",
                    cashier, credit_amount, customer_facing_reason,
                    identifier_from_merchant, internal_reason, location_id,
                    loyalty_id, refunded_at, register)
            };

            ICreateDetachedRefund client = ClientModuleFunctionalTestingUtilities.GetMockedLevelUpModule<ICreateDetachedRefund, DetachedRefundRequest>(
                expectedResponse, expectedRequestBody, expectedAccessToken: accessToken, expectedRequestUrl: expectedRequestUrl);
            var resp = client.CreateDetachedRefund(accessToken, location_id, payment_token_data, credit_amount, register,
                cashier, identifier_from_merchant, manager_confirmation, customer_facing_reason, internal_reason);

            Assert.AreEqual(resp.Cashier, cashier);
            Assert.AreEqual(resp.CreditAmountCents, credit_amount);
            Assert.AreEqual(resp.CustomerFacingReason, customer_facing_reason);
            Assert.AreEqual(resp.Identifier, identifier_from_merchant);
            Assert.AreEqual(resp.InternalReason, internal_reason);
            Assert.AreEqual(resp.LocationId, location_id);
            Assert.AreEqual(resp.UserId, loyalty_id);
            Assert.AreEqual(resp.RefundedAt, DateTime.Parse(refunded_at));
            Assert.AreEqual(resp.Register, register);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.FunctionalTests)]
        [ExpectedException((typeof(LevelUpApiException)), "No LevelUpAPI exception was thrown an invalid response body.")]
        public void CreateDetachedRefundShouldFailDeserialization()
        {
            // Note: This test is not really specific to Detached Refunds.
            RestResponse expectedResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = "{{" +
                              "\"detached_refund\": {{" +
                                "\"cashier\": \"blah\"," +
                              "}}" +
                          "}}"
            };

            ICreateDetachedRefund client = ClientModuleFunctionalTestingUtilities.GetMockedLevelUpModule<ICreateDetachedRefund>(expectedResponse);
            var resp = client.CreateDetachedRefund("we", 0, "are", 0, "not", "checking", "any", "of", "these", "strings");
        }
    }
}
