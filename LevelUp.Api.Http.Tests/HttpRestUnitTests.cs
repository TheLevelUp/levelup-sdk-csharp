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
using NUnit.Framework;
using RestSharp;

namespace LevelUp.Api.Http.Tests
{
    [TestFixture]
    public class HttpRestUnitTests
    {
        [Test]
        public void BuildRequestShouldSucceed()
        {
            const string body = "{{\"access_token\": {{\"api_key\": \"my_key\",\"username\": \"my_username\",\"password\": \"my_password\"}}}}";
            const string bodyType = "application/json";

            var headers = new Dictionary<string, string>();
            headers.Add("Authorization", "auth_value");
            headers.Add("Accept", bodyType);

            IRestRequest request = RestSharpUtils.BuildRequest(Method.GET, headers, null, body, RestSharpUtils.ContentType.Json);

            foreach (var pair in headers)
            {
                Assert.AreEqual(request.Parameters.First(x => x.Name == pair.Key).Value, pair.Value);
            }

            Assert.AreEqual(request.Parameters.First(x => x.Name == bodyType).Value, body);
        }

        [Test]
        public void CreateCombinedUrlShouldSucceed()
        {
            string baseUrlWithTrailingSlash = "http://test.thelevelup.com/";
            string baseUrlWithOutTrailingSlash = "http://test.thelevelup.com";
            string addendumWithLeadingSlash = "/blah";
            string addendumWithoutLeadingSlash = "blah";

            string expectedCombination = "http://test.thelevelup.com/blah";

            Assert.AreEqual(RestSharpUtils.CreateCombinedUrl(baseUrlWithTrailingSlash, addendumWithLeadingSlash), expectedCombination);
            Assert.AreEqual(RestSharpUtils.CreateCombinedUrl(baseUrlWithTrailingSlash, addendumWithoutLeadingSlash), expectedCombination);
            Assert.AreEqual(RestSharpUtils.CreateCombinedUrl(baseUrlWithOutTrailingSlash, addendumWithLeadingSlash), expectedCombination);
            Assert.AreEqual(RestSharpUtils.CreateCombinedUrl(baseUrlWithOutTrailingSlash, addendumWithoutLeadingSlash), expectedCombination);
        }

        [Test]
        public void CreateCombinedUrlShouldFailForInvalidFormat()
        {
            string baseUrl = "levelup";
            string addendum = @"\blah";
            Assert.Throws<UriFormatException>(() =>
            {
                RestSharpUtils.CreateCombinedUrl(baseUrl, addendum);
            });
        }

        [Test]
        public void CreateCombinedUrlShouldSucceedForFileFormat()
        {
            string baseUrl = "file://levelup";
            string addendum = "blah.txt";
            string expectedCombination = "file://levelup/blah.txt";

            Assert.AreEqual(RestSharpUtils.CreateCombinedUrl(baseUrl, addendum), expectedCombination);
        }
    }
}
