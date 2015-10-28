//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="ClientTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace LevelUp.Api.Client.Test
{
    [TestClass]
    public class ClientTests
    {
        [TestClass]
        [DeploymentItem(@"TestData\LevelUpBaseUri.config")]
        public class ReadBaseUriFromFile: ApiUnitTestsBase
        {
            private const string PRODUCTION_BASE_URL = @"https://api.thelevelup.com/";
            private const string TEST_DATA_DIR_PREFIX = "TestData";

            [TestMethod]
            public void DefaultPath()
            {
                ILevelUpClient client = LevelUpClientFactory.Create(TestData.Valid.COMPANY_NAME,
                                                                    TestData.Valid.PRODUCT_NAME,
                                                                    TestData.Valid.PRODUCT_VERSION,
                                                                    TestData.Valid.OS_NAME);

                client.ApiUrlBase.ShouldBeEquivalentTo(PRODUCTION_BASE_URL);
            }

            [TestMethod]
            public void CustomPath_FileDoesNotExist()
            {
                ILevelUpClient client = LevelUpClientFactory.Create(TestData.Valid.COMPANY_NAME,
                                                                    TestData.Valid.PRODUCT_NAME,
                                                                    TestData.Valid.PRODUCT_VERSION,
                                                                    TestData.Valid.OS_NAME,
                                                                    Path.Combine(TEST_DATA_DIR_PREFIX,
                                                                                 "TestCustomPath.Config"));
                client.ApiUrlBase.ShouldBeEquivalentTo(PRODUCTION_BASE_URL);
            }

            [TestMethod]
            public void CustomPath()
            {
                ILevelUpClient client = LevelUpClientFactory.Create(TestData.Valid.COMPANY_NAME,
                                                                    TestData.Valid.PRODUCT_NAME,
                                                                    TestData.Valid.PRODUCT_VERSION,
                                                                    TestData.Valid.OS_NAME,
                                                                    Path.Combine(TEST_DATA_DIR_PREFIX,
                                                                                 "TestLevelUpCustomPath.Config"));
                client.ApiUrlBase.ShouldBeEquivalentTo("https://sandbox.thelevelup.com/");
            }

            [TestMethod]
            public void DefaultPath_CustomUri()
            {
                const string customUri = "http://staging.thelevelup.com/";
                string source = Path.Combine(TEST_DATA_DIR_PREFIX, TestConstants.DEFAULT_URI_CONFIG_FILE);
                bool copied = false;

                if (File.Exists(source))
                {
                    File.Copy(source,
                              Path.Combine(Environment.CurrentDirectory, TestConstants.DEFAULT_URI_CONFIG_FILE),
                              true);
                    copied = true;
                }

                ILevelUpClient client = LevelUpClientFactory.Create(TestData.Valid.COMPANY_NAME,
                                                                    TestData.Valid.PRODUCT_NAME,
                                                                    TestData.Valid.PRODUCT_VERSION,
                                                                    TestData.Valid.OS_NAME);


                client.ApiUrlBase.ShouldBeEquivalentTo(customUri);

                if (copied &&
                    File.Exists(Path.Combine(Environment.CurrentDirectory, 
                    TestConstants.DEFAULT_URI_CONFIG_FILE)))
                {
                    File.Delete(Path.Combine(Environment.CurrentDirectory, TestConstants.DEFAULT_URI_CONFIG_FILE));
                }
            }

            [TestMethod]
            public void CustomPath_IpAddressAndPort()
            {
                ILevelUpClient client = LevelUpClientFactory.Create(TestData.Valid.COMPANY_NAME,
                                                                    TestData.Valid.PRODUCT_NAME,
                                                                    TestData.Valid.PRODUCT_VERSION,
                                                                    TestData.Valid.OS_NAME,
                                                                    Path.Combine(TEST_DATA_DIR_PREFIX,
                                                                                 "TestLevelUpCustomPath1.Config"));
                client.ApiUrlBase.ShouldBeEquivalentTo("http://192.168.2.3/");
            }

            [TestMethod]
            public void CustomPath_EmptyFile()
            {
                ILevelUpClient client = LevelUpClientFactory.Create(TestData.Valid.COMPANY_NAME,
                                                    TestData.Valid.PRODUCT_NAME,
                                                    TestData.Valid.PRODUCT_VERSION,
                                                    TestData.Valid.OS_NAME,
                                                    Path.Combine(TEST_DATA_DIR_PREFIX,
                                                                 "TestLevelUpCustomPath2.Config"));
                client.ApiUrlBase.ShouldBeEquivalentTo(PRODUCTION_BASE_URL);
            }

            [TestMethod]
            [ExpectedException(typeof(System.ArgumentException))]
            public void CustomPath_MultipleLines()
            {
                ILevelUpClient client = LevelUpClientFactory.Create(TestData.Valid.COMPANY_NAME,
                                    TestData.Valid.PRODUCT_NAME,
                                    TestData.Valid.PRODUCT_VERSION,
                                    TestData.Valid.OS_NAME,
                                    Path.Combine(TEST_DATA_DIR_PREFIX,
                                                 "TestLevelUpCustomPath3.Config"));
                client.ApiUrlBase.ShouldBeEquivalentTo("This is a test".ToLowerInvariant());
            }
        }
    }
}
