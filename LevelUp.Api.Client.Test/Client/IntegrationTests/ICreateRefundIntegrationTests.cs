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
using LevelUp.Api.Client.Models.Requests;
using LevelUp.Api.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LevelUp.Api.Client.Test.Client
{
    [TestClass]
    public class ICreateRefundIntegrationTests
    {
        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.IntegrationTests)]
        public void CreateRefundForV14Order()
        {
            ClientModuleIntegrationTestingUtilities.RemoveAnyGiftCardCreditOnConsumerUserAccount();
            ClientModuleIntegrationTestingUtilities.AddGiftCardCreditOnConsumerUserAccount(1000);

            ICreateOrders orderInterface = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<ICreateOrders>();
            ICreateRefund refundInterface = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<ICreateRefund>();
            
            Order toPlace = new Order( LevelUpTestConfiguration.Current.MerchantLocationId, LevelUpTestConfiguration.Current.ConsumerQrData,
                2000, 500, 1000, 50, null, null, "CreateOrder_integration_test", true, null);

            var order = orderInterface.PlaceOrder(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken, toPlace);
            var refund = refundInterface.RefundOrder(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken, order.OrderIdentifier);

            Assert.IsNotNull(refund.TimeOfRefund);
            Assert.AreEqual(refund.TotalAmount, order.Total);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.IntegrationTests)]
        public void CreateRefundForV15ProposedOrder()
        {
            // These are essentially the same test.
            IManageProposedOrdersIntegrationTests tmp = new IManageProposedOrdersIntegrationTests();
            tmp.TwoStagePlaceOrderWorkflowShouldSucceed();
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.IntegrationTests)]
        [ExpectedException(typeof(Http.LevelUpApiException))]
        public void CreateRefundForNonExistantOrder()
        {
            ICreateRefund refundInterface = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<ICreateRefund>();
            refundInterface.RefundOrder(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken, "aaa");
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.IntegrationTests)]
        [ExpectedException(typeof(LevelUpApiException))]
        public void CreateRefundForUnFinalizedProposedOrder()
        {
            IRetrieveMerchantFundedCredit creditClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IRetrieveMerchantFundedCredit>();

            var availableCredit = creditClient.GetMerchantFundedCredit(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                                                                        LevelUpTestConfiguration.Current.MerchantLocationId,
                                                                        LevelUpTestConfiguration.Current.ConsumerQrData);
            int availableDiscountCents = availableCredit.DiscountAmount;

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
                                                                taxAmountInCents,
                                                                0, null, null, null, null, true, null);

           ICreateRefund refundClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<ICreateRefund>();
           refundClient.RefundOrder(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                proposedOrder.ProposedOrderIdentifier);
        }
    }
}
            
            

