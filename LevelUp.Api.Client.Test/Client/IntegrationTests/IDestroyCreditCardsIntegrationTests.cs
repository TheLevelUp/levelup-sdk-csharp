#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="IDestroyCreditCardsIntegrationTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
    public class IDestroyCreditCardsIntegrationTests
    {
        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.IntegrationTests)]
        [Ignore] // Braintree enforces a 50-cards-per-user restriction, so we can't keep adding/removing cards for the test user.
        public void DestroyCard()
        {
            IQueryCreditCards queryClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IQueryCreditCards>();
            IDestroyCreditCards destroyClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IDestroyCreditCards>();
            ICreateCreditCards createClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<ICreateCreditCards>();

            var toCreate = createClient.CreateCreditCard(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpUserAccessToken, 
                ClientModuleIntegrationTestingUtilities.SandboxIntegrationTestCreditCard);

            var cards = queryClient.ListCreditCards(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpUserAccessToken);
            Assert.AreEqual(cards.Count, 1);
            Assert.AreEqual(cards[0].Active, true);
            Assert.AreEqual(cards[0].Id, toCreate.Id );

            destroyClient.DeleteCreditCard(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpUserAccessToken, toCreate.Id);

            cards = queryClient.ListCreditCards(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpUserAccessToken);
            Assert.AreEqual(cards.Count, 0);

            createClient.CreateCreditCard(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpUserAccessToken,
                ClientModuleIntegrationTestingUtilities.SandboxIntegrationTestCreditCard);
        }
    }
}
