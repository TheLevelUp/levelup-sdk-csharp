﻿#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="ICreateGiftCardValueIntegrationTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using LevelUp.Api.Client.Models.Responses;
using LevelUp.Api.Http;
using NUnit.Framework;

namespace LevelUp.Api.Client.Tests.Client.IntegrationTests
{
    [TestFixture, Explicit]
    public class ICreateGiftCardValueIntegrationTests
    {
        [Test]
        public void CreateGiftCardValue()
        {
            const int valueToAdd = 1000;

            ICreateGiftCardValue createInterface = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<ICreateGiftCardValue>();
            ILookupUserLoyalty loyaltyInterface = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<ILookupUserLoyalty>();
            IDestroyGiftCardValue destroyInterface = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IDestroyGiftCardValue>();

            Loyalty initialLoyalty = loyaltyInterface.GetLoyalty(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpUserAccessToken, 
                LevelUpTestConfiguration.Current.MerchantId);

            var createdGiftCard = createInterface.GiftCardAddValue( ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken, 
                                                                    LevelUpTestConfiguration.Current.MerchantId, 
                                                                    LevelUpTestConfiguration.Current.MerchantLocationId, 
                                                                    LevelUpTestConfiguration.Current.ConsumerQrData, 
                                                                    valueToAdd);

            Assert.AreEqual(createdGiftCard.AmountAddedInCents, valueToAdd);

            Loyalty postAdditionLoyalty = loyaltyInterface.GetLoyalty(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpUserAccessToken, 
                LevelUpTestConfiguration.Current.MerchantId);

            Assert.AreEqual(postAdditionLoyalty.PotentialCreditAmount - initialLoyalty.PotentialCreditAmount, valueToAdd);

            // Cleanup
            destroyInterface.GiftCardDestroyValue(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                LevelUpTestConfiguration.Current.MerchantId, LevelUpTestConfiguration.Current.ConsumerQrData, valueToAdd);

            Loyalty postDestructionLoyalty = loyaltyInterface.GetLoyalty(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpUserAccessToken, 
                LevelUpTestConfiguration.Current.MerchantId);
            Assert.AreEqual(postDestructionLoyalty.PotentialCreditAmount, initialLoyalty.PotentialCreditAmount);
        }

        [Test]
        public void CreateGiftCardValue_BelowMinimum()
        {
            const int valueToAdd = 50;

            ICreateGiftCardValue createInterface = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<ICreateGiftCardValue>();

            Assert.Throws<LevelUpApiException>(() =>
            {
                createInterface.GiftCardAddValue(
                    ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                    LevelUpTestConfiguration.Current.MerchantId,
                    LevelUpTestConfiguration.Current.MerchantLocationId,
                    LevelUpTestConfiguration.Current.ConsumerQrData,
                    valueToAdd);
            }, "Failed to throw exception for a gift card value below $10.");

        }

        [Test]
        public void CreateGiftCardValue_NegativeAmount()
        {
            const int valueToAdd = -50;

            ICreateGiftCardValue createInterface = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<ICreateGiftCardValue>();

            Assert.Throws<LevelUpApiException>(() =>
            {
                createInterface.GiftCardAddValue(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                    LevelUpTestConfiguration.Current.MerchantId,
                    LevelUpTestConfiguration.Current.MerchantLocationId,
                    LevelUpTestConfiguration.Current.ConsumerQrData,
                    valueToAdd);
            }, "Failed to throw exception for a negative gift card value.");
        }
    }
}
