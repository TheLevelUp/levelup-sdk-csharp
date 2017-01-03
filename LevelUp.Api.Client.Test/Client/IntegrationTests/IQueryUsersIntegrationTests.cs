#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="IQueryUsersIntegrationTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using System.Linq;
using LevelUp.Api.Client.ClientInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LevelUp.Api.Client.Test.Client
{
    [TestClass]
    public class IQueryUsersIntegrationTests
    {
        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.IntegrationTests)]
        public void GetUser()
        {
            IQueryUser queryInterface = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IQueryUser>();

            var userData = queryInterface.GetUser(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpUserAccessToken,
                LevelUpTestConfiguration.Current.ConsumerId);

            Assert.AreEqual(userData.FirstName, LevelUpTestConfiguration.Current.ConsumerUserFirstName);
            Assert.AreEqual(userData.LastName.ToLower().ToCharArray()[0], 
                LevelUpTestConfiguration.Current.ConsumerUserLastInitial.ToLower().ToCharArray()[0]);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.IntegrationTests)]
        public void GetLocationDetails()
        {
            IQueryUser queryInterface = ClientModuleIntegrationTestingUtilities.GetSandboxedLevelUpModule<IQueryUser>();

            var locationData = queryInterface.ListUserAddresses(ClientModuleIntegrationTestingUtilities.SandboxedLevelUpUserAccessToken);
            Assert.IsTrue(locationData.Count > 0);
            Assert.IsNotNull(locationData.Where(x => x.StreetAddress == "101 Arch Street").DefaultIfEmpty(null).FirstOrDefault());
        }
    }
}
