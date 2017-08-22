#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="RequestVersionUnitTest.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LevelUp.Api.Client.Test.Models
{
    [TestClass]
    public class RequestVersionUnitTest
    {
        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.UnitTests)]
        [ExpectedException(typeof(InvalidOperationException), "Failed to throw InvalidOperationException for a request object with unspecified applicable versions")]
        public void ModelWithUnspecifiedRequestVersion_ShouldThrow()
        {
            var request = new Requests.RequestWithNoSpecifiedVersions("arbitrary_access_token");
            try
            {
                var version = request.ApiVersion;
            }
            catch (InvalidOperationException ex)
            {
                Assert.AreEqual(ex.Message, "The request object of type [" + request.GetType() +
                    "] does not specify any applicable platform API verisons.");
                throw;
            }
        }
    }
}
