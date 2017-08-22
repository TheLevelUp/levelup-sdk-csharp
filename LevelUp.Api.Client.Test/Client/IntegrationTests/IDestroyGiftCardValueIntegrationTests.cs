#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="IDestroyGiftCardValueIntegrationTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using LevelUp.Api.Client.ClientInterfaces;
using LevelUp.Api.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LevelUp.Api.Client.Test.Client
{
    [TestClass]
    public class IDestroyGiftCardValueIntegrationTests
    {
        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.IntegrationTests)]
        public void DestroyGiftCardValue()
        {
            const int giftCardAmountToAdd = 2000;
            int giftCardAmountToDelete = giftCardAmountToAdd/2;
            AddThenDestroyGiftCardValue(giftCardAmountToAdd, 
                                        giftCardAmountToDelete, 
                                        LevelUpTestConfiguration.Current.ConsumerQrData,
                                        ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.IntegrationTests)]
        public void DestroyGiftCardValue_ShouldFailForGreaterThanGiftCardCredit()
        {
            DestroyGiftCardValue_TestForApiException(1000, 2000, LevelUpTestConfiguration.Current.ConsumerQrData, 
                ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken, (System.Net.HttpStatusCode) 422, 
                "destroy value greater than existing gift card credit");
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.IntegrationTests)]
        public void DestroyGiftCardValue_ShouldFailForZeroValue()
        {
            DestroyGiftCardValue_TestForApiException(1000, 0, LevelUpTestConfiguration.Current.ConsumerQrData,
                ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken, (System.Net.HttpStatusCode)422,
                "destroy value is zero");
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.IntegrationTests)]
        public void DestroyGiftCardValue_ShouldFailForNegativeValue()
        {
            DestroyGiftCardValue_TestForApiException(1000, -1000, LevelUpTestConfiguration.Current.ConsumerQrData,
                ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken, (System.Net.HttpStatusCode)422,
                "destroy value is negative");
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.IntegrationTests)]
        public void DestroyGiftCardValue_ShouldFailForInvalidQrCode()
        {
            DestroyGiftCardValue_TestForApiException(1000, 1000, "this_value_is_invalid",
                ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken, (System.Net.HttpStatusCode)422,
                "invalid QR code");
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.IntegrationTests)]
        public void DestroyGiftCardValue_ShouldFailForInvalidMerchantToken()
        {
            DestroyGiftCardValue_TestForApiException(1000, 1000, LevelUpTestConfiguration.Current.ConsumerQrData,
                "this_value_is_invalid", (System.Net.HttpStatusCode)401,
                "invalid merchant token");
        }


        private void DestroyGiftCardValue_TestForApiException(int giftCardAmountToAdd, int giftCardAmountToDelete, string qrCode, 
            string merchantToken, System.Net.HttpStatusCode expectedCode, string testDescription)
        {
            try
            {
                AddThenDestroyGiftCardValue(giftCardAmountToAdd, giftCardAmountToDelete, qrCode, merchantToken);
                Assert.Fail(string.Format("DestroyGiftCardTest [{0}] failed to throw a LevelUpApiException with HTTP status code {1}.", 
                    testDescription, (int)expectedCode));
            }
            catch (LevelUpApiException ex)
            {
                Assert.AreEqual(ex.StatusCode, expectedCode);
            }
        }

        private void AddThenDestroyGiftCardValue(int giftCardAmountToAdd, int giftCardAmountToDelete, string QrCode, string merchantToken)
        {
            IDestroyGiftCardValue destroyInterface = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IDestroyGiftCardValue>();
            
            ClientModuleIntegrationTestingUtilities.RemoveAnyGiftCardCreditOnConsumerUserAccount();
            ClientModuleIntegrationTestingUtilities.AddGiftCardCreditOnConsumerUserAccount(giftCardAmountToAdd);

            var destroyed = destroyInterface.GiftCardDestroyValue(  merchantToken, 
                                                                    LevelUpTestConfiguration.Current.MerchantId, 
                                                                    QrCode, 
                                                                    giftCardAmountToDelete);

            Assert.AreEqual(destroyed.AmountRemovedInCents, giftCardAmountToDelete);
            Assert.AreEqual(destroyed.PreviousGiftCardAmountInCents, giftCardAmountToAdd);
            Assert.AreEqual(destroyed.NewGiftCardAmountInCents, giftCardAmountToAdd - giftCardAmountToDelete);

            IRetrieveMerchantFundedGiftCardCredit queryInterface  = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IRetrieveMerchantFundedGiftCardCredit>();
            var loyalty = queryInterface.GetMerchantFundedGiftCardCredit(
                ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                LevelUpTestConfiguration.Current.MerchantLocationId, 
                LevelUpTestConfiguration.Current.ConsumerQrData);

            Assert.AreEqual(loyalty.TotalAmount, giftCardAmountToAdd - giftCardAmountToDelete);

            ClientModuleIntegrationTestingUtilities.RemoveAnyGiftCardCreditOnConsumerUserAccount();
        }
    }
}
