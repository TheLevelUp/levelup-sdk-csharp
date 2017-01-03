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
using LevelUp.Api.Client.Models.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LevelUp.Api.Client.Test.Client
{
    [TestClass]
    public class IQueryMerchantDataIntegrationTests
    {
        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.IntegrationTests)]
        public void GetLocationDetails()
        {
            IQueryMerchantData queryClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IQueryMerchantData>();
            var locationDetails = queryClient.GetLocationDetails( ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken, 
                LevelUpTestConfiguration.Current.MerchantLocationId);

            Assert.AreEqual(locationDetails.MerchantId, LevelUpTestConfiguration.Current.MerchantId);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.IntegrationTests)]
        public void ListLocations()
        {
            IQueryMerchantData queryClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IQueryMerchantData>();
            var locations = queryClient.ListLocations(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                LevelUpTestConfiguration.Current.MerchantId);

            Assert.IsTrue(locations.Count > 0);
            Assert.AreEqual(locations.Count(x => x.LocationId == LevelUpTestConfiguration.Current.MerchantLocationId), 1);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.IntegrationTests)]
        public void ListManagedLocations()
        {
            IQueryMerchantData queryClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IQueryMerchantData>();
            var locations = queryClient.ListManagedLocations(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken);

            Assert.IsTrue(locations.Count > 0);
            Assert.AreEqual(locations.Count(x => x.LocationId == LevelUpTestConfiguration.Current.MerchantLocationId), 1);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.IntegrationTests)]
        public void GetMerchantOrderDetails()
        {
            ClientModuleIntegrationTestingUtilities.RemoveAnyGiftCardCreditOnConsumerUserAccount();

            IQueryMerchantData queryClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IQueryMerchantData>();
            ICreateOrders orderClient = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<ICreateOrders>();

            Order toPlace = new Order(  LevelUpTestConfiguration.Current.MerchantLocationId, 
                                        LevelUpTestConfiguration.Current.ConsumerQrData,
                                        1000, 
                                        0, 0, 0, null, null, 
                                        "GetMerchantOrderDetails_integration_test", 
                                        true, null);
            var placed = orderClient.PlaceOrder(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken, toPlace);

            var queriedOrderDetails = queryClient.GetMerchantOrderDetails(
                ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken, 
                LevelUpTestConfiguration.Current.MerchantId, 
                placed.OrderIdentifier);

            Assert.AreEqual(queriedOrderDetails.OrderIdentifier, placed.OrderIdentifier);
            Assert.AreEqual(queriedOrderDetails.SpendAmount, placed.SpendAmount);
            Assert.AreEqual(queriedOrderDetails.TipAmount, placed.TipAmount);
            Assert.AreEqual(queriedOrderDetails.Total, placed.Total);
        }
    }
}
