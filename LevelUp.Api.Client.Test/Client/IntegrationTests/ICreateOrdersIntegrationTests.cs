#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="ICreateOrdersIntegrationTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using LevelUp.Api.Client.ClientInterfaces;
using LevelUp.Api.Client.Models.Requests;
using LevelUp.Api.Client.Models.Responses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LevelUp.Api.Client.Test.Client
{
    [TestClass]
    public class ICreateOrdersIntegrationTests
    {
        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.IntegrationTests)]
        public void CreateOrder()
        {
            const int amountToSpendCents = 2000;

            PlaceOrderWithGiftCardAndRefundIt(amountToSpendCents, 0);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.IntegrationTests)]
        public void CreateOrderWithGiftCardCredit()
        {
            const int amountToSpendCents = 2000;
            const int giftCardAmount = 1000;

            PlaceOrderWithGiftCardAndRefundIt(amountToSpendCents, giftCardAmount);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.IntegrationTests)]
        [ExpectedException(typeof(LevelUp.Api.Http.LevelUpApiException))]
        public void CreateOrderWithBadQrCode()
        {
            ICreateOrders ordersClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<ICreateOrders>();

            Order toPlace = new Order(LevelUpTestConfiguration.Current.MerchantLocationId, "bad_qr_code", 10, null, null, null);

            ordersClient.PlaceOrder(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken, toPlace);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.IntegrationTests)]
        public void CreateOrderWithExemption()
        {
            IRetrieveMerchantFundedCredit creditClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IRetrieveMerchantFundedCredit>();
            ICreateRefund refundClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<ICreateRefund>();

            const int spendGiftCardAndExemptionAmount = 1000;

            ClientModuleIntegrationTestingUtilities.RemoveAnyGiftCardCreditOnConsumerUserAccount();
            ClientModuleIntegrationTestingUtilities.AddGiftCardCreditOnConsumerUserAccount(spendGiftCardAndExemptionAmount);

            var order = PlaceOrderOnTestUserAccountForTestMerchant( LevelUpTestConfiguration.Current.MerchantLocationId,
                                                                    LevelUpTestConfiguration.Current.ConsumerQrData, 
                                                                    spendGiftCardAndExemptionAmount, 
                                                                    spendGiftCardAndExemptionAmount, 
                                                                    spendGiftCardAndExemptionAmount, 
                                                                    0, 
                                                                    true);

            var credit = creditClient.GetMerchantFundedCredit(  ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                                                                LevelUpTestConfiguration.Current.MerchantLocationId, 
                                                                LevelUpTestConfiguration.Current.ConsumerQrData);
            Assert.AreEqual(credit.GiftCardAmount, spendGiftCardAndExemptionAmount);    // GiftCard should not have been used.

            var refund = refundClient.RefundOrder(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken, order.OrderIdentifier);
            Assert.IsNotNull(refund.TimeOfRefund);

            ClientModuleIntegrationTestingUtilities.RemoveAnyGiftCardCreditOnConsumerUserAccount();
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.IntegrationTests)]
        public void CreateOrderWithPartialAuth()
        {
            ClientModuleIntegrationTestingUtilities.RemoveAnyGiftCardCreditOnConsumerWithNoLinkedPaymentUserAccount();

            IRetrieveMerchantFundedCredit creditClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IRetrieveMerchantFundedCredit>();
            ICreateRefund refundClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<ICreateRefund>();

            var credit = creditClient.GetMerchantFundedCredit(  ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                                                                LevelUpTestConfiguration.Current.MerchantLocationId, 
                                                                LevelUpTestConfiguration.Current.ConsumerWithNoLinkedPaymentQrData);

            int spendAmount = (2000 + credit.DiscountAmount);
            int giftCardAmount = 1000;

            ClientModuleIntegrationTestingUtilities.AddGiftCardCreditOnConsumerWithNoLinkedPaymentUserAccount(giftCardAmount);
            var order = PlaceOrderOnTestUserAccountForTestMerchant( LevelUpTestConfiguration.Current.MerchantLocationId,
                                                                    LevelUpTestConfiguration.Current.ConsumerWithNoLinkedPaymentQrData, 
                                                                    spendAmount, 
                                                                    giftCardAmount, 
                                                                    0, 
                                                                    credit.DiscountAmount, 
                                                                    true);

            Assert.AreEqual(order.SpendAmount, credit.DiscountAmount + giftCardAmount);
            Assert.AreNotEqual(order.SpendAmount,spendAmount);

            refundClient.RefundOrder(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,order.OrderIdentifier);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.IntegrationTests)]
        [ExpectedException(typeof(Http.LevelUpApiException))]
        public void CreateOrderWithNoPartialAuth()
        {
            ClientModuleIntegrationTestingUtilities.RemoveAnyGiftCardCreditOnConsumerWithNoLinkedPaymentUserAccount();

            IRetrieveMerchantFundedCredit creditClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IRetrieveMerchantFundedCredit>();

            var credit = creditClient.GetMerchantFundedCredit(  ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                                                                LevelUpTestConfiguration.Current.MerchantLocationId, 
                                                                LevelUpTestConfiguration.Current.ConsumerWithNoLinkedPaymentQrData);

            int spendAmount = (2000 + credit.DiscountAmount);
            int giftCardAmount = 1000;

            ClientModuleIntegrationTestingUtilities.AddGiftCardCreditOnConsumerWithNoLinkedPaymentUserAccount(giftCardAmount);
            PlaceOrderOnTestUserAccountForTestMerchant( LevelUpTestConfiguration.Current.MerchantLocationId, 
                                                        LevelUpTestConfiguration.Current.ConsumerWithNoLinkedPaymentQrData, 
                                                        spendAmount, 
                                                        giftCardAmount, 
                                                        0, 
                                                        credit.DiscountAmount, 
                                                        false);

            Assert.Fail("Failed to throw a LevelUpAPIException for an order with no partial auth and a spend amount " +
                        "greater than the user's available credit.");
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.IntegrationTests)]
        public void CreateOrder_With10PercentTip()
        {
            const int expectedSpendAmountCents = 10;
            const int tipPercent = 10;
            const double tipDecimal = (double)tipPercent / 100;
            const double tipFactor = 1 + tipDecimal;

            ICreateOrders ordersClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<ICreateOrders>();

            var order = new Order(  LevelUpTestConfiguration.Current.MerchantLocationId,
                                    LevelUpTestConfiguration.Current.ConsumerQrDataWith10PercentTip, 
                                    expectedSpendAmountCents, 
                                    null, null, null, null);

            var orderResponse = ordersClient.PlaceOrder(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken, order);

            Assert.IsNotNull(orderResponse);
            Assert.AreEqual(expectedSpendAmountCents, orderResponse.SpendAmount);
            Assert.AreEqual(tipDecimal * expectedSpendAmountCents, orderResponse.TipAmount);
            Assert.AreEqual(tipFactor * expectedSpendAmountCents, orderResponse.Total);

            var refundClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<ICreateRefund>();
            refundClient.RefundOrder(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken, orderResponse.OrderIdentifier);
        }

        private void PlaceOrderWithGiftCardAndRefundIt(int spendAmount, int giftCardAmountToUse)
        {
            ClientModuleIntegrationTestingUtilities.RemoveAnyGiftCardCreditOnConsumerUserAccount();
            if (giftCardAmountToUse != 0)
            {
                ClientModuleIntegrationTestingUtilities.AddGiftCardCreditOnConsumerUserAccount(giftCardAmountToUse);
            }

            var order = PlaceOrderOnTestUserAccountForTestMerchant( LevelUpTestConfiguration.Current.MerchantLocationId, 
                                                                    LevelUpTestConfiguration.Current.ConsumerQrData, 
                                                                    spendAmount, 
                                                                    giftCardAmountToUse, 
                                                                    0, 
                                                                    0, 
                                                                    true);

            ICreateRefund refundClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<ICreateRefund>();
            var refund = refundClient.RefundOrder(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken, order.OrderIdentifier);
            Assert.IsNotNull(refund.TimeOfRefund);

            ClientModuleIntegrationTestingUtilities.RemoveAnyGiftCardCreditOnConsumerUserAccount();
        }

        internal OrderResponse PlaceOrderOnTestUserAccountForTestMerchant(int merchantLocationId, string userQrCode, int spendAmount, 
            int giftCardAmount, int exemptionAmount, int discountAmount, bool allowPartialAuth)
        {
            ICreateOrders ordersClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<ICreateOrders>();

            IList<Item> orderItems = new List<Item>
            {
                new Item("Hamburger", "A cow in between two slices of bread", "S12T-Bug-RM", "123456789999",
                    "grill", spendAmount, spendAmount, 1, null)
            };

            Order toPlace = new Order(merchantLocationId, userQrCode, spendAmount, discountAmount, giftCardAmount,
                exemptionAmount, null, null, "CreateOrder_integration_test", allowPartialAuth, orderItems);

            return ordersClient.PlaceOrder(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken, toPlace);
        }
    }
}
