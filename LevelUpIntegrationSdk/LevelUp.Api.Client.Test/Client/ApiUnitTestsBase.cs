//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="ApiUnitTestsBase.cs" company="SCVNGR, Inc. d/b/a LevelUp">
//   Copyright(c) 2015 SCVNGR, Inc. d/b/a LevelUp. All rights reserved.
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

using LevelUp.Api.Client.Models.Requests;
using LevelUp.Api.Client.Models.Responses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace LevelUp.Api.Client.Test
{
    /// <summary>
    /// Base class to simplify the creation of api unit tests
    /// </summary>
    public abstract class ApiUnitTestsBase
    {
        private static ILevelUpClient _api;
        private static AccessToken _accessToken;

        protected const int EXPECTED_SPEND_AMOUNT_CENTS = 10;

        public AccessToken AccessToken
        {
            get
            {
                return _accessToken ?? (_accessToken = Api.Authenticate(LevelUpTestConfiguration.Current.App_ApiKey,
                                                                        LevelUpTestConfiguration.Current.Merchant_Username,
                                                                        LevelUpTestConfiguration.Current.Merchant_Password));
            }
        }

        public ILevelUpClient Api
        {
            get
            {
                return _api ?? (_api = LevelUpClientFactory.Create(TestConstants.COMPANY_NAME,
                                                                   TestConstants.PRODUCT_NAME,
                                                                   TestConstants.PRODUCT_VERSION,
                                                                   TestConstants.OS_NAME,
                                                                   TestConstants.BASE_URL_CONFIG_FILE));
            }
        }

        protected GiftCardAddValueResponse AddValueToGiftCard(int valueToAddInCents)
        {
            GiftCardAddValueRequest request = new GiftCardAddValueRequest(LevelUpTestConfiguration.Current.User_GiftCardPaymentToken,
                                                                          valueToAddInCents,
                                                                          LevelUpTestConfiguration.Current.Merchant_LocationId_Visible,
                                                                          "abc123",
                                                                          new List<string>() { "cash", "Credit - Discover" });

            var response = Api.GiftCardAddValue(AccessToken.Token, LevelUpTestConfiguration.Current.Merchant_LocationId_Visible, request);

            Assert.IsNotNull(response);
            Assert.AreEqual(valueToAddInCents, response.AmountAddedInCents);
            Assert.AreEqual(valueToAddInCents,
                            response.NewGiftCardAmountInCents - response.PreviousGiftCardAmountInCents);

            return response;
        }

        protected OrderResponse PlaceOrder(string accessToken,
                                           int spendAmount = 10,
                                           int? appliedDiscountAmount = null,
                                           int? availableGiftCardAmount = null,
                                           int exemptionAmount = 0,
                                           string qrCodeToUse = null,
                                           bool partialAuthEnabled = false)
        {
            Order order = new Order(locationId: LevelUpTestConfiguration.Current.Merchant_LocationId_Visible,
                                    qrPaymentData: qrCodeToUse ?? LevelUpTestConfiguration.Current.User_PaymentToken,
                                    spendAmountCents: spendAmount,
                                    appliedDiscountAmountCents: appliedDiscountAmount,
                                    availableGiftCardAmountCents: availableGiftCardAmount,
                                    exemptionAmountCents: exemptionAmount,
                                    identifierFromMerchant: "Unit test", //Max. 10 characters
                                    partialAuthorizationAllowed: partialAuthEnabled,
                                    items: new List<Item>
                                        {
                                            new Item("Sprockets",
                                                     "Lovely sprockets with gravy",
                                                     "1234",
                                                     "4321",
                                                     "Weird stuff", 1, 1, 7),
                                            new Item("Soylent Green Eggs & Spam",
                                                     "Highly processed edibles",
                                                     "1111",
                                                     "9999",
                                                     "Food...maybe...", 2, 2, 2)
                                        });
            return Api.PlaceOrder(accessToken, order);
        }

        protected int GetLocationIdForMerchant(int? merchantId)
        {
            IList<Location> locations = Api.ListLocations(AccessToken.Token,
                                                          merchantId.GetValueOrDefault(LevelUpTestConfiguration.Current.Merchant_Id));
            Assert.IsNotNull(locations);
            Assert.IsTrue(locations.Count > 0);
            Assert.IsNotNull(locations[0]);
            return locations[0].LocationId;
        }
    }
}
