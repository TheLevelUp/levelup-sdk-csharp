#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="IQueryCreditCardsIntegrationTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using NUnit.Framework;

namespace LevelUp.Api.Client.Tests.Client.IntegrationTests
{
    [TestFixture, Explicit]
    public class IQueryCreditCardsIntegrationTests
    {
        [Test]  // Braintree enforces a 50-cards-per-user restriction, so we can't keep adding/removing cards for the 
                 // test user.  This test requires adding/removing cards.
        public void QueryCreditCards()
        {
            IQueryCreditCards queryInterface = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IQueryCreditCards>();
            ICreateCreditCards createInterface = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<ICreateCreditCards>();
            IDestroyCreditCards destroyInterface = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IDestroyCreditCards>();

            var existingCards = queryInterface.ListCreditCards(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpUserAccessToken);
            foreach (var card in existingCards)
            {
                destroyInterface.DeleteCreditCard(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpUserAccessToken, card.Id);
            }

            Assert.AreEqual(queryInterface.ListCreditCards(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpUserAccessToken).Count, 0);

            createInterface.CreateCreditCard(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpUserAccessToken,
                ClientModuleIntegrationTestingUtilities.SandboxIntegrationTestCreditCard);

            var newCards = queryInterface.ListCreditCards(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpUserAccessToken);
            Assert.AreEqual(newCards.Count, 1);
            Assert.AreEqual(newCards[0].State.ToLower(), "active");
            Assert.AreEqual(newCards[0].Promoted, true);
        }
    }
}