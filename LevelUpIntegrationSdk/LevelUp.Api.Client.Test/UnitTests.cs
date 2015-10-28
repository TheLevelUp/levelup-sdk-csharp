//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="UnitTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
//   Copyright(c) 2015 SCVNGR, Inc. d/b/a LevelUp. All rights reserved.
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

using LevelUp.Api.Client.Models.Responses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using LevelUp.Api.Http;

namespace LevelUp.Api.Client.Test
{
    [TestClass]
    [DeploymentItem("LevelUpBaseUri.txt")]
    [DeploymentItem("test_config_settings.xml")]
    public class ApiUnitTests
    {
        private ILevelUpClient _api;
        private AccessToken _accessToken;

        public AccessToken AccessToken
        {
            get
            {
                return _accessToken ?? (_accessToken = Api.Authenticate(LevelUpTestConfiguration.Current.ApiKey,
                                                                        LevelUpTestConfiguration.Current.Username,
                                                                        LevelUpTestConfiguration.Current.Password));
            }
        }

        public ILevelUpClient Api
        {
            get
            {
                return _api ?? (_api = LevelUpClientFactory.Create(TestData.Valid.COMPANY_NAME,
                                                                   TestData.Valid.PRODUCT_NAME,
                                                                   TestData.Valid.PRODUCT_VERSION,
                                                                   TestData.Valid.OS_NAME,
                                                                   TestConstants.BASE_URL_CONFIG_FILE));
            }
        }

        #region Test Methods

        [TestMethod]
        public void ApiFactory_Default()
        {
            var defaultVersion = LevelUpClientFactory.Create(TestData.Valid.COMPANY_NAME,
                                                             TestData.Valid.PRODUCT_NAME,
                                                             TestData.Valid.PRODUCT_VERSION,
                                                             TestData.Valid.OS_NAME,
                                                             TestConstants.BASE_URL_CONFIG_FILE);

            Assert.IsNotNull(defaultVersion);
            Assert.IsTrue(defaultVersion is ILevelUpClient);
        }

        [TestMethod]
        public void Authenticate()
        {
            Assert.IsNotNull(AccessToken);
        }

        [TestMethod]
        public void Locations()
        {
            IList<Location> locations = Api.ListLocations(AccessToken.Token, AccessToken.MerchantId.GetValueOrDefault(3225));
            Assert.IsNotNull(locations);
            Assert.IsTrue(locations.Count > 0);
            Assert.IsNotNull(locations[0]);
        }

        [TestMethod]
        public void Locations_Count()
        {
            Assert.AreNotEqual(Api.ListLocations(AccessToken.Token, AccessToken.MerchantId.Value).Count, 0);
        }

        [TestMethod]
        public void LocationDetails_Invisible()
        {
            const int expectedStatus = (int) HttpStatusCodeExtended.NotFound;
            const string expectedErrorMessage = "This location may not exist, not be visible, or the merchant owner may not be live.";

            try
            {
                var location = Api.GetLocationDetails(AccessToken.Token, TestData.Valid.INVISIBLE_LOCATION_ID);
                Assert.Fail("Expected to catch a LevelUpApiException when requesting details for an invisible location");
            }
            catch (LevelUpApiException luEx)
            {
                // Catch expected exception
                Assert.AreEqual(expectedStatus, (int) luEx.StatusCode);
                Assert.IsTrue(luEx.Message.ToLower().Contains(expectedErrorMessage.ToLower()));
            }
        }

        [TestMethod]
        public void LocationDetails_Visible()
        {
            var location = Api.GetLocationDetails(AccessToken.Token, TestData.Valid.VISIBLE_LOCATION_ID);
            Assert.IsNotNull(location);
            Assert.IsTrue(location.IsVisible);
            Assert.AreEqual(TestData.Valid.VISIBLE_MERCHANT_ID, location.MerchantId);
            Assert.AreEqual(TestData.Valid.VISIBLE_LOCATION_MERCHANT_NAME, location.MerchantName);
        }

        [TestMethod]
        public void MerchantId()
        {
            Assert.AreEqual(AccessToken.MerchantId.GetValueOrDefault(0), TestData.Valid.POS_MERCHANT_ID);
        }

        [TestMethod]
        public void MerchantFundedCredit_Available()
        {
            var merchantCredit = Api.GetMerchantFundedCredit(AccessToken.Token,
                                                             GetLocationIdForMerchant(AccessToken.MerchantId),
                                                             LevelUpTestConfiguration.Current.QrData);
            Assert.IsNotNull(merchantCredit);
            Assert.IsTrue(merchantCredit.DiscountAmount >= 0);
        }

        #endregion Test Methods

        private int GetLocationIdForMerchant(int? merchantId)
        {
            IList<Location> locations = Api.ListLocations(AccessToken.Token,
                                                          merchantId.GetValueOrDefault(TestData.Valid.POS_MERCHANT_ID));
            Assert.IsNotNull(locations);
            Assert.IsTrue(locations.Count > 0);
            Assert.IsNotNull(locations[0]);
            return locations[0].LocationId;
        }
    }
}
