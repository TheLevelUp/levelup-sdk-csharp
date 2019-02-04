#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="IDestroyCreditCardsFunctionalTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using System.Net;
using LevelUp.Api.Client.ClientInterfaces;
using NUnit.Framework;
using RestSharp;

namespace LevelUp.Api.Client.Tests.Client.FunctionalTests
{
    [TestFixture]
    public class IDestroyCreditCardsFunctionalTests
    {
        [Test]
        public void DeleteCreditCardShouldSucceed()
        {
            const int id = 305999;
            const int bin = 123456;
            const int expiration_month = 7;
            const int expiration_year = 2015;
            const bool promoted = true;
            const string description = "JCBendingin1234";
            const string last_4 = "1234";
            const string state = "inactive";
            const string @type = "JCB";

            string expectedRequestUrl = string.Format(ClientModuleFunctionalTestingUtilities.SANDBOX_URL_PREFIX  + "/v14/credit_cards/{0}", id);

            RestResponse expectedResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = string.Format("{{" +
                                            "\"credit_card\":{{" +
                                                "\"bin\":\"{0}\"," +
                                                "\"description\":\"{1}\"," +
                                                "\"expiration_month\":{2}," +
                                                "\"expiration_year\":{3}," +
                                                "\"id\":{4}," +
                                                "\"last_4\":\"{5}\"," +
                                                "\"promoted\":{6}," +
                                                "\"state\":\"{7}\"," +
                                                "\"type\":\"{8}\"" +
                                            "}}" +
                                        "}}",
                                        bin, description, expiration_month, expiration_year, id, last_4, promoted, state, @type)
            };

            IDestroyCreditCards client = ClientModuleFunctionalTestingUtilities.GetMockedLevelUpModule<IDestroyCreditCards>(
                expectedResponse, expectedRequestUrl: expectedRequestUrl);
            client.DeleteCreditCard("not_checking_this", id);
        }
    }
}
