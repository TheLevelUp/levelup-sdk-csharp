#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="IRetrieveMerchantFundedGiftCardCreditFunctionalTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using NUnit.Framework;
using RestSharp;

namespace LevelUp.Api.Client.Tests.Client.FunctionalTests
{
    [TestFixture]
    public class IRetrieveMerchantFundedGiftCardCreditFunctionalTests
    {
        [Test]
        public void RetrieveMerchantFundedGiftCardCreditShouldPass()
        {
            const int locationId = 19;
            const string qr_code = "LU02000008ZS9OJFUBNEL6ZM030000LU";
            const int gift_card_total = 2000;
            const string auth_Token = "example_auth_token";

            string expectedRequestUrl = string.Format(ClientModuleFunctionalTestingUtilities.SANDBOX_URL_PREFIX  + "/v15/locations/{0}/get_merchant_funded_gift_card_credit", locationId);

            string expectedRequestbody = string.Format(
                "{{\"get_merchant_funded_gift_card_credit\": {{\"payment_token_data\": \"{0}\"}} }}", qr_code);

            RestResponse expectedResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = string.Format("{{\"merchant_funded_gift_card_credit\": {{\"total_amount\": {0} }} }}", gift_card_total)
            };

            IRetrieveMerchantFundedGiftCardCredit client = ClientModuleFunctionalTestingUtilities.GetMockedLevelUpModule
                <IRetrieveMerchantFundedGiftCardCredit, GiftCardCreditQueryRequest>(expectedResponse, expectedRequestbody, 
                expectedAccessToken: auth_Token, expectedRequestUrl: expectedRequestUrl);

            var credit = client.GetMerchantFundedGiftCardCredit(auth_Token, locationId, qr_code);
            Assert.AreEqual(credit.TotalAmount, gift_card_total);
        }
    }
}
