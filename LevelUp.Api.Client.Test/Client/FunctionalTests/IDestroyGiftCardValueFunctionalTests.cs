#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="IDestroyGiftCardValueFunctionalTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;

namespace LevelUp.Api.Client.Test.Client.FunctionalTests
{
    [TestClass]
    public class IDestroyGiftCardValueFunctionalTests
    {
        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.FunctionalTests)]
        public void GiftCardDestroyValueShouldSucceed()
        {
            string paymentTokenData = "LU020000029080KFZ02I9A8V030000LU";
            int merchantId = 3;
            int valueAmountCents = 100;
            int initialTotalValueAtMerchantCents = 202;
            
            string expectedRequestUrl = string.Format("https://sandbox.thelevelup.com/v15/merchants/{0}/gift_card_value_removals", merchantId);

            string expectedRequestBody = string.Format("{{" +
                                                           "\"gift_card_value_removal\":{{" +
                                                               "\"payment_token_data\":\"{0}\"," +
                                                               "\"value_amount\":{1}" +
                                                           "}}" +
                                                       "}}",
                                                        paymentTokenData, valueAmountCents);


            RestResponse expectedResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = string.Format("{{" +
                                            "\"gift_card_value_removal\":{{" +
                                                "\"removed_value_amount\":{0}," +
                                                "\"new_value_at_merchant_amount\":{1}," +
                                                "\"old_value_at_merchant_amount\":{2}," +
                                            "}}" +
                                        "}}",
                                        valueAmountCents, (initialTotalValueAtMerchantCents - valueAmountCents), initialTotalValueAtMerchantCents)
            };

            IDestroyGiftCardValue client = ClientModuleFunctionalTestingUtilities.GetMockedLevelUpModule<IDestroyGiftCardValue, GiftCardRemoveValueRequest>(
                expectedResponse, expectedRequestBody, expectedRequestUrl: expectedRequestUrl);

            var destroyed = client.GiftCardDestroyValue("not_checking_this", merchantId, paymentTokenData, valueAmountCents);
            Assert.AreEqual(destroyed.AmountRemovedInCents, valueAmountCents);
            Assert.AreEqual(destroyed.PreviousGiftCardAmountInCents, initialTotalValueAtMerchantCents);
            Assert.AreEqual(destroyed.NewGiftCardAmountInCents, initialTotalValueAtMerchantCents - valueAmountCents);
        }


        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.FunctionalTests)]
        public void GiftCardDestroyValueShouldFailForBadHttpStatusCodes()
        {
            List<RestResponse> possibleErrors = new List<RestResponse>(new RestResponse[]
            {
                new RestResponse // Invalid QR code
                {
                    StatusCode = HttpStatusCode.NotFound
                },

                new RestResponse // Destroy value was negative
                {
                    StatusCode = (HttpStatusCode) 422,
                    Content = "[{\"error\":{\"object\":\"gift_cards/value_adder\",\"property\":\"base\",\"message\":\"You must remove a positive value.\"}}]"
                },

                new RestResponse // Destroy value was more than giftcard value
                {
                    StatusCode = (HttpStatusCode) 422,
                    Content = "[{\"error\":{\"object\":\"gift_cards/value_adder\",\"property\":\"base\",\"message\":\"This giftcard has a balance of $X.XX. Please retry with that amount.\"}}]"
                },

                new RestResponse // Bad merchant token
                {
                    StatusCode = HttpStatusCode.Unauthorized
                }
            });

            foreach (var response in possibleErrors)
            {
                IDestroyGiftCardValue client = ClientModuleFunctionalTestingUtilities.GetMockedLevelUpModule<IDestroyGiftCardValue>(response);

                try
                {
                    var destroyed = client.GiftCardDestroyValue("not_checking_this", 1, "not_checking_this", 1000);
                    Assert.Fail("GiftCardDestroyValue failed to throw for an invalid HTTP return code of " + response.StatusCode);
                }
                catch (LevelUp.Api.Http.LevelUpApiException) { }
            }
        }
    }
}
