#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="CreateAndUpdateRemoteCheckDataRequestTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using FluentAssertions;
using LevelUp.Api.Client.Models.Requests;
using NUnit.Framework;
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Tests.Models.Requests
{
    [TestFixture]
    public class CreateAndUpdateRemoteCheckDataRequestTests : ModelTestsBase
    {
        private const int LOCATION_ID = 3795;

        private static readonly List<Item> ITEMS = new List<Item>
            {
                new Item("Beets by Dre", "Fresh Beets", "R00tz", null, "T00berz", 453, 500, 2, null),
                new Item("Quinoa", "Not so great grains", "UNKNOWN", null, "health foods", 3, 2, 10000, null),
                new Item("Slammin' Salmon", "Festive fish", "NONE", "34254", "Food of the sea", 5443, 3323, 1, new List<Item>
                    {
                        new Item("Slammin' Marinade", "Secret sauce", null, null, "Add-ons", 321, 333, 1, null),
                        new Item("Herbs and Spices", "Zesty flavors for your fish", null, null, "Add-ons", 211, 222, 5, null),
                    }),
            };

        private const string ITEMS_STRING =
            "[{\"item\":{\"charged_price_amount\":453,\"children\":null,\"description\":" +
            "\"Fresh Beets\",\"name\":\"Beets by Dre\",\"quantity\":2,\"sku\":\"R00tz\",\"category\":" +
            "\"T00berz\",\"standard_price_amount\":500,\"upc\":null}},{\"item\":" +
            "{\"charged_price_amount\":3,\"children\":null,\"description\":\"Not so great grains\"," +
            "\"name\":\"Quinoa\",\"quantity\":10000,\"sku\":\"UNKNOWN\",\"category\":\"health foods\"," +
            "\"standard_price_amount\":2,\"upc\":null}},{\"item\":{\"charged_price_amount\":5443," +
            "\"children\":[{\"item\":{\"charged_price_amount\":321,\"children\":null,\"description\":" +
            "\"Secret sauce\",\"name\":\"Slammin' Marinade\",\"quantity\":1,\"sku\":null,\"category\":" +
            "\"Add-ons\",\"standard_price_amount\":333,\"upc\":null}},{\"item\":{" +
            "\"charged_price_amount\":211,\"children\":null,\"description\":" +
            "\"Zesty flavors for your fish\",\"name\":\"Herbs and Spices\",\"quantity\":5,\"sku\":" +
            "null,\"category\":\"Add-ons\",\"standard_price_amount\":222,\"upc\":null}}]," +
            "\"description\":\"Festive fish\",\"name\":\"Slammin' Salmon\",\"quantity\":1,\"sku\":" +
            "\"NONE\",\"category\":\"Food of the sea\",\"standard_price_amount\":3323," +
            "\"upc\":\"34254\"}}]";

        [Test]
        
        public void Serialize()
        {
            const int spendAmount = 4567;
            const int taxAmount = 25;
            const int exemptionAmount = 213;
            const string register = "Register X";
            const string cashier = "Ca$hier";
            const string identifier = "!dent!f!er";

            string expected = CreateExpectedSerializedString(LOCATION_ID,
                                                             spendAmount,
                                                             taxAmount,
                                                             exemptionAmount,
                                                             cashier,
                                                             register,
                                                             identifier,
                                                             true,
                                                             ITEMS_STRING);

            RemoteCheckDataRequestBody request = new RemoteCheckDataRequestBody(LOCATION_ID,
                                                                                    spendAmount,
                                                                                    taxAmount,
                                                                                    exemptionAmount,
                                                                                    identifier,
                                                                                    register,
                                                                                    cashier,
                                                                                    true,
                                                                                    ITEMS);

            string serialized = JsonConvert.SerializeObject(request);

            serialized.Should().NotBeNullOrEmpty();
            TestUtilities.VerifyJsonIsEquivalent<RemoteCheckDataRequestBody>(serialized, expected);
        }

        [Test]
        
        public void Serialize_Defaults()
        {
            const int spendAmount = 2132;
            const int taxAmount = 59;
            const int exemptionAmount = 3;

            string expected = CreateExpectedSerializedString(LOCATION_ID, spendAmount, taxAmount, exemptionAmount);

            RemoteCheckDataRequestBody request = new RemoteCheckDataRequestBody(LOCATION_ID,
                                                                                    spendAmount,
                                                                                    taxAmount,
                                                                                    exemptionAmount);

            string serialized = JsonConvert.SerializeObject(request);

            serialized.Should().NotBeNullOrEmpty();
            TestUtilities.VerifyJsonIsEquivalent<RemoteCheckDataRequestBody>(serialized, expected);
        }

        private string CreateExpectedSerializedString(int locationId,
                                                      int spendAmount,
                                                      int taxAmount,
                                                      int exemptionAmount,
                                                      string cashier = null,
                                                      string register = null,
                                                      string identifier = null,
                                                      bool partialAuth = true,
                                                      string itemsString = "null")
        {
            return "{\"check\":{" +
                   "\"cashier\":" + FormatJsonString(cashier) +
                   ",\"exemption_amount\":" + exemptionAmount +
                   ",\"location_id\":" + locationId +
                   ",\"partial_authorization_allowed\":" + (partialAuth ? "true" : "false") +
                   ",\"register\":" + FormatJsonString(register) +
                   ",\"spend_amount\":" + spendAmount +
                   ",\"tax_amount\":" + taxAmount +
                   ",\"identifier_from_merchant\":" + FormatJsonString(identifier) +
                   ",\"items\":" + itemsString + "}}";
        }
    }
}
