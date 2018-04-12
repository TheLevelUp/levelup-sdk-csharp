#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="IManageProposedOrdersFunctionalTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using System.Net;
using LevelUp.Api.Client.ClientInterfaces;
using LevelUp.Api.Client.Models.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;

namespace LevelUp.Api.Client.Test.Client.FunctionalTests
{
    [TestClass]
    public class IManageProposedOrdersFunctionalTests
    {
        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.FunctionalTests)]
        public void CreateProposedOrderShouldSucceed()
        {
            string expectedRequestUrl = "https://sandbox.thelevelup.com/v15/proposed_orders";

            string expectedRequest = "{" +
                                      "\"proposed_order\": {" +
                                        "\"payment_token_data\": \"LU02000008ZS9OJFUBNEL6ZM030000LU\"," +
                                        "\"cashier\": \"Bob\"," +
                                        "\"exemption_amount\": 0," +
                                        "\"receipt_message_html\": null," +
                                        "\"register\": \"3\"," +
                                        "\"tax_amount\": 10," +
                                        "\"identifier_from_merchant\": \"001001\"," +
                                        "\"partial_authorization_allowed\": true," +
                                        "\"location_id\": 19," +
                                        "\"spend_amount\": 110," +
                                        GetSampleItems().Item1 +
                                      "}" +
                                    "}";

            RestResponse expectedResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = "{ \"proposed_order\": { \"discount_amount\": 2578, \"uuid\": \"1b3b3c4d5e6f7g8a9i9h8g7f6e5d4c3b2a1\"}}"
            };

            var items = GetSampleItems().Item2;
            
            IManageProposedOrders client = ClientModuleFunctionalTestingUtilities.GetMockedLevelUpModule<IManageProposedOrders, CreateProposedOrderRequest>(
                expectedResponse, expectedRequest, expectedRequestUrl: expectedRequestUrl);
            var proposedOrder = client.CreateProposedOrder("not_checking_this", 19, "LU02000008ZS9OJFUBNEL6ZM030000LU",
                110, 110, 10, 0, "3", "Bob", "001001", null, true, items);

            Assert.AreEqual(proposedOrder.ProposedOrderIdentifier, "1b3b3c4d5e6f7g8a9i9h8g7f6e5d4c3b2a1");
            Assert.AreEqual(proposedOrder.DiscountAmountCents, 2578);

        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.FunctionalTests)]
        public void CreateProposedOrderShouldSucceedForSplitTender()
        {
            string expectedRequestUrl = "https://sandbox.thelevelup.com/v15/proposed_orders";

            string expectedRequest = "{" +
                                     "\"proposed_order\": {" +
                                     "\"payment_token_data\": \"LU02000008ZS9OJFUBNEL6ZM030000LU\"," +
                                     "\"cashier\": \"Bob\"," +
                                     "\"exemption_amount\": 40," +
                                     "\"receipt_message_html\": null," +
                                     "\"register\": \"3\"," +
                                     "\"tax_amount\": 0," +
                                     "\"identifier_from_merchant\": \"001001\"," +
                                     "\"partial_authorization_allowed\": true," +
                                     "\"location_id\": 19," +
                                     "\"spend_amount\": 100," +
                                     GetSampleItems().Item1 +
                                     "}" +
                                     "}";

            RestResponse expectedResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = "{ \"proposed_order\": { \"discount_amount\": 2578, \"uuid\": \"1b3b3c4d5e6f7g8a9i9h8g7f6e5d4c3b2a1\"}}"
            };

            var items = GetSampleItems().Item2;

            IManageProposedOrders client = ClientModuleFunctionalTestingUtilities.GetMockedLevelUpModule<IManageProposedOrders, CreateProposedOrderRequest>(
                expectedResponse, expectedRequest, expectedRequestUrl: expectedRequestUrl);
            var proposedOrder = client.CreateProposedOrder("not_checking_this", 19, "LU02000008ZS9OJFUBNEL6ZM030000LU",
                200, 100, 10, 130, "3", "Bob", "001001", null, true, items);

            Assert.AreEqual(proposedOrder.ProposedOrderIdentifier, "1b3b3c4d5e6f7g8a9i9h8g7f6e5d4c3b2a1");
            Assert.AreEqual(proposedOrder.DiscountAmountCents, 2578);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.FunctionalTests)]
        public void CompleteProposedOrderShouldSucceed()
        {
            string expectedRequestUrl = "https://sandbox.thelevelup.com/v15/completed_orders";

            string expectedRequest = "{" +
                                      "\"completed_order\": {" +
                                        "\"applied_discount_amount\": 100," +
                                        "\"cashier\": \"Bob\"," +
                                        "\"exemption_amount\": 0," +
                                        "\"identifier_from_merchant\": \"001001\"," +
                                        "\"location_id\": 19," +
                                        "\"partial_authorization_allowed\": false," +
                                        "\"payment_token_data\": \"LU02000008ZS9OJFUBNEL6ZM030000LU\"," +
                                        "\"proposed_order_uuid\": \"1b3b3c4d5e6f7g8a9i9h8g7f6e5d4c3b2a1\"," +
                                        "\"receipt_message_html\": \"Pick up your order at <strong>counter #4</strong>\"," +
                                        "\"register\": \"3\"," +
                                        "\"spend_amount\": 110," +
                                        "\"tax_amount\": 0," +
                                        GetSampleItems().Item1 +
                                      "}" +
                                    "}";

            RestResponse expectedResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = "{ " +
                              "\"order\": { " +
                                  "\"gift_card_total_amount\": 0, " +
                                  "\"gift_card_tip_amount\": 0, " +
                                  "\"spend_amount\": 100, " +
                                  "\"tip_amount\": 0, " +
                                  "\"total_amount\": 100, " +
                                  "\"uuid\": \"5a1z9x2h31ah7g8a9i9h8g7f6e5d4c4a21o\"" +
                              "}" +
                          "}"
            };

            var items = GetSampleItems().Item2;

            IManageProposedOrders client = ClientModuleFunctionalTestingUtilities.GetMockedLevelUpModule<IManageProposedOrders, CompleteProposedOrderRequest>(
                expectedResponse, expectedRequest, expectedRequestUrl: expectedRequestUrl);
            var completedOrder = client.CompleteProposedOrder("not_checking_this", 19, "LU02000008ZS9OJFUBNEL6ZM030000LU",
                "1b3b3c4d5e6f7g8a9i9h8g7f6e5d4c3b2a1", 110, 110, 10, 0, 100, "3", "Bob", "001001", 
                "Pick up your order at <strong>counter #4</strong>", false, items);

            Assert.AreEqual(completedOrder.GiftCardTotalAmount, 0);
            Assert.AreEqual(completedOrder.GiftCardTipAmount, 0);
            Assert.AreEqual(completedOrder.SpendAmount, 100);
            Assert.AreEqual(completedOrder.TipAmount, 0);
            Assert.AreEqual(completedOrder.Total, 100);
            Assert.AreEqual(completedOrder.OrderIdentifier, "5a1z9x2h31ah7g8a9i9h8g7f6e5d4c4a21o");
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.FunctionalTests)]
        public void CompleteProposedOrderShouldSucceedForSplitTender()
        {
            string expectedRequestUrl = "https://sandbox.thelevelup.com/v15/completed_orders";

            string expectedRequest = "{" +
                                     "\"completed_order\": {" +
                                     "\"applied_discount_amount\": 100," +
                                     "\"cashier\": \"Bob\"," +
                                     "\"exemption_amount\": 0," +
                                     "\"identifier_from_merchant\": \"001001\"," +
                                     "\"location_id\": 19," +
                                     "\"partial_authorization_allowed\": false," +
                                     "\"payment_token_data\": \"LU02000008ZS9OJFUBNEL6ZM030000LU\"," +
                                     "\"proposed_order_uuid\": \"1b3b3c4d5e6f7g8a9i9h8g7f6e5d4c3b2a1\"," +
                                     "\"receipt_message_html\": \"Pick up your order at <strong>counter #4</strong>\"," +
                                     "\"register\": \"3\"," +
                                     "\"spend_amount\": 200," +
                                     "\"tax_amount\": 0," +
                                     GetSampleItems().Item1 +
                                     "}" +
                                     "}";

            RestResponse expectedResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = "{ " +
                          "\"order\": { " +
                          "\"gift_card_total_amount\": 0, " +
                          "\"gift_card_tip_amount\": 0, " +
                          "\"spend_amount\": 200, " +
                          "\"tip_amount\": 0, " +
                          "\"total_amount\": 200, " +
                          "\"uuid\": \"5a1z9x2h31ah7g8a9i9h8g7f6e5d4c4a21o\"" +
                          "}" +
                          "}"
            };

            var items = GetSampleItems().Item2;

            IManageProposedOrders client = ClientModuleFunctionalTestingUtilities.GetMockedLevelUpModule<IManageProposedOrders, CompleteProposedOrderRequest>(
                expectedResponse, expectedRequest, expectedRequestUrl: expectedRequestUrl);
            var completedOrder = client.CompleteProposedOrder("not_checking_this", 19, "LU02000008ZS9OJFUBNEL6ZM030000LU",
                "1b3b3c4d5e6f7g8a9i9h8g7f6e5d4c3b2a1", 300, 200, 10, 130, 100, "3", "Bob", "001001",
                "Pick up your order at <strong>counter #4</strong>", false, items);

            Assert.AreEqual(completedOrder.GiftCardTotalAmount, 0);
            Assert.AreEqual(completedOrder.GiftCardTipAmount, 0);
            Assert.AreEqual(completedOrder.SpendAmount, 200);
            Assert.AreEqual(completedOrder.TipAmount, 0);
            Assert.AreEqual(completedOrder.Total, 200);
            Assert.AreEqual(completedOrder.OrderIdentifier, "5a1z9x2h31ah7g8a9i9h8g7f6e5d4c4a21o");
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.FunctionalTests)]
        public void CompleteProposedOrderShouldSucceedForDiscountsDisabled()
        {
            string expectedRequestUrl = "https://sandbox.thelevelup.com/v15/completed_orders";

            string expectedRequest = "{" +
                                     "\"completed_order\": {" +
                                     "\"applied_discount_amount\": null," +
                                     "\"cashier\": \"Bob\"," +
                                     "\"exemption_amount\": 40," +
                                     "\"identifier_from_merchant\": \"001001\"," +
                                     "\"location_id\": 19," +
                                     "\"partial_authorization_allowed\": false," +
                                     "\"payment_token_data\": \"LU02000008ZS9OJFUBNEL6ZM030000LU\"," +
                                     "\"proposed_order_uuid\": \"1b3b3c4d5e6f7g8a9i9h8g7f6e5d4c3b2a1\"," +
                                     "\"receipt_message_html\": \"Pick up your order at <strong>counter #4</strong>\"," +
                                     "\"register\": \"3\"," +
                                     "\"spend_amount\": 200," +
                                     "\"tax_amount\": 0," +
                                     GetSampleItems().Item1 +
                                     "}" +
                                     "}";

            RestResponse expectedResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = "{ " +
                          "\"order\": { " +
                          "\"gift_card_total_amount\": 0, " +
                          "\"gift_card_tip_amount\": 0, " +
                          "\"spend_amount\": 200, " +
                          "\"tip_amount\": 0, " +
                          "\"total_amount\": 200, " +
                          "\"uuid\": \"5a1z9x2h31ah7g8a9i9h8g7f6e5d4c4a21o\"" +
                          "}" +
                          "}"
            };

            var items = GetSampleItems().Item2;

            IManageProposedOrders client = ClientModuleFunctionalTestingUtilities.GetMockedLevelUpModule<IManageProposedOrders, CompleteProposedOrderRequest>(
                expectedResponse, expectedRequest, expectedRequestUrl: expectedRequestUrl);
            var completedOrder = client.CompleteProposedOrder("not_checking_this", 19, "LU02000008ZS9OJFUBNEL6ZM030000LU",
                "1b3b3c4d5e6f7g8a9i9h8g7f6e5d4c3b2a1", 300, 200, 10, 130, null, "3", "Bob", "001001",
                "Pick up your order at <strong>counter #4</strong>", false, items);

            Assert.AreEqual(completedOrder.GiftCardTotalAmount, 0);
            Assert.AreEqual(completedOrder.GiftCardTipAmount, 0);
            Assert.AreEqual(completedOrder.SpendAmount, 200);
            Assert.AreEqual(completedOrder.TipAmount, 0);
            Assert.AreEqual(completedOrder.Total, 200);
            Assert.AreEqual(completedOrder.OrderIdentifier, "5a1z9x2h31ah7g8a9i9h8g7f6e5d4c4a21o");
        }

        private System.Tuple<string, List<Item>> GetSampleItems()
        {
            string json = "\"items\": [" +
                            "{" +
                            "\"item\": {" +
                                "\"charged_price_amount\": 299," +
                                "\"description\": \"Shredded potatoes griddled to perfection\"," +
                                "\"name\": \"Hashbrowns\"," +
                                "\"quantity\": 1," +
                                "\"sku\": \"01abc192\"," +
                                "\"category\": \"Breakfast Sides\"," +
                                "\"standard_price_amount\": 299," +
                                "\"children\": [" +
                                "{" +
                                    "\"item\": {" +
                                    "\"charged_price_amount\": 0," +
                                    "\"name\": \"Special Instructions\"," +
                                    "\"quantity\": 1," +
                                    "\"description\": \"Scattered\"" +
                                    "}" +
                                "}," +
                                "{" +
                                    "\"item\": {" +
                                    "\"charged_price_amount\": 50," +
                                    "\"name\": \"Onions\"," +
                                    "\"quantity\": 1," +
                                    "\"description\": \"Smothered\"" +
                                    "}" +
                                "}," +
                                "{" +
                                    "\"item\": {" +
                                    "\"charged_price_amount\": 100," +
                                    "\"name\": \"Cheese\"," +
                                    "\"quantity\": 2," +
                                    "\"description\": \"Covered\"" +
                                    "}" +
                                "}," +
                                "{" +
                                    "\"item\": {" +
                                    "\"charged_price_amount\": 50," +
                                    "\"name\": \"Ham\"," +
                                    "\"quantity\": 1," +
                                    "\"description\": \"Chunked\"" +
                                    "}" +
                                "}" +
                                "]" +
                            "}" +
                            "}," +
                            "{" +
                            "\"item\": {" +
                                "\"charged_price_amount\": 398," +
                                "\"description\": \"12oz Can of Coca-Cola\"," +
                                "\"name\": \"Can Coke\"," +
                                "\"quantity\": 2," +
                                "\"sku\": \"291soo01\"," +
                                "\"category\": \"Drinks\"," +
                                "\"standard_price_amount\": 199," +
                                "\"upc\": \"04963406\"" +
                            "}" +
                            "}" +
                        "]";

            var items = new List<Item>(new Item[]
            {
                new Item("Hashbrowns", "Shredded potatoes griddled to perfection", "01abc192", null, "Breakfast Sides", 299, 299, 1, 
                    new List<Item>(new Item[]
                    {
                        new Item("Special Instructions", "Scattered", null, null, null, 0, null),
                        new Item("Onions", "Smothered", null, null, null, 50, null),
                        new Item("Cheese", "Covered", null, null, null, 100, null, 2),
                        new Item("Ham", "Chunked", null, null, null, 50, null)
                    })
                ),
                new Item("Can Coke", "12oz Can of Coca-Cola", "291soo01", "04963406", "Drinks", 398, 199, 2, null)
            });

            return new System.Tuple<string, List<Item>>(json, items);
        }
    }
}
