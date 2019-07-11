#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="IQueryUsersIntegrationTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using System.Linq;
using LevelUp.Api.Client.ClientInterfaces;
using LevelUp.Api.Client.Models.Responses;
using NUnit.Framework;

namespace LevelUp.Api.Client.Tests.Client.IntegrationTests
{
    [TestFixture, Explicit]
    public class QueryUsersIntegrationTests
    {
        [Test]
        public void GetUser()
        {
            IQueryUser queryInterface = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IQueryUser>();

            var userData = queryInterface.GetUser(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpUserAccessToken,
                LevelUpTestConfiguration.Current.ConsumerId);

            Assert.AreEqual(userData.FirstName, LevelUpTestConfiguration.Current.ConsumerUserFirstName);
            Assert.AreEqual(userData.LastName.ToLower().ToCharArray()[0], 
                LevelUpTestConfiguration.Current.ConsumerUserLastInitial.ToLower().ToCharArray()[0]);
        }

        [Test]
        public void GetLocationDetails()
        {
            IQueryUser queryInterface = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IQueryUser>();

            var locationData = queryInterface.ListUserAddresses(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpUserAccessToken);
            Assert.IsTrue(locationData.Count > 0);
            Assert.IsNotNull(locationData.Where(x => x.StreetAddress == "101 Arch Street").DefaultIfEmpty(null).FirstOrDefault());
        }

        [Test]
        public void GetLocationUserCredit()
        {
            int merchantFundedCreditAmount = 175;

            ClientModuleIntegrationTestingUtilities.GrantMerchantFundedCredit(merchantFundedCreditAmount);

            IQueryUser queryInterface = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IQueryUser>();

            var credit = queryInterface.GetLocationUserCredit(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpUserAccessToken,
                LevelUpTestConfiguration.Current.MerchantLocationId);

            // Verify that the credit is at least the amount of credit we issued.
            Assert.GreaterOrEqual(credit.TotalAmount, merchantFundedCreditAmount);

            // For this test, we don't really care what the order number is...
            var identifierFromMerchant = System.Environment.TickCount.ToString();

            // Remove the credit we gave to the user.
            CompletedOrderResponse completedOrder = ClientModuleIntegrationTestingUtilities.CompleteOrderAtTestMerchantWithTestConsumer(
                merchantFundedCreditAmount,
                merchantFundedCreditAmount,
                identifierFromMerchant);
        }
    }
}
