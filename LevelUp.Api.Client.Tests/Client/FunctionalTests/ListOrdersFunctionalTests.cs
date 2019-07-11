#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="IQueryOrdersFunctionalTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using System.Net;
using LevelUp.Api.Client.ClientInterfaces;
using NUnit.Framework;
using RestSharp;

namespace LevelUp.Api.Client.Tests.Client.FunctionalTests
{
    [TestFixture]
    public class ListOrdersFunctionalTests
    {
        private const int locationId = 19;
        private string expectedRequestBaseUrl = string.Format(ClientModuleFunctionalTestingUtilities.SANDBOX_URL_PREFIX + "/v15/apps/orders");

        private const string orderUnitJson =
            "\"order\": {{" +
                  "\"uuid\": \"{0}\"," +
                  "\"created_at\": \"2014-01-01T00:00:00-04:00\"," +
                  "\"merchant_funded_credit_amount\": 5," +
                  "\"earn_amount\": 0," +
                  "\"loyalty_id\": 123," +
                  "\"refunded_at\": null," +
                  "\"refund_source\": null," +
                  "\"spend_amount\": 5," +
                  "\"tip_amount\": 0," +
                  "\"total_amount\": 5," +
                  "\"user_display_name\": \"Ryan T\"" +
            "}}";

        private RestResponse[] successfulResponses = 
        {
            GetOrderResponseWithThreeElements("a", "b", "c"),
            GetOrderResponseWithThreeElements("d", "e", "f"),
            GetOrderResponseWithThreeElements("g", "h", "i")
        };

        private static RestResponse GetOrderResponseWithThreeElements(string id1, string id2, string id3)
        {
            return new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = "[{" + string.Format(orderUnitJson, id1) + "},{" + string.Format(orderUnitJson, id2) + "},{" + string.Format(orderUnitJson, id3) + "}]"
            };
        }

        [Test]
        public void ListOrdersShouldSucceedWhenAllPagesQueried()
        {
            IListOrders client = ClientModuleFunctionalTestingUtilities.GetMockedLevelUpModuleWithPaging<IListOrders>(successfulResponses, expectedRequestBaseUrl);
            var orders = client.ListOrders("not_checking_this", 1, 3);

            Assert.AreEqual(orders.Count, 9);
            Assert.AreEqual(orders[0].OrderIdentifier, "a");
            Assert.AreEqual(orders[1].OrderIdentifier, "b");
            Assert.AreEqual(orders[2].OrderIdentifier, "c");
            Assert.AreEqual(orders[3].OrderIdentifier, "d");
            Assert.AreEqual(orders[4].OrderIdentifier, "e");
            Assert.AreEqual(orders[5].OrderIdentifier, "f");
            Assert.AreEqual(orders[6].OrderIdentifier, "g");
            Assert.AreEqual(orders[7].OrderIdentifier, "h");
            Assert.AreEqual(orders[8].OrderIdentifier, "i");
        }

        [Test]
        public void ListOrdersShouldSucceedWhenMorePagesQueriedThanExist()
        {
            IListOrders client = ClientModuleFunctionalTestingUtilities.GetMockedLevelUpModuleWithPaging<IListOrders>(successfulResponses, expectedRequestBaseUrl);
            var orders = client.ListOrders("not_checking_this", 1, 10);     // Note that we specify pages 1-10 when only 3 pages exist

            Assert.AreEqual(orders.Count, 9);
        }

        [Test]
        public void ListOrdersShouldSucceedAndAreThereMorePagesShouldBeValid()
        {
            IListOrders client = ClientModuleFunctionalTestingUtilities.GetMockedLevelUpModuleWithPaging<IListOrders>(successfulResponses, expectedRequestBaseUrl);
            bool areThereMorePages;
            
            var orders = client.ListOrders("not_checking_this", 1, 1, out areThereMorePages);
            Assert.IsTrue(areThereMorePages);

            orders = client.ListOrders("not_checking_this", 1, 3, out areThereMorePages);
            Assert.IsFalse(areThereMorePages);

            orders = client.ListOrders("not_checking_this", 3, 3, out areThereMorePages);
            Assert.IsFalse(areThereMorePages);

            orders = client.ListOrders("not_checking_this", 2, 5, out areThereMorePages);
            Assert.IsFalse(areThereMorePages);
        }

        [Test]
        public void ListOrdersShouldSucceedDespiteInvalidEndPageNumbers()
        {
            IListOrders client = ClientModuleFunctionalTestingUtilities.GetMockedLevelUpModuleWithPaging<IListOrders>(successfulResponses, expectedRequestBaseUrl);
            
            var orders = client.ListOrders("not_checking_this", 1, 10);
            Assert.AreEqual(orders.Count, 9);

            orders = client.ListOrders("not_checking_this", 3, 11);
            Assert.AreEqual(orders.Count, 3);
        }

        [Test]
        public void ListOrdersShouldFailWith204ForInvalidStartPageNumbers()
        {
            IListOrders client = ClientModuleFunctionalTestingUtilities.GetMockedLevelUpModuleWithPaging<IListOrders>(
                successfulResponses, expectedRequestBaseUrl);
            var orders = client.ListOrders("not_checking_this", 5, 10);
            Assert.AreEqual(orders.Count, 0);
        }

        [Test]
        public void ListFilteredOrdersShouldSucceedWithNoModifierFunctions()
        {
            IListOrders client = ClientModuleFunctionalTestingUtilities.GetMockedLevelUpModuleWithPaging<IListOrders>(
                successfulResponses, expectedRequestBaseUrl);
            var orders = client.ListFilteredOrders("not_checking_this", 1, 3);
            Assert.AreEqual(orders.Count, 9);
        }
        
        [Test]
        public void ListFilteredOrdersShouldSucceedWithAFilterFunction()
        {
            IListOrders client = ClientModuleFunctionalTestingUtilities.GetMockedLevelUpModuleWithPaging<IListOrders>(
                successfulResponses, expectedRequestBaseUrl);
            var orders = client.ListFilteredOrders("not_checking_this", 1, 3, 
                (x => (x.OrderIdentifier == "c" || x.OrderIdentifier == "i")), null);
            Assert.AreEqual(orders.Count, 2);
            Assert.AreEqual(orders[0].OrderIdentifier, "c");
            Assert.AreEqual(orders[1].OrderIdentifier, "i");
        }

        [Test]
        public void ListFilteredOrdersShouldSucceedWithAnOrderingFunction()
        {
            IListOrders client = ClientModuleFunctionalTestingUtilities.GetMockedLevelUpModuleWithPaging<IListOrders>(
                successfulResponses, expectedRequestBaseUrl);
            var orders = client.ListFilteredOrders("not_checking_this", 1, 3, null,
                ((x, y) => ( 0 - string.Compare(x.OrderIdentifier, y.OrderIdentifier) ))); // i.e. orderbydecending
            Assert.AreEqual(orders.Count, 9);
            Assert.AreEqual(orders[0].OrderIdentifier, "i");
            Assert.AreEqual(orders[8].OrderIdentifier, "a");
        }

    }
}
