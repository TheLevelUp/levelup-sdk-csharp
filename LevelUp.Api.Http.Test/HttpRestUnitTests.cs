#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="HttpRestUnitTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;

namespace LevelUp.Api.Http.Test
{
    [TestClass]
    public class HttpRestUnitTests
    {
        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.UnitTests)]
        public void BuildRequestShouldSucceed()
        {
            const string body = "{{\"access_token\": {{\"api_key\": \"my_key\",\"username\": \"my_username\",\"password\": \"my_password\"}}}}";
            const string bodyType = "application/json";

            var headers = new Dictionary<string, string>();
            headers.Add("Authorization", "auth_value");
            headers.Add("Accept", bodyType);

            IRestRequest request = HttpRest.BuildRequest(Method.GET, headers, null, body, HttpRest.ContentType.Json);

            foreach (var pair in headers)
            {
                Assert.AreEqual(request.Parameters.First(x => x.Name == pair.Key).Value, pair.Value);
            }

            Assert.AreEqual(request.Parameters.First(x => x.Name == bodyType).Value, body);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.UnitTests)]
        public void CreateCombinedUrlShouldSucceed()
        {
            string baseUrlWithTrailingSlash = "http://test.thelevelup.com/";
            string baseUrlWithOutTrailingSlash = "http://test.thelevelup.com";
            string addendumWithLeadingSlash = "/blah";
            string addendumWithoutLeadingSlash = "blah";

            string expectedCombination = "http://test.thelevelup.com/blah";

            Assert.AreEqual(HttpRest.CreateCombinedUrl(baseUrlWithTrailingSlash, addendumWithLeadingSlash), expectedCombination);
            Assert.AreEqual(HttpRest.CreateCombinedUrl(baseUrlWithTrailingSlash, addendumWithoutLeadingSlash), expectedCombination);
            Assert.AreEqual(HttpRest.CreateCombinedUrl(baseUrlWithOutTrailingSlash, addendumWithLeadingSlash), expectedCombination);
            Assert.AreEqual(HttpRest.CreateCombinedUrl(baseUrlWithOutTrailingSlash, addendumWithoutLeadingSlash), expectedCombination);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.UnitTests)]
        [ExpectedException(typeof(UriFormatException))]
        public void CreateCombinedUrlShouldFailForInvalidFormat()
        {
            string baseUrl = "levelup";
            string addendum = "\blah";

            HttpRest.CreateCombinedUrl(baseUrl, addendum);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.UnitTests)]
        public void CreateCombinedUrlShouldSucceedForFileFormat()
        {
            string baseUrl = "file://levelup";
            string addendum = "blah.txt";
            string expectedCombination = "file://levelup/blah.txt";

            Assert.AreEqual(HttpRest.CreateCombinedUrl(baseUrl, addendum), expectedCombination);
        }
    }
}
