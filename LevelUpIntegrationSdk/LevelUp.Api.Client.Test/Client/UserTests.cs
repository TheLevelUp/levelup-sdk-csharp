//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="UserTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using LevelUp.Api.Client.Models.Requests;
using LevelUp.Api.Client.Models.Responses;
using LevelUp.Api.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace LevelUp.Api.Client.Test
{
    [TestClass]
    [DeploymentItem("LevelUpBaseUri.txt")]
    [DeploymentItem("test_config_settings.xml")]
    public class UserTests : ApiUnitTestsBase
    {
        [TestMethod]
        [Ignore]
        public void CreateUser()
        {
            // generate a new email via a guid
            string uniqueEmail = string.Format("{0}+test@thelevelup.com", Guid.NewGuid().ToString("N"));

            CreateUserRequest request = new CreateUserRequest(LevelUpTestConfiguration.Current.User_FirstName,
                                                              LevelUpTestConfiguration.Current.User_LastInitial,
                                                              uniqueEmail,
                                                              "password");

            User user = Api.CreateUser(AccessToken.Token, request);
            Assert.IsNotNull(user);
        }

        [TestMethod]
        [ExpectedException(typeof(LevelUpApiException))]
        public void CreateUser_WillFailIfEmailIsNotUnique()
        {
            CreateUserRequest request = new CreateUserRequest(LevelUpTestConfiguration.Current.User_FirstName,
                                                              LevelUpTestConfiguration.Current.User_LastInitial,
                                                              LevelUpTestConfiguration.Current.User_EmailAddress,
                                                              "password");

            Api.CreateUser(AccessToken.Token, request);
        }

        [TestMethod]
        public void GetUser()
        {
            User user = Api.GetUser(AccessToken.Token, LevelUpTestConfiguration.Current.User_Id);
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void ListUserAddresses()
        {
            IList<UserAddress> addresses = Api.ListUserAddresses(AccessToken.Token);
            Assert.IsNotNull(addresses);
        }

        [TestMethod]
        [Ignore]
        public void ResetPassword()
        {
            Api.PasswordResetRequest(LevelUpTestConfiguration.Current.User_EmailAddress);
            // because the method didn't throw we know it worked.
        }

        [TestMethod]
        [Ignore]
        public void UpdateUser()
        {
            UpdateUserRequest request = new UpdateUserRequest(LevelUpTestConfiguration.Current.User_Id)
            {
                BornAt = DateTime.Today.AddYears(-25)
            };

            User user = Api.UpdateUser(AccessToken.Token, request);

            Assert.IsNotNull(user);
        }
    }
}
