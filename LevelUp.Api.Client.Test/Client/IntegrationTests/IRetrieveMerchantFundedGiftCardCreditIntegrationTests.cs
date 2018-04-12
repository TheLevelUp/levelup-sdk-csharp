﻿#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="IRetrieveMerchantFundedGiftCardCreditIntegrationTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

namespace LevelUp.Api.Client.Test.Client.IntegrationTests
{
    [TestClass]
    public class IRetrieveMerchantFundedGiftCardCreditIntegrationTests
    {
        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.IntegrationTests)]
        public void GetMerchantFundedGiftCardCredit()
        {
            ClientModuleIntegrationTestingUtilities.RemoveAnyGiftCardCreditOnConsumerUserAccount();
            IRetrieveMerchantFundedGiftCardCredit creditInterface = 
                ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IRetrieveMerchantFundedGiftCardCredit>();

            var credit = creditInterface.GetMerchantFundedGiftCardCredit(
                ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                LevelUpTestConfiguration.Current.MerchantLocationId, LevelUpTestConfiguration.Current.ConsumerQrData);

            Assert.AreEqual(0, credit.TotalAmount);

            const int creditAmountCents = 1000;
            ClientModuleIntegrationTestingUtilities.AddGiftCardCreditOnConsumerUserAccount(creditAmountCents);

            credit = creditInterface.GetMerchantFundedGiftCardCredit(
                ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                LevelUpTestConfiguration.Current.MerchantLocationId, LevelUpTestConfiguration.Current.ConsumerQrData);

            Assert.AreEqual(creditAmountCents, credit.TotalAmount);

            ClientModuleIntegrationTestingUtilities.RemoveAnyGiftCardCreditOnConsumerUserAccount();
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.IntegrationTests)]
        [ExpectedException(typeof(LevelUpApiException))]
        public void GetMerchantFundedGiftCardCreditWithInvalidQRCode()
        {
            IRetrieveMerchantFundedGiftCardCredit creditInterface = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IRetrieveMerchantFundedGiftCardCredit>();
            creditInterface.GetMerchantFundedGiftCardCredit(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                LevelUpTestConfiguration.Current.MerchantLocationId,
                "LU_invalid_qr_code");
        }
    }
}
