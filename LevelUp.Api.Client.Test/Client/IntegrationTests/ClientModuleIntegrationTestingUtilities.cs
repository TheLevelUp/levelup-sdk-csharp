#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="ClientModuleIntegrationTestingUtilities.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using LevelUp.Api.Client.Models.Responses;
using LevelUp.Api.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LevelUp.Api.Client.Test.Client.IntegrationTests
{
    internal static class ClientModuleIntegrationTestingUtilities
    {
        /// <summary>
        /// Gets a LevelUp module that hits sandbox endpoints.
        /// </summary>
        /// <typeparam name="T">The specific type of ILevelUpClientModule to return.</typeparam>
        internal static T GetSandboxedLevelUpModule<T>() where T : ILevelUpClientModule
        {
            return LevelUpClientFactory.Create<T>(
                new AgentIdentifier(LevelUpTestConfiguration.Current.CompanyName, 
                                    LevelUpTestConfiguration.Current.ProductName,
                                    LevelUpTestConfiguration.Current.ProductVersion, 
                                    LevelUpTestConfiguration.Current.OperatingSystem), 
                LevelUpEnvironment.Sandbox);
        }

        /// <summary>
        /// Gets a LevelUp access token that is compatible with sandbox endpoints, and can be used in sandbox request headers.
        /// </summary>
        internal static string SandboxedLevelUpMerchantAccessToken
        {
            get { return MerchantAccessTokenHolder.SandboxedLevelUpMerchantAccessToken; }
        }

        private static class MerchantAccessTokenHolder
        {
            internal static readonly string SandboxedLevelUpMerchantAccessToken;

            static MerchantAccessTokenHolder()
            {
                IAuthenticate auth = GetSandboxedLevelUpModule<IAuthenticate>();
                var token = auth.Authenticate(LevelUpTestConfiguration.Current.ClientId,
                    LevelUpTestConfiguration.Current.MerchantUsername,
                    LevelUpTestConfiguration.Current.MerchantPassword);
                SandboxedLevelUpMerchantAccessToken = token.Token;
            }
        }

        /// <summary>
        /// Gets a LevelUp access token that is compatible with sandbox endpoints, and can be used in sandbox request headers.
        /// </summary>
        internal static string SandboxedLevelUpUserAccessToken
        {
            get { return UserAccessTokenHolder.SandboxedLevelUpUserAccessToken; }
        }

        private static class UserAccessTokenHolder
        {
            internal static readonly string SandboxedLevelUpUserAccessToken;

            static UserAccessTokenHolder()
            {
                IAuthenticate auth = GetSandboxedLevelUpModule<IAuthenticate>();
                var token = auth.Authenticate(LevelUpTestConfiguration.Current.ClientId,
                    LevelUpTestConfiguration.Current.ConsumerUsername,
                    LevelUpTestConfiguration.Current.ConsumerPassword);
                SandboxedLevelUpUserAccessToken = token.Token;
            }
        }

        internal static CreateCreditCardRequestBody SandboxIntegrationTestCreditCard
        {
            get
            {
                return new CreateCreditCardRequestBody(
                    encryptedCvv: LevelUpTestConfiguration.Current.TestCreditCardEncryptedCvv,
                    encryptedExpirationMonth: "1",
                    encryptedExpirationYear: "2015",
                    encryptedNumber: LevelUpTestConfiguration.Current.TestCreditCardEncryptedNumber,
                    postalCode: "12345"
                    );
            }
        }

        internal static void RemoveAnyGiftCardCreditOnConsumerUserAccount()
        {
            RemoveAnyGiftCardOnAccount( LevelUpTestConfiguration.Current.MerchantId, 
                                        LevelUpTestConfiguration.Current.MerchantLocationId,
                                        LevelUpTestConfiguration.Current.ConsumerQrData);
        }

        internal static void RemoveAnyGiftCardCreditOnConsumerWithNoLinkedPaymentUserAccount()
        {
            RemoveAnyGiftCardOnAccount( LevelUpTestConfiguration.Current.MerchantId,
                                        LevelUpTestConfiguration.Current.MerchantLocationId,
                                        LevelUpTestConfiguration.Current.ConsumerWithNoLinkedPaymentQrData);
        }

        private static void RemoveAnyGiftCardOnAccount(int merchantId, int merchantLocationId, string userQrCode)
        {
            IRetrieveMerchantFundedGiftCardCredit creditClient = GetSandboxedLevelUpModule<IRetrieveMerchantFundedGiftCardCredit>();
            IDestroyGiftCardValue giftCardDestroyClient = GetSandboxedLevelUpModule<IDestroyGiftCardValue>();

            var credit = creditClient.GetMerchantFundedGiftCardCredit(SandboxedLevelUpMerchantAccessToken,
               merchantLocationId, userQrCode);

            if (credit.TotalAmount == 0)
                return;

            giftCardDestroyClient.GiftCardDestroyValue(SandboxedLevelUpMerchantAccessToken, 
                merchantId, userQrCode, credit.TotalAmount);

            credit = creditClient.GetMerchantFundedGiftCardCredit(SandboxedLevelUpMerchantAccessToken,
               merchantLocationId, userQrCode);

            Assert.IsTrue(credit.TotalAmount == 0);
        }

        internal static void AddGiftCardCreditOnConsumerUserAccount(int amountToAdd)
        {
            AddGiftCardCreditOnUserAccount(LevelUpTestConfiguration.Current.ConsumerQrData, amountToAdd);
        }

        internal static void AddGiftCardCreditOnConsumerWithNoLinkedPaymentUserAccount(int amountToAdd)
        {
            AddGiftCardCreditOnUserAccount(LevelUpTestConfiguration.Current.ConsumerWithNoLinkedPaymentQrData, amountToAdd);
        }

        internal static void AddGiftCardCreditOnUserAccount(string userQrCode, int amountToAdd)
        {
            IRetrieveMerchantFundedGiftCardCredit creditClient = GetSandboxedLevelUpModule<IRetrieveMerchantFundedGiftCardCredit>();
            ICreateGiftCardValue giftCardClient = GetSandboxedLevelUpModule<ICreateGiftCardValue>();

            var initialCredit = creditClient.GetMerchantFundedGiftCardCredit(   
                ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                LevelUpTestConfiguration.Current.MerchantLocationId, userQrCode);

            giftCardClient.GiftCardAddValue(SandboxedLevelUpMerchantAccessToken,
                                            LevelUpTestConfiguration.Current.MerchantId, 
                                            LevelUpTestConfiguration.Current.MerchantLocationId, 
                                            userQrCode, 
                                            amountToAdd);

            var newCredit = creditClient.GetMerchantFundedGiftCardCredit(SandboxedLevelUpMerchantAccessToken,
                LevelUpTestConfiguration.Current.MerchantLocationId, userQrCode);

            Assert.AreEqual(newCredit.TotalAmount - initialCredit.TotalAmount, amountToAdd);
        }

        internal static CompletedOrderResponse PlaceOrderAtTestMerchantWithTestConsumer(int total = 100)
        {
            ClientModuleIntegrationTestingUtilities.RemoveAnyGiftCardCreditOnConsumerUserAccount();

            IManageProposedOrders orderClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IManageProposedOrders>();
            var proposedOrder = orderClient.CreateProposedOrder(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                LevelUpTestConfiguration.Current.MerchantLocationId,
                LevelUpTestConfiguration.Current.ConsumerQrData,
                total, total, 0, 0, null, null, null, null, true, null);

            return orderClient.CompleteProposedOrder(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                LevelUpTestConfiguration.Current.MerchantLocationId,
                LevelUpTestConfiguration.Current.ConsumerQrData,
                proposedOrder.ProposedOrderIdentifier,
                total, total, 0, 0, proposedOrder.DiscountAmountCents, null, null, null, null, true, null);
        }

        internal static int GetAvailableDiscountCredit(string qrData)
        {
            IManageProposedOrders orderClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IManageProposedOrders>();

            var proposedOrder = orderClient.CreateProposedOrder(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                LevelUpTestConfiguration.Current.MerchantLocationId,
                qrData, int.MaxValue, int.MaxValue, 0, 0, null, null, null, null, true, null);

            return proposedOrder.DiscountAmountCents;
        }
    }
}
