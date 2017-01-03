#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="IManageProposedOrdersIntegrationTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LevelUp.Api.Client.Test.Client
{
    [TestClass]
    public class IManageProposedOrdersIntegrationTests
    {
        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.IntegrationTests)]
        public void TwoStagePlaceOrderWorkflowShouldSucceed()
        {
            IRetrieveMerchantFundedCredit creditClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IRetrieveMerchantFundedCredit>();

            var availableCredit = creditClient.GetMerchantFundedCredit( ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                                                                        LevelUpTestConfiguration.Current.MerchantLocationId, 
                                                                        LevelUpTestConfiguration.Current.ConsumerQrData);
            int availableDiscountCents = availableCredit.DiscountAmount;

            const int giftCardAmountToUseInCents = 1000;
            ClientModuleIntegrationTestingUtilities.RemoveAnyGiftCardCreditOnConsumerUserAccount();
            ClientModuleIntegrationTestingUtilities.AddGiftCardCreditOnConsumerUserAccount(giftCardAmountToUseInCents);

            const int taxAmountInCents = 100;
            // Make sure that gift card, credit card, and discount(if available) will all be requried as part of this test payment.
            int costOfItemInCents = availableDiscountCents + giftCardAmountToUseInCents + taxAmountInCents + 500;
            
            IManageProposedOrders orderClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IManageProposedOrders>();
            
            var proposedOrder = orderClient.CreateProposedOrder(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                                                                LevelUpTestConfiguration.Current.MerchantLocationId,
                                                                LevelUpTestConfiguration.Current.ConsumerQrData,
                                                                costOfItemInCents,
                                                                taxAmountInCents,
                                                                0, null, null, null, null, true, null);

            Assert.AreEqual(proposedOrder.DiscountAmountCents, availableDiscountCents);
            int taxAmountAfterDiscountApplied = (taxAmountInCents * (costOfItemInCents - proposedOrder.DiscountAmountCents)) / costOfItemInCents;

            var completedOrder = orderClient.CompleteProposedOrder( ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                                                                    LevelUpTestConfiguration.Current.MerchantLocationId,
                                                                    LevelUpTestConfiguration.Current.ConsumerQrData,
                                                                    proposedOrder.ProposedOrderIdentifier,
                                                                    costOfItemInCents,
                                                                    taxAmountAfterDiscountApplied,
                                                                    0,
                                                                    proposedOrder.DiscountAmountCents,
                                                                    null, null, null, null, true, null);

            Assert.AreEqual(completedOrder.GiftCardTipAmount, 0);
            Assert.AreEqual(completedOrder.GiftCardTotalAmount, giftCardAmountToUseInCents);
            Assert.AreEqual(completedOrder.GiftCardTipAmount, 0);
            Assert.AreEqual(completedOrder.TipAmount, 0);
            Assert.AreEqual(completedOrder.SpendAmount, costOfItemInCents);

            ICreateRefund refundClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<ICreateRefund>();
            refundClient.RefundOrder(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                completedOrder.OrderIdentifier);
        }
    }
}
