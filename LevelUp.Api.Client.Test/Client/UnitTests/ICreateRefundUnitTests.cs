#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="ICreateRefundUnitTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using System.Net;
using LevelUp.Api.Client.ClientInterfaces;
using LevelUp.Api.Client.Models.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThirdParty.RestSharp;

namespace LevelUp.Api.Client.Test.Client
{
    [TestClass]
    public class ICreateRefundUnitTests
    {
        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
        public void RefundOrderShouldSucceedSansManagerConfirmation()
        {
            const string uuid = "bf143c9084810132faf95a123bd6cde9";
            const string created_at = "2015-01-22T11:29:22-05:00";
            const int location_id = 19;
            const int loyalty_id = 20;
            const string refunded_at = "2015-01-22T14:28:05-05:00";
            const string user_display_name = "TestCredsT.";
            const int earn_amount = 0;
            const int merchant_funded_credit_amount = 0;
            const int spend_amount = 10;
            const int tip_amount = 0;
            const int total_amount = 10;


            string expectedRequestUrl = string.Format("https://sandbox.thelevelup.com/v15/orders/{0}/refund", uuid);

            RestResponse expectedResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = string.Format("{{" +
                                          "\"order\":{{" +
                                              "\"created_at\":\"{0}\"," +
                                              "\"location_id\":{1}," +
                                              "\"loyalty_id\":{2}," +
                                              "\"refunded_at\":\"{3}\"," +
                                              "\"user_display_name\":\"{4}\"," +
                                              "\"uuid\":\"{5}\"," +
                                              "\"earn_amount\":{6}," +
                                              "\"merchant_funded_credit_amount\":{7}," +
                                              "\"spend_amount\":{8}," +
                                              "\"tip_amount\":{9}," +
                                              "\"total_amount\":{10}" +
                                          "}}" +
                                      "}}", created_at, location_id, loyalty_id, refunded_at, user_display_name, uuid, earn_amount, 
                                      merchant_funded_credit_amount, spend_amount, tip_amount, total_amount)
            };

            string expectedRequest = string.Empty;

            ICreateRefund client = ClientModuleUnitTestingUtilities.GetMockedLevelUpModule<ICreateRefund, RefundRequest>(
                expectedResponse, expectedRequest, expectedRequestUrl: expectedRequestUrl);
            var order = client.RefundOrder("not_checking_this", uuid);

            Assert.AreEqual(order.EarnAmount, earn_amount);
            Assert.AreEqual(order.LocationId, location_id);
            Assert.AreEqual(order.LoyaltyId, loyalty_id);
            Assert.AreEqual(order.MerchantFundedCreditAmount, merchant_funded_credit_amount);
            Assert.AreEqual(order.OrderIdentifier, uuid);
            Assert.AreEqual(order.SpendAmount, spend_amount);
            Assert.AreEqual(order.TipAmount, tip_amount);
            Assert.AreEqual(order.UserName, user_display_name);
            Assert.AreEqual(order.TimeOfRefund, refunded_at);
            Assert.AreEqual(order.CreatedAt, created_at);
        }
    }
}
