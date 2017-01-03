#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="IQueryUserUnitTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
    public class IQueryUserUnitTests
    {
        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
        public void ListUserAddressesShouldSucceed()
        {
            const string expectedRequestUrl = "https://sandbox.thelevelup.com/v15/user_addresses";

            RestResponse expectedResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = 
                    "[{" +
                        "\"user_address\": {" + 
                          "\"address_type\": \"payment\"," +
                          "\"extended_address\": \"\"," +
                          "\"id\": 149," +
                          "\"locality\": \"Boston\"," +
                          "\"postal_code\": \"01801\"," +
                          "\"region\": \"MA\"," +
                          "\"street_address\": \"123 Fake St\"" +
                        "}"+
                     "},{" +
                        "\"user_address\": {" + 
                          "\"address_type\": \"payment\"," +
                          "\"extended_address\": \"\"," +
                          "\"id\": 149," +
                          "\"locality\": \"Boston\"," +
                          "\"postal_code\": \"01801\"," +
                          "\"region\": \"MA\"," +
                          "\"street_address\": \"123 Fake St\"" +
                        "}"+
                    "}]"
            };
            
            IQueryUser client = ClientModuleUnitTestingUtilities.GetMockedLevelUpModule<IQueryUser>(
                expectedResponse, expectedRequestUrl: expectedRequestUrl);
            var addresses = client.ListUserAddresses("not_checking_this");
            Assert.AreEqual(addresses.Count, 2);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
        public void GetUserShouldSucceed()
        {
            const int id = 3;
            string expectedRequestUrl = string.Format("https://sandbox.thelevelup.com/v14/users/{0}", id);

            RestResponse expectedResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = 
                    "{" +
                      "\"user\": {" +
                        "\"born_at\": null," +
                        "\"cause_id\": 123," +
                        "\"connected_to_facebook\": true," +
                        "\"custom_attributes\": { \"foo\": \"bar\" }," +
                        "\"email\": \"ryanp@thelevelup.com\"," +
                        "\"first_name\": \"Ryan\"," +
                        "\"gender\": null," +
                        "\"global_credit_amount\": 0," +
                        "\"id\": 123," +
                        "\"last_name\": \"Punxsutawney\"," +
                        "\"merchants_visited_count\": 0," +
                        "\"orders_count\": 0," +
                        "\"phone\": \"(555) 555-5555\"," +
                        "\"terms_accepted_at\": null," +
                        "\"total_savings_amount\": 0" +
                      "}" +
                    "}"
            };

            IQueryUser client = ClientModuleUnitTestingUtilities.GetMockedLevelUpModule<IQueryUser>(
                expectedResponse, expectedRequestUrl: expectedRequestUrl);
            var user = client.GetUser("not_checking_this", id);

            Assert.AreEqual(user.Id, 123);
        }
    }
}
