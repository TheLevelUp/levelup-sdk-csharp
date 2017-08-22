#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="ICreateRefundIntegrationTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
    public class ICreateRefundIntegrationTests
    {
        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.IntegrationTests)]
        public void CreateRefundForV15ProposedOrder()
        {
            // These are essentially the same test.
            IManageProposedOrdersIntegrationTests tmp = new IManageProposedOrdersIntegrationTests();
            tmp.ProposedOrderWithDiscountAndGiftCard_ShouldSucceed();
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.IntegrationTests)]
        [ExpectedException(typeof(Http.LevelUpApiException))]
        public void CreateRefundForNonExistantOrder()
        {
            ICreateRefund refundInterface = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<ICreateRefund>();
            refundInterface.RefundOrder(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken, "aaa");
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.IntegrationTests)]
        [ExpectedException(typeof(LevelUpApiException))]
        public void CreateRefundForUnFinalizedProposedOrder()
        {
            int availableDiscountCents = ClientModuleIntegrationTestingUtilities.GetAvailableDiscountCredit(LevelUpTestConfiguration.Current.ConsumerQrData);

            const int giftCardAmountToUseInCents = 1000;
            ClientModuleIntegrationTestingUtilities.RemoveAnyGiftCardCreditOnConsumerUserAccount();
            ClientModuleIntegrationTestingUtilities.AddGiftCardCreditOnConsumerUserAccount(giftCardAmountToUseInCents);

            const int taxAmountInCents = 100;
            int costOfItemInCents = availableDiscountCents + giftCardAmountToUseInCents + taxAmountInCents + 500;

            IManageProposedOrders orderClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IManageProposedOrders>();
            var proposedOrder = orderClient.CreateProposedOrder(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                                                                LevelUpTestConfiguration.Current.MerchantLocationId,
                                                                LevelUpTestConfiguration.Current.ConsumerQrData,
                                                                costOfItemInCents,
                                                                costOfItemInCents,
                                                                taxAmountInCents,
                                                                0, null, null, null, null, true, null);

           ICreateRefund refundClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<ICreateRefund>();
           refundClient.RefundOrder(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                proposedOrder.ProposedOrderIdentifier);
        }
    }
}
            
            

