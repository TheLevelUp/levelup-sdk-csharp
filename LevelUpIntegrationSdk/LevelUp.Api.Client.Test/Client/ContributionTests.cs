//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="ContributionTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
//   Copyright(c) 2014 SCVNGR, Inc. d/b/a LevelUp. All rights reserved.
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

using System;
using System.Collections.Generic;
using LevelUp.Api.Client;
using LevelUp.Api.Client.Models.Responses;
using LevelUp.Api.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LevelUp.Api.Client.Test
{
    [TestClass]
    public class ContributionTests : ApiUnitTestsBase
    {
        [TestMethod]
        public void GetContributionTarget()
        {
            ContributionTarget target = Api.GetContributionTarget(TestData.Valid.CONTRIBUTION_TARGET_ID);
            Assert.IsNotNull(target);
        }

        [TestMethod]
        [ExpectedException(typeof(LevelUpApiException))]
        public void GetContributionTarget_WillThrowWhenTargetNotFound()
        {
            Api.GetContributionTarget("NOT_A_TARGET");
        }

        [TestMethod]
        public void ListContributionTargets()
        {
            IList<ContributionTarget> targets = Api.ListContributionTargets();
            Assert.IsNotNull(targets);
            Assert.IsTrue(targets.Count > 0);
            Assert.IsNotNull(targets[0]);

            Console.WriteLine(targets[0]);
        }
    }
}
