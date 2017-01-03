#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="IModifyUserUnitTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using System.Collections.Generic;
using System.Net;
using LevelUp.Api.Client.ClientInterfaces;
using LevelUp.Api.Client.Models.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThirdParty.RestSharp;

namespace LevelUp.Api.Client.Test.Client
{
    [TestClass]
    public class IModifyUserUnitTests
    {
        private const string email = "ryanp@thelevelup.com";
        private const string firstName = "Ryan";
        private const string lastName = "Punxsutawney";
        private const string password = "s3cr3t";
        private const string apiKey = "api_key_12345";
        private const int globalCreditAmount = 0;
        private const object gender = null;
        private KeyValuePair<string, string> customAttributes = new KeyValuePair<string, string>("foo", "bar");

        private RestResponse expectedCreateOrUpdateUserResponse = new RestResponse
        {
            StatusCode = HttpStatusCode.OK,
            Content = string.Format("{{" +
                                        "\"user\":{{" +
                                            "\"born_at\":null," +
                                            "\"cause_id\":123," +
                                            "\"connected_to_facebook\":true," +
                                            "\"custom_attributes\":{{ \"foo\":\"bar\"}}," +
                                            "\"email\":\"{0}\"," +
                                            "\"first_name\":\"{1}\"," +
                                            "\"gender\":{2}," +
                                            "\"global_credit_amount\":{3}," +
                                            "\"id\":123," +
                                            "\"last_name\":\"{4}\"," +
                                            "\"merchants_visited_count\":0," +
                                            "\"orders_count\":0," +
                                            "\"terms_accepted_at\":null," +
                                            "\"total_savings_amount\":0" +
                                        "}}" +
                                    "}}", email, firstName, gender ?? "null", globalCreditAmount, lastName)
        };

        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
        public void CreateUserShouldSucceed()
        {
            string expectedRequestUrl = "https://sandbox.thelevelup.com/v14/users";

            string expectedRequestBody = string.Format("{{" +
                                                        "\"api_key\": \"{0}\"," +
                                                        "\"user\":{{" +
                                                            "\"email\":\"{1}\"," +
                                                            "\"first_name\":\"{2}\"," +
                                                            "\"last_name\":\"{3}\"," +
                                                            "\"password\":\"{4}\"" +
                                                        "}}," +
                                                    "}}", apiKey, email, firstName, lastName, password);

            IModifyUser client = ClientModuleUnitTestingUtilities.GetMockedLevelUpModule<IModifyUser, CreateUserRequest>(
expectedCreateOrUpdateUserResponse, expectedRequestBody, expectedRequestUrl: expectedRequestUrl);
var user = client.CreateUser(apiKey, firstName, lastName, email, password);

            Assert.AreEqual(user.GlobalCreditAmount, globalCreditAmount);
            Assert.AreEqual(user.Gender, gender);
            Assert.AreEqual(user.CustomAttributes[customAttributes.Key], customAttributes.Value);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
        public void UpdateUserShouldSucceed()
        {
            string expectedRequestUrl = "https://sandbox.thelevelup.com/v14/users/123";

            IModifyUser client = ClientModuleUnitTestingUtilities.GetMockedLevelUpModule<IModifyUser>(
                expectedCreateOrUpdateUserResponse, expectedRequestUrl: expectedRequestUrl);
            var user = client.UpdateUser("not_checking_this", new UpdateUserRequestBody(123));

            Assert.AreEqual(user.GlobalCreditAmount, globalCreditAmount);
            Assert.AreEqual(user.Gender, gender);
            Assert.AreEqual(user.CustomAttributes[customAttributes.Key], customAttributes.Value);
        }


        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
        public void PasswordResetRequestShouldSucceed()
        {
            const string expectedRequestUrl = "https://sandbox.thelevelup.com/v14/passwords";
            const string expectedRequestBody = "{\"email\": \"foo@example.com\"}";

            RestResponse expectedResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.NoContent
            };

            IModifyUser client = ClientModuleUnitTestingUtilities.GetMockedLevelUpModule<IModifyUser, PasswordResetRequest>(
                expectedResponse, expectedRequestBody, expectedRequestUrl);
            client.PasswordResetRequest("foo@example.com");
        }
    }
}
