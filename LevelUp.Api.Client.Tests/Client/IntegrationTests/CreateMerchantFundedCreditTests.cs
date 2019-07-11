#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="ICreateCreditCardsFunctionalTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using NUnit.Framework;
using System;

namespace LevelUp.Api.Client.Tests.Client.IntegrationTests
{
    [TestFixture, Explicit]
    public class CreateMerchantFundedCreditTests
    {
        [Test]
        public void GrantMerchantFundedCredit()
        {
            int valueAmount = 175;
            int durationInSeconds = 60 * 60;

            ICreateMerchantFundedCredit merchantFundedCreditInterface = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<ICreateMerchantFundedCredit>();
            merchantFundedCreditInterface.GrantMerchantFundedCredit(
                ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                LevelUpTestConfiguration.Current.ConsumerEmailAddress,
                durationInSeconds,
                LevelUpTestConfiguration.Current.MerchantId,
                $"Simphony2 test from {LevelUpTestConfiguration.Current.CompanyName} {DateTime.Now}",
                valueAmount,
                false);

            // For this test, we don't really care what the order number is...
            var identifierFromMerchant = System.Environment.TickCount.ToString();

            ProposedOrderResponse proposedOrder = ClientModuleIntegrationTestingUtilities.ProposedOrderAtTestMerchantWithTestConsumer(valueAmount, identifierFromMerchant);

            Assert.AreEqual(proposedOrder.DiscountAmountCents, valueAmount);

            CompletedOrderResponse completedOrder = ClientModuleIntegrationTestingUtilities.CompleteOrderAtTestMerchantWithTestConsumer(
                valueAmount,
                proposedOrder.DiscountAmountCents,
                identifierFromMerchant);

            Assert.AreEqual(completedOrder.Total, valueAmount);
        }
    }
}