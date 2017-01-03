#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="ICreateOrdersUnitTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

extern alias ThirdParty;
using System.Net;
using LevelUp.Api.Client.ClientInterfaces;
using LevelUp.Api.Client.Models.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThirdParty.RestSharp;

namespace LevelUp.Api.Client.Test.Client
{
    [TestClass]
    public class ICreateOrdersUnitTests
    {
        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
        public void PlaceOrderShouldSucceed()
        {
            const string expectedRequestUrl = "https://sandbox.thelevelup.com/v15/orders";

            RestResponse expectedResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = "{\"order\":{\"uuid\":\"1a2b3c4d5e6f7g8h9i9h8g7f6e5d4c3b2a1\",\"spend_amount\":997,\"tip_amount\":0,\"total_amount\":997}}"
            };

            string expectedRequest = // This chuck of text was copied from the examples on the API docs.
                "{\"order\":{\"identifier_from_merchant\":\"001001\",\"location_id\":19,\"spend_amount\":997," +
                "\"cashier\":\"AndrewJones\",\"register\":\"03\",\"applied_discount_amount\":null," +
                "\"available_gift_card_amount\":null,\"exemption_amount\":null,\"partial_authorization_allowed\":false," +
                "\"payment_token_data\":\"LU02000008ZS9OJFUBNEL6ZM030000LU\"," +
                "\"receipt_message_html\":\"Pickupyourorderat<strong>counter#4</strong>\"," +
                "\"items\":[{\"item\":{\"charged_price_amount\":299,\"description\":\"Shreddedpotatoesgriddledtoperfection\"," +
                "\"name\":\"Hashbrowns\",\"quantity\":1,\"sku\":\"01abc192\",\"category\":\"BreakfastSides\"," +
                "\"standard_price_amount\":299,\"children\":[{\"item\":{\"charged_price_amount\":0," +
                "\"name\":\"SpecialInstructions\",\"description\":\"Scattered\"}}," +
                "{\"item\":{\"charged_price_amount\":50,\"name\":\"Onions\",\"description\":\"Smothered\"}}," +
                "{\"item\":{\"charged_price_amount\":100,\"name\":\"Cheese\",\"quantity\":2,\"description\":\"Covered\"}}," +
                "{\"item\":{\"charged_price_amount\":50,\"name\":\"Ham\",\"description\":\"Chunked\"}}]}}," +
                "{\"item\":{\"charged_price_amount\":398,\"description\":\"12ozCanofCoca-Cola\",\"name\":\"CanCoke\"," +
                "\"quantity\":2,\"sku\":\"291soo01\",\"category\":\"Drinks\",\"standard_price_amount\":398,\"upc\":\"04963406\"}}]}}";

            Order toPlace = Newtonsoft.Json.JsonConvert.DeserializeObject<Order>(expectedRequest);

            ICreateOrders client = ClientModuleUnitTestingUtilities.GetMockedLevelUpModule<ICreateOrders>(
                expectedResponse, expectedRequestUrl: expectedRequestUrl);
            var order = client.PlaceOrder("not_checking_this", toPlace);

            Assert.AreEqual(order.OrderIdentifier, "1a2b3c4d5e6f7g8h9i9h8g7f6e5d4c3b2a1");
            Assert.AreEqual(order.SpendAmount, 997);
            Assert.AreEqual(order.TipAmount, 0);
            Assert.AreEqual(order.Total, 997);
        }
    }
}
