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

namespace LevelUp.Api.Client.Test.Client.IntegrationTests
{
    [TestClass]
    public class IManageProposedOrdersIntegrationTests
    {
        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.IntegrationTests)]
        public void ProposedOrderWithDiscountAndGiftCard_ShouldSucceed()
        {
            int availableDiscountCents = ClientModuleIntegrationTestingUtilities.GetAvailableDiscountCredit(LevelUpTestConfiguration.Current.ConsumerQrData);

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
                                                                costOfItemInCents,
                                                                taxAmountInCents,
                                                                0, null, null, null, null, true, null);

            Assert.AreEqual(proposedOrder.DiscountAmountCents, availableDiscountCents);
            int taxAmountAfterDiscountApplied = (taxAmountInCents * (costOfItemInCents - proposedOrder.DiscountAmountCents)) / costOfItemInCents;

            var completedOrder = orderClient.CompleteProposedOrder(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                                                                    LevelUpTestConfiguration.Current.MerchantLocationId,
                                                                    LevelUpTestConfiguration.Current.ConsumerQrData,
                                                                    proposedOrder.ProposedOrderIdentifier,
                                                                    costOfItemInCents,
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


        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.IntegrationTests)]
        [ExpectedException(typeof(LevelUp.Api.Http.LevelUpApiException))]
        public void ProposedOrderWithBadQrCode_ShouldThrow()
        {
            IManageProposedOrders orderClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IManageProposedOrders>();
            
            orderClient.CreateProposedOrder(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken, 
                LevelUpTestConfiguration.Current.MerchantLocationId, "bad_qr_code", 10, 10, 0, 0);
        }

        [TestCategory(LevelUp.Api.Http.Test.TestCategory.IntegrationTests)]
        public void ProposedOrderWithExemption_ShouldSucceed()
        {
            int availableDiscountCents = ClientModuleIntegrationTestingUtilities.GetAvailableDiscountCredit(LevelUpTestConfiguration.Current.ConsumerQrData);

            const int giftCardAmountInCents = 1000;
            ClientModuleIntegrationTestingUtilities.RemoveAnyGiftCardCreditOnConsumerUserAccount();
            ClientModuleIntegrationTestingUtilities.AddGiftCardCreditOnConsumerUserAccount(giftCardAmountInCents);

            const int taxAmountInCents = 100;

            // Make sure that gift card, credit card, and discount(if available) will all be requried as part of this test payment.
            int costOfItemInCents = availableDiscountCents + giftCardAmountInCents + taxAmountInCents + 500;

            IManageProposedOrders orderClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IManageProposedOrders>();
            var proposedOrder = orderClient.CreateProposedOrder(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                LevelUpTestConfiguration.Current.MerchantLocationId,
                LevelUpTestConfiguration.Current.ConsumerQrData,
                costOfItemInCents,
                costOfItemInCents,
                taxAmountInCents,
                costOfItemInCents, null, null, null, null, true, null);

            Assert.AreEqual(proposedOrder.DiscountAmountCents, 0);
            
            var completedOrder = orderClient.CompleteProposedOrder(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                LevelUpTestConfiguration.Current.MerchantLocationId,
                LevelUpTestConfiguration.Current.ConsumerQrData,
                proposedOrder.ProposedOrderIdentifier,
                costOfItemInCents,
                costOfItemInCents,
                taxAmountInCents,
                costOfItemInCents,
                proposedOrder.DiscountAmountCents,
                null, null, null, null, true, null);

            Assert.AreEqual(completedOrder.GiftCardTipAmount, 0);
            Assert.AreEqual(completedOrder.GiftCardTotalAmount, 0);

            ICreateRefund refundClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<ICreateRefund>();
            refundClient.RefundOrder(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                completedOrder.OrderIdentifier);
        }

        [TestCategory(LevelUp.Api.Http.Test.TestCategory.IntegrationTests)]
        public void ProposedOrderWithPartialAuth_ShouldSucceed()
        {
            int giftCardAmount = 1000;
            ClientModuleIntegrationTestingUtilities.RemoveAnyGiftCardCreditOnConsumerWithNoLinkedPaymentUserAccount();
            ClientModuleIntegrationTestingUtilities.AddGiftCardCreditOnConsumerWithNoLinkedPaymentUserAccount(giftCardAmount);

            var credit = ClientModuleIntegrationTestingUtilities.GetAvailableDiscountCredit(LevelUpTestConfiguration.Current.ConsumerWithNoLinkedPaymentQrData);

            int spendAmount = (1000 + giftCardAmount + credit);


            IManageProposedOrders orderClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IManageProposedOrders>();
            var proposedOrder = orderClient.CreateProposedOrder(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                LevelUpTestConfiguration.Current.MerchantLocationId,
                LevelUpTestConfiguration.Current.ConsumerWithNoLinkedPaymentQrData,
                spendAmount, spendAmount, 0, 0, null, null, null, null, true, null);

            Assert.AreEqual(proposedOrder.DiscountAmountCents, credit);

            var completedOrder = orderClient.CompleteProposedOrder(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                LevelUpTestConfiguration.Current.MerchantLocationId,
                LevelUpTestConfiguration.Current.ConsumerWithNoLinkedPaymentQrData,
                proposedOrder.ProposedOrderIdentifier,
                spendAmount, spendAmount, 0, 0, proposedOrder.DiscountAmountCents, null, null, null, null, true, null);

            Assert.AreEqual(completedOrder.SpendAmount, credit + giftCardAmount);
            Assert.AreNotEqual(completedOrder.SpendAmount, spendAmount);

            ICreateRefund refundClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<ICreateRefund>();
            refundClient.RefundOrder(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken, completedOrder.OrderIdentifier);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.IntegrationTests)]
        [ExpectedException(typeof(Http.LevelUpApiException))]
        public void ProposedOrderWithNoPartialAuth_ShouldThrow_ForInsufficientCredit()
        {
            int giftCardAmount = 1000;
            ClientModuleIntegrationTestingUtilities.RemoveAnyGiftCardCreditOnConsumerWithNoLinkedPaymentUserAccount();
            ClientModuleIntegrationTestingUtilities.AddGiftCardCreditOnConsumerWithNoLinkedPaymentUserAccount(giftCardAmount);

            var discountCredit = ClientModuleIntegrationTestingUtilities.GetAvailableDiscountCredit(LevelUpTestConfiguration.Current.ConsumerWithNoLinkedPaymentQrData);
            
            int spendAmount = (1000 + giftCardAmount + discountCredit);


            IManageProposedOrders orderClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IManageProposedOrders>();
            var proposedOrder = orderClient.CreateProposedOrder(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                LevelUpTestConfiguration.Current.MerchantLocationId,
                LevelUpTestConfiguration.Current.ConsumerWithNoLinkedPaymentQrData,
                spendAmount, spendAmount, 0, 0, null, null, null, null, false, null);

            var completedOrder = orderClient.CompleteProposedOrder(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                LevelUpTestConfiguration.Current.MerchantLocationId,
                LevelUpTestConfiguration.Current.ConsumerWithNoLinkedPaymentQrData,
                proposedOrder.ProposedOrderIdentifier,
                spendAmount, spendAmount, 0, 0, proposedOrder.DiscountAmountCents, null, null, null, null, false, null);

            Assert.Fail("Failed to throw a LevelUpAPIException for an order with no partial auth and a spend amount " +
                        "greater than the user's available credit.");
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.IntegrationTests)]
        public void ProposedOrderWith10PercentTip_ShouldSucceed()
        {
            const int spendAmountCents = 10;
            const int tipAmountCents = 1; // 10%
            
            IManageProposedOrders orderClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IManageProposedOrders>();
            var proposedOrder = orderClient.CreateProposedOrder(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                LevelUpTestConfiguration.Current.MerchantLocationId,
                LevelUpTestConfiguration.Current.ConsumerQrDataWith10PercentTip,
                spendAmountCents, spendAmountCents,
                0, 0, null, null, null, null, true, null);

            var completedOrder = orderClient.CompleteProposedOrder(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                LevelUpTestConfiguration.Current.MerchantLocationId,
                LevelUpTestConfiguration.Current.ConsumerQrDataWith10PercentTip,
                proposedOrder.ProposedOrderIdentifier,
                spendAmountCents, spendAmountCents,
                0, 0, proposedOrder.DiscountAmountCents, null, null, null, null, true, null);

            Assert.AreEqual(completedOrder.TipAmount, tipAmountCents);
            
            var refundClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<ICreateRefund>();
            refundClient.RefundOrder(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken, completedOrder.OrderIdentifier);
        }
    }
}
