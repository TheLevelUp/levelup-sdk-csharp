#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="ICreateCreditCardsIntegrationTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using LevelUp.Api.Client.Models.Responses;
using NUnit.Framework;

namespace LevelUp.Api.Client.Tests.Client.IntegrationTests
{
    [TestFixture, Explicit]
    public class ICreateCreditCardsIntegrationTests
    {
        [Test]
        // Braintree enforces a 50-cards-per-user restriction, so we can't keep adding/removing cards for the test user.
        public void CreateCreditCards()
        {
            ICreateCreditCards createInterface = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<ICreateCreditCards>();
            IDestroyCreditCards destroyInterface = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IDestroyCreditCards>();
            IQueryCreditCards queryInterface = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IQueryCreditCards>();

            CreditCard created = createInterface.CreateCreditCard(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken, 
                ClientModuleIntegrationTestingUtilities.SandboxIntegrationTestCreditCard);

            List<CreditCard> creditCards = new List<CreditCard>(queryInterface.ListCreditCards(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken));
            Assert.AreEqual(creditCards.FindAll(x => x.Id == created.Id).Count, 1);
            
            destroyInterface.DeleteCreditCard(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken, created.Id);
            
            creditCards = new List<CreditCard> (queryInterface.ListCreditCards(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken));
            Assert.AreEqual(creditCards.FindAll(x => x.Id == created.Id).Count, 0);

            // Note: The card won't actually be deleted in the cleanup for this test, just deactivated.  AFAIK cards cannot be deleted via 
            // the api, and they just languish in a deactivated state.  As a result, the sandbox merchant account that is used for integration 
            // tests may wind up with a bunch of old credit card entries.
        }
    }
}
