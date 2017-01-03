#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="IRetrieveMerchantFundedCreditUnitTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
    public class IRetrieveMerchantFundedCreditUnitTests
    {
        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
        public void GetMerchantFundedCreditShouldSucceed()
        {
            const int locationId = 3;
            const string qrCode = "LU02000008ZS9OJFUBNEL6ZM030000LU";
            string expectedRequestUrl = string.Format("https://sandbox.thelevelup.com/v15/locations/{0}/merchant_funded_credit", locationId);

            RestResponse expectedResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = 
                "{" + 
                    "\"merchant_funded_credit\": {" +
                        "\"discount_amount\": 2578," +
                        "\"gift_card_amount\": 0," +
                        "\"total_amount\": 2578" +
                "}}"
            };

            IRetrieveMerchantFundedCredit client = ClientModuleUnitTestingUtilities.GetMockedLevelUpModule<IRetrieveMerchantFundedCredit>(
                expectedResponse, expectedRequestUrl: expectedRequestUrl);
            var resp = client.GetMerchantFundedCredit("not_chekcing_this", locationId, qrCode);

            Assert.AreEqual(resp.DiscountAmount, 2578);
        }

    }
}
