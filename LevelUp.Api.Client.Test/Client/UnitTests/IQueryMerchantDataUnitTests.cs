#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="IQueryMerchantDataUnitTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThirdParty.RestSharp;

namespace LevelUp.Api.Client.Test.Client
{
    [TestClass]
    public class IQueryMerchantDataUnitTests
    {
        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
        public void GetLocationDetailsShouldSucceed()
        {
            const int locationId = 17;

            string expectedRequestUrl = "https://sandbox.thelevelup.com/v15/locations/" + locationId;

            RestResponse expectedResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = string.Format("{{" +
                                            "\"location\": {{" +
                                                "\"categories\": [" +
                                                    "50" +
                                                "]," +
                                                "\"extended_address\": \"\"," +
                                                "\"facebook_url\": \"http://www.facebook.com/pages/PizzaPalace\"," +
                                                "\"foodler_url\": \"http://deeplink.me/www.foodler.com/pizza-palace/1234\"," +
                                                "\"hours\": null," +
                                                "\"id\": {0}," +
                                                "\"latitude\": 42.351639," +
                                                "\"locality\": \"Boston\"," +
                                                "\"longitude\": -71.121797," +
                                                "\"menu_url\": null," +
                                                "\"merchant_id\": 18," +
                                                "\"merchant_description_html\": \"This is a place that has pizza!\"," +
                                                "\"merchant_name\": \"Pizza Palace\"," +
                                                "\"merchant_tip_preference\": \"no preference\"," +
                                                "\"name\": null," +
                                                "\"newsletter_url\": null," +
                                                "\"opentable_url\": null," +
                                                "\"phone\": null," +
                                                "\"postal_code\": \"02215\"," +
                                                "\"region\": \"MA\"," +
                                                "\"street_address\": \"1024 Pizza Road\"," +
                                                "\"twitter_url\": null," +
                                                "\"updated_at\": \"2014-11-30T10:28:23-05:00\"," +
                                                "\"yelp_url\": \"http://www.yelp.com/biz/pizza-palace\"," +
                                                "\"shown\": true" +
                                            "}}" +
                                        "}}", locationId)
            };

            IQueryMerchantData client = ClientModuleUnitTestingUtilities.GetMockedLevelUpModule<IQueryMerchantData>(
                expectedResponse, expectedRequestUrl: expectedRequestUrl);
            var details = client.GetLocationDetails("not_checking_this", locationId);

            Assert.AreEqual(details.LocationId, locationId);
            Assert.AreEqual(details.Address.PostalCode, "02215");
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
        public void GetMerchantOrderDetailsShouldSucceed()
        {
            const int merchantId = 34;
            const string uuid = "123b75b0806011e29e960800200c9a66";
            string expectedRequestUrl = string.Format("https://sandbox.thelevelup.com/v15/merchants/{0}/orders/{1}", merchantId, uuid);

            RestResponse expectedResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = string.Format("{{" +
                                            "\"order\": {{" +
                                                "\"cashier\": \"Andrew Jones\"," +
                                                "\"created_at\": \"2014-12-17T11:24:26-05:00\"," +
                                                "\"identifier_from_merchant\": \"1001\"," +
                                                "\"location_id\": 5698," +
                                                "\"loyalty_id\": 633172," +
                                                "\"refunded_at\": null," +
                                                "\"register\": \"3\"," +
                                                "\"transacted_at\": \"2014-12-17T11:24:26-05:00\"," +
                                                "\"user_display_name\": \"Test U.\"," +
                                                "\"uuid\": \"123b75b0806011e29e960800200c9a66\"," +
                                                "\"items\": [" +
                                                    "{{" +
                                                        "\"item\": {{" +
                                                            "\"description\": \"Leftovers featuring pickled cabbage\"," +
                                                            "\"name\": \"BiBimBap\"," +
                                                            "\"quantity\": 2," +
                                                            "\"sku\": \"1111\"," +
                                                            "\"category\": \"Korean\"," +
                                                            "\"upc\": \"9999\"," +
                                                            "\"children\": []," +
                                                            "\"charged_price_amount\": 200," +
                                                            "\"standard_price_amount\": 200" +
                                                        "}}" +
                                                    "}}," +
                                                    "{{" +
                                                        "\"item\": {{" +
                                                            "\"description\": \"Lovely sprockets with gravy\"," +
                                                            "\"name\": \"Sprockets\"," +
                                                            "\"quantity\": 7," +
                                                            "\"sku\": \"1234\"," +
                                                            "\"category\": \"Weird stuff\"," +
                                                            "\"upc\": \"4321\"," +
                                                            "\"children\": []," +
                                                            "\"charged_price_amount\": 100," +
                                                            "\"standard_price_amount\": 100" +
                                                        "}}" +
                                                    "}}" +
                                                "]," +
                                                "\"earn_amount\": 0," +
                                                "\"merchant_funded_credit_amount\": 12," +
                                                "\"spend_amount\": 10," +
                                                "\"tip_amount\": 2," +
                                                "\"total_amount\": 12" +
                                            "}}" +
                                        "}}")
            };

