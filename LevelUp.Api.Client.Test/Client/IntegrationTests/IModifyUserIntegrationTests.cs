#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="IModifyUserIntegrationTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using System;
using LevelUp.Api.Client.ClientInterfaces;
using LevelUp.Api.Client.Models.Requests;
using LevelUp.Api.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LevelUp.Api.Client.Test.Client
{
    [TestClass]
    public class IModifyUserIntegrationTests
    {
        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.IntegrationTests)]
        [Ignore]    // Shouldn't keep making a bunch of sandbox users, at least by default
        public void CreateUserShouldSucceed()
        {
            var client = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IModifyUser>();

            // generate a new email via a guid
            string uniqueEmail = string.Format("{0}+test@thelevelup.com", Guid.NewGuid().ToString("N"));

            var user = client.CreateUser(LevelUpTestConfiguration.Current.ClientId, "john", "doe", uniqueEmail, "password");

            Assert.IsNotNull(user);
            Assert.AreEqual(user.FirstName, "john");
            Assert.AreEqual(user.Email, uniqueEmail);
        }
        
        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.IntegrationTests)]
        [ExpectedException(typeof(LevelUpApiException))]
        public void CreateUser_WillFailIfEmailIsNotUnique()
        {
            CreateUserRequestBodyUserSection request = new CreateUserRequestBodyUserSection(
                LevelUpTestConfiguration.Current.ConsumerUserFirstName,
                LevelUpTestConfiguration.Current.ConsumerUserLastInitial,
                LevelUpTestConfiguration.Current.MerchantEmailAddress,
                "password");

            var client = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IModifyUser>();
            client.CreateUser(LevelUpTestConfiguration.Current.ClientId, request);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.IntegrationTests)]
        public void UpdateUserShoudSucceed()
        {
            UpdateUserRequestBody request = new UpdateUserRequestBody(LevelUpTestConfiguration.Current.ConsumerId)
            {
                BornAt = DateTime.UtcNow.Date.AddYears(-25)
            };

            var client = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IModifyUser>();
            var user = client.UpdateUser(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpUserAccessToken, request);

            Assert.IsNotNull(user);
            Assert.AreEqual(user.BornAt, request.BornAt);

            request.BornAt = DateTimeOffset.UtcNow.Date; 
            user = client.UpdateUser(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpUserAccessToken, request);

            Assert.IsNotNull(user);
            Assert.IsTrue(user.BornAt.HasValue);
            Assert.AreEqual(user.BornAt, request.BornAt);
        }
    }
}
