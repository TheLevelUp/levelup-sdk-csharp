#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="ILookupUserLoyaltyUnitTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThirdParty.RestSharp;

namespace LevelUp.Api.Client.Test.Client
{
    [TestClass]
    public class ILookupUserLoyaltyUnitTests
    {
        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
        public void GetLoyaltyShouldSucceed()
        {
            const int merchant_Id = 456;
            const int merchant_earn_amount = 500;
            const int merchant_spend_amount = 5000;
            const int orders_count = 77;
            const int potential_credit_amount = 7350;
            const int savings_amount = 835;
            const int spend_remaining_amount = 427;
            const int total_volume_amount = 6317;
            const int user_id = 789;
            const bool merchant_loyalty_enabled = true;
            const decimal progress_percentage = 42.0m;

            string expectedRequestUrl = string.Format("https://sandbox.thelevelup.com/v14/merchants/{0}/loyalty", merchant_Id);

            RestResponse expectedResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = string.Format(  "{{" +
                                              "\"loyalty\":{{" +
                                                  "\"merchant_earn_amount\":{0}," +
                                                  "\"merchant_id\":{1}," +
                                                  "\"merchant_loyalty_enabled\":{2}," +
                                                  "\"merchant_spend_amount\":{3}," +
                                                  "\"orders_count\":{4}," +
                                                  "\"potential_credit_amount\":{5}," +
                                                  "\"progress_percentage\":{6}," +
                                                  "\"savings_amount\":{7}," +
                                                  "\"spend_remaining_amount\":{8}," +
                                                  "\"total_volume_amount\":{9}," +
                                                  "\"user_id\":{10}" +
                                              "}}" +
                                          "}}", merchant_earn_amount, merchant_Id, merchant_loyalty_enabled ? "true" : "false", merchant_spend_amount, orders_count, 
                                          potential_credit_amount, progress_percentage, savings_amount, spend_remaining_amount, total_volume_amount, user_id)
            };

            ILookupUserLoyalty client = ClientModuleUnitTestingUtilities.GetMockedLevelUpModule<ILookupUserLoyalty>(
                expectedResponse, expectedRequestUrl: expectedRequestUrl);

            var loyalty = client.GetLoyalty("not_checking_this", merchant_Id);

            Assert.AreEqual(loyalty.MerchantEarnAmount, merchant_earn_amount);
            Assert.AreEqual(loyalty.MerchantId, merchant_Id);
            Assert.AreEqual(loyalty.MerchantLoyaltyEnabled, merchant_loyalty_enabled);
            Assert.AreEqual(loyalty.MerchantSpendAmount, merchant_spend_amount);
            Assert.AreEqual(loyalty.OrdersCount, orders_count);
            Assert.AreEqual(loyalty.PotentialCreditAmount, potential_credit_amount);
            Assert.AreEqual(loyalty.ProgressPercentage, progress_percentage);
            Assert.AreEqual(loyalty.SavingsAmount, savings_amount);
            Assert.AreEqual(loyalty.SpendRemainingAmount, spend_remaining_amount);
            Assert.AreEqual(loyalty.TotalVolumeAmount, total_volume_amount);
            Assert.AreEqual(loyalty.UserId, user_id);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
        [ExpectedException(typeof(Http.LevelUpApiException), "GetLoyalty failed to throw an exception for an invalid returned http status code.")]
        public void GetLoyaltyShouldFailForBadStatusCode()
        {
            const int merchant_Id = 456;
            string expectedRequestUrl = string.Format("https://sandbox.thelevelup.com/v14/merchants/{0}/loyalty", merchant_Id);

            RestResponse expectedResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.NotFound,
            };

            ILookupUserLoyalty client = ClientModuleUnitTestingUtilities.GetMockedLevelUpModule<ILookupUserLoyalty>(
                expectedResponse, expectedRequestUrl: expectedRequestUrl);

            client.GetLoyalty("not_checking_this", merchant_Id);
        }
    }
}