            IQueryMerchantData client = ClientModuleUnitTestingUtilities.GetMockedLevelUpModule<IQueryMerchantData>(
                expectedResponse, expectedRequestUrl: expectedRequestUrl);
            var details = client.GetMerchantOrderDetails("not_checking_this", merchantId, uuid);

            Assert.AreEqual(details.LoyaltyId, 633172);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
        public void ListLocationsShouldSucceed()
        {
            const int merchantId = 34;
            string expectedRequestUrl = string.Format("https://sandbox.thelevelup.com/v15/merchants/{0}/locations", merchantId);

            RestResponse expectedResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = 
                    "[{" +
                        "\"location\": {" +
                            "\"extended_address\": \"\"," +
                            "\"facebook_url\": null," +
                            "\"foodler_url\": null," +
                            "\"hours\": \"\"," +
                            "\"id\": 19," +
                            "\"latitude\": 42.351231," +
                            "\"locality\": \"Boston\"," +
                            "\"longitude\": -71.077396," +
                            "\"menu_url\": null," +
                            "\"merchant_id\": 34," +
                            "\"merchant_description_html\": \"SampleMerchant\"," +
                            "\"merchant_name\": \"SampleMerchant\"," +
                            "\"merchant_tip_preference\": \"no preference\"," +
                            "\"name\": null," +
                            "\"newsletter_url\": null," +
                            "\"opentable_url\": null," +
                            "\"phone\": \"\"," +
                            "\"postal_code\": \"02114\"," +
                            "\"region\": \"Massachusetts\"," +
                            "\"street_address\": \"1234 Test Street\"," +
                            "\"twitter_url\": null," +
                            "\"updated_at\": \"2015-01-22T14:26:19-05:00\"," +
                            "\"yelp_url\": null," +
                            "\"shown\": true" +
                        "}" +
                    "}," +
                    "{" +
                        "\"location\": {" +
                            "\"extended_address\": \"\"," +
                            "\"facebook_url\": null," +
                            "\"foodler_url\": null," +
                            "\"hours\": \"\"," +
                            "\"id\": 19," +
                            "\"latitude\": 42.351231," +
                            "\"locality\": \"Boston\"," +
                            "\"longitude\": -71.077396," +
                            "\"menu_url\": null," +
                            "\"merchant_id\": 34," +
                            "\"merchant_description_html\": \"SampleMerchant\"," +
                            "\"merchant_name\": \"SampleMerchant\"," +
                            "\"merchant_tip_preference\": \"no preference\"," +
                            "\"name\": null," +
                            "\"newsletter_url\": null," +
                            "\"opentable_url\": null," +
                            "\"phone\": \"\"," +
                            "\"postal_code\": \"02114\"," +
                            "\"region\": \"Massachusetts\"," +
                            "\"street_address\": \"1234 Test Street\"," +
                            "\"twitter_url\": null," +
                            "\"updated_at\": \"2015-01-22T14:26:19-05:00\"," +
                            "\"yelp_url\": null," +
                            "\"shown\": true" +
                        "}" +
                    "}]"
            };

            IQueryMerchantData client = ClientModuleUnitTestingUtilities.GetMockedLevelUpModule<IQueryMerchantData>(
                expectedResponse, expectedRequestUrl: expectedRequestUrl);
            var locations = client.ListLocations("not_checking_this", merchantId);

            Assert.IsTrue(locations.Count == 2);
            Assert.IsTrue(locations[0].Name == null);
            Assert.IsTrue(locations[0].TipPreference == "no preference");
            Assert.IsTrue(locations[0].LocationId == 19);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
        public void ListManagedLocationsShouldSucceed()
        {
            string expectedRequestUrl = "https://sandbox.thelevelup.com/v15/managed_locations";

            RestResponse expectedResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content =
                    "[{" +
                        "\"location\": {" +
                            "\"address\": \"101 Arch St., Boston, MA\"," +
                            "\"id\": 3," +
                            "\"merchant_id\": 1," + 
                            "\"merchant_name\": \"LevelUp Cafe\"," +
                            "\"name\": \"LU#3 - 101 Arch St.\"," +
                            "\"tip_preference\": \"expected\"" +
                        "}" + 
                    "}," +
                    "{" +
                        "\"location\": {" +
                            "\"address\": \"1 Beach Dr, Honolulu, HI\"," +
                            "\"id\": 10," +
                            "\"merchant_id\": 1001," +
                            "\"merchant_name\": \"Hang Ten Cafe\"," +
                            "\"name\": \"LU#10 - 1 Beach Dr.\"," +
                            "\"tip_preference\": \"unwanted\"" +
                        "}" +
                    "}]"
            };

            string expectedAccessToken = "token merchant=\"my_access_token\"";

            IQueryMerchantData client = ClientModuleUnitTestingUtilities.GetMockedLevelUpModule<IQueryMerchantData>(
                expectedResponse, expectedRequestUrl: expectedRequestUrl, expectedAccessToken: expectedAccessToken);
            var locations = client.ListManagedLocations("my_access_token");

            Assert.IsTrue(locations.Count == 2);
            Assert.IsTrue(locations[0].LocationId == 3);
            Assert.IsTrue(locations[0].TipPreference == "expected");
        }
    }
}
