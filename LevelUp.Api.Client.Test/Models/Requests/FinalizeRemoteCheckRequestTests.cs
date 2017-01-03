#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="FinalizeRemoteCheckRequestTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using FluentAssertions;
using LevelUp.Api.Client.Models.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Test.Models.Requests
{
    [TestClass]
    public class FinalizeRemoteCheckRequestTests : ModelTestsBase
    {
        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
        public void Serialize()
        {
            const int spend = 987;
            int? discount = 200;
            const int tax = 50;

            string expected = CreateExpectedSerializedString(spend, tax, discount);

            FinalizeRemoteCheckRequestBody request = new FinalizeRemoteCheckRequestBody(spend, tax, discount);

            string serialized = JsonConvert.SerializeObject(request);

            serialized.Should().NotBeNullOrEmpty();
            serialized.ShouldBeEquivalentTo(expected);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
        public void Serialize_Defaults()
        {
            const int spend = 8765;

            string expected = CreateExpectedSerializedString(spend);

            FinalizeRemoteCheckRequestBody request = new FinalizeRemoteCheckRequestBody(spend);

            string serialized = JsonConvert.SerializeObject(request);

            serialized.Should().NotBeNullOrEmpty();
            serialized.ShouldBeEquivalentTo(expected);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
        public void Serialize_NullDiscount()
        {
            const int spend = 8765;
            const int tax = 176;

            string expected = CreateExpectedSerializedString(spend, tax, null);

            FinalizeRemoteCheckRequestBody request = new FinalizeRemoteCheckRequestBody(spend, tax, null);

            string serialized = JsonConvert.SerializeObject(request);

            serialized.Should().NotBeNullOrEmpty();
            serialized.ShouldBeEquivalentTo(expected);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
        public void Serialize_NoTax()
        {
            const int spend = 8765;
            int? discount = 30;
            const int tax = 0;

            string expected = CreateExpectedSerializedString(spend, tax, discount);

            FinalizeRemoteCheckRequestBody request = new FinalizeRemoteCheckRequestBody(spend, tax, discount);

            string serialized = JsonConvert.SerializeObject(request);

            serialized.Should().NotBeNullOrEmpty();
            serialized.ShouldBeEquivalentTo(expected);
        }


        private string CreateExpectedSerializedString(int spend,
                                                      int tax = 0,
                                                      int? discount = null)
        {
            return "{\"order\":{\"applied_discount_amount\":" + FormatJsonNullableType(discount) +
                   ",\"tax_amount\":" + tax +
                   ",\"spend_amount\":" + spend + "}}";
        }
    }
}
