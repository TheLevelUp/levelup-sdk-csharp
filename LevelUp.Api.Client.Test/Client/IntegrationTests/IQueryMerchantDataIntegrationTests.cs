#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="IQueryMerchantDataIntegrationTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LevelUp.Api.Client.Test.Client.IntegrationTests
{
    [TestClass]
    public class IQueryMerchantDataIntegrationTests
    {
        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.IntegrationTests)]
        public void GetLocationDetails()
        {
            IQueryMerchantData queryClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IQueryMerchantData>();
            var locationDetails = queryClient.GetLocationDetails( ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken, 
                LevelUpTestConfiguration.Current.MerchantLocationId);

            Assert.AreEqual(locationDetails.MerchantId, LevelUpTestConfiguration.Current.MerchantId);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.IntegrationTests)]
        public void ListLocations()
        {
            IQueryMerchantData queryClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IQueryMerchantData>();
            var locations = queryClient.ListLocations(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                LevelUpTestConfiguration.Current.MerchantId);

            Assert.IsTrue(locations.Count > 0);
            Assert.AreEqual(locations.Count(x => x.LocationId == LevelUpTestConfiguration.Current.MerchantLocationId), 1);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.IntegrationTests)]
        public void ListManagedLocations()
        {
            IQueryMerchantData queryClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IQueryMerchantData>();
            var locations = queryClient.ListManagedLocations(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken);

            Assert.IsTrue(locations.Count > 0);
            Assert.AreEqual(locations.Count(x => x.LocationId == LevelUpTestConfiguration.Current.MerchantLocationId), 1);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.IntegrationTests)]
        public void GetMerchantOrderDetails()
        {
            ClientModuleIntegrationTestingUtilities.RemoveAnyGiftCardCreditOnConsumerUserAccount();

            IQueryMerchantData queryClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IQueryMerchantData>();

            var ordered = ClientModuleIntegrationTestingUtilities.PlaceOrderAtTestMerchantWithTestConsumer(1000);

            var queriedOrderDetails = queryClient.GetMerchantOrderDetails(
                ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken, 
                LevelUpTestConfiguration.Current.MerchantId,
                ordered.OrderIdentifier);

            Assert.AreEqual(queriedOrderDetails.OrderIdentifier, ordered.OrderIdentifier);
            Assert.AreEqual(queriedOrderDetails.SpendAmount, ordered.SpendAmount);
            Assert.AreEqual(queriedOrderDetails.TipAmount, ordered.TipAmount);
            Assert.AreEqual(queriedOrderDetails.Total, ordered.Total);
        }
    }
}
