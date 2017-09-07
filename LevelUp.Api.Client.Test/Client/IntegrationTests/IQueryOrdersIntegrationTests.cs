#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="IQueryOrdersIntegrationTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using System.Linq;
using LevelUp.Api.Client.ClientInterfaces;
using LevelUp.Api.Client.Models.Responses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LevelUp.Api.Client.Test.Client
{
    [TestClass]
    public class IQueryOrdersIntegrationTests
    {
        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.IntegrationTests)]
        public void ListOrders()
        {
            CompletedOrderResponse created = ClientModuleIntegrationTestingUtilities.PlaceOrderAtTestMerchantWithTestConsumer();

            IQueryOrders queryInterface = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IQueryOrders>();
            var orders = queryInterface.ListOrders(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                    LevelUpTestConfiguration.Current.MerchantLocationId, 1, 3);

            Assert.IsNotNull(orders.Where(x => x.OrderIdentifier == created.OrderIdentifier).DefaultIfEmpty(null).FirstOrDefault());
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.IntegrationTests)]
        public void ListFilteredOrders_WithFilter()
        {
            IQueryOrders queryInterface = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IQueryOrders>();
            CompletedOrderResponse first = ClientModuleIntegrationTestingUtilities.PlaceOrderAtTestMerchantWithTestConsumer(200);
            ClientModuleIntegrationTestingUtilities.PlaceOrderAtTestMerchantWithTestConsumer(300);  // ensure that there is at least one other order that the filter will ignore.

            var orders = queryInterface.ListFilteredOrders(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                    LevelUpTestConfiguration.Current.MerchantLocationId, 1, 1, (x => x.OrderIdentifier == first.OrderIdentifier));

            Assert.AreEqual(orders.Count, 1);
            Assert.IsTrue(orders.First().OrderIdentifier == first.OrderIdentifier);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.IntegrationTests)]
        public void ListFilteredOrders_WithOrdering()
        {
            IQueryOrders queryInterface = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IQueryOrders>();
            ClientModuleIntegrationTestingUtilities.PlaceOrderAtTestMerchantWithTestConsumer(200);
            ClientModuleIntegrationTestingUtilities.PlaceOrderAtTestMerchantWithTestConsumer(300);

            var orders = queryInterface.ListFilteredOrders(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                    LevelUpTestConfiguration.Current.MerchantLocationId, 1, 1, null, ((x, y) => y.Total - x.Total));

            var copy = new List<OrderDetailsResponse>(orders);
            Assert.IsTrue(copy.OrderByDescending((x => x.Total)).SequenceEqual(orders));
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.IntegrationTests)]
        public void ListFilteredOrders_WithFilterAndOrdering()
        {
            IQueryOrders queryInterface = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IQueryOrders>();
            CompletedOrderResponse first = ClientModuleIntegrationTestingUtilities.PlaceOrderAtTestMerchantWithTestConsumer(200);
            CompletedOrderResponse second = ClientModuleIntegrationTestingUtilities.PlaceOrderAtTestMerchantWithTestConsumer(300);

            var orders = queryInterface.ListFilteredOrders(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken,
                   LevelUpTestConfiguration.Current.MerchantLocationId, 1, 1, 
                   (x => x.OrderIdentifier == first.OrderIdentifier || x.OrderIdentifier == second.OrderIdentifier), 
                   ((x, y) => y.Total - x.Total));
            Assert.AreEqual(orders.Count, 2);
            Assert.IsTrue(orders[0].OrderIdentifier == second.OrderIdentifier);
            Assert.IsTrue(orders[1].OrderIdentifier == first.OrderIdentifier);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.IntegrationTests)]
        public void ListOrdersWithPaging()
        {
            ClientModuleIntegrationTestingUtilities.RemoveAnyGiftCardCreditOnConsumerUserAccount();

            ClientModuleIntegrationTestingUtilities.PlaceOrderAtTestMerchantWithTestConsumer();
            var initialOrders = GetFirstThreePagesOfOrdersForTestMerchant();
            ClientModuleIntegrationTestingUtilities.PlaceOrderAtTestMerchantWithTestConsumer();
            var currentOrders = GetFirstThreePagesOfOrdersForTestMerchant();
            
            Assert.IsTrue(currentOrders.ElementAt(0).OrderIdentifier != initialOrders.ElementAt(0).OrderIdentifier);
            Assert.IsTrue(currentOrders.ElementAt(1).OrderIdentifier == initialOrders.ElementAt(0).OrderIdentifier);
        }

        private List<OrderDetailsResponse> GetFirstThreePagesOfOrdersForTestMerchant()
        {
            IQueryOrders queryInterface = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IQueryOrders>();

            var retval = new List<OrderDetailsResponse>();

            int currentPage = 1;
            bool areThereMorePages = true;

            while (areThereMorePages && currentPage < 4)
            {
                var orders = queryInterface.ListOrders(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpMerchantAccessToken, 
                    LevelUpTestConfiguration.Current.MerchantLocationId, currentPage, currentPage, out areThereMorePages);
                retval.AddRange(orders);
                currentPage++;
            }
            return retval;
        }
    }
}
