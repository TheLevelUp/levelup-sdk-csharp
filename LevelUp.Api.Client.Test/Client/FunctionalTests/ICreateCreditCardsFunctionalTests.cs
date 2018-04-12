#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="ICreateCreditCardsFunctionalTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using LevelUp.Api.Client.Models.Requests;
using LevelUp.Api.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;

namespace LevelUp.Api.Client.Test.Client.FunctionalTests
{
    [TestClass]
    public class ICreateCreditCardsFunctionalTests
    {
        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.FunctionalTests)]
        public void CreateCreditCardShouldSucceed()
        {
            const string expectedRequestUrl = "https://sandbox.thelevelup.com/v15/credit_cards";

            // The below strings can be arbitrary for the purposes of this test -- These are just the examples from the api docs.
            const string encrypted_cvv = "$bt4|javascript_1_3_9$Zar7J1+0QNsrHtKFufeJ8UCpSd5RM1PwTjzNE1Dm1N0A969OuWfU03...";
            const string encrypted_expiration_month = "$bt4|javascript_1_3_9$7ad9aydahduiw+89w7dHusaihdas...";
            const string encrypted_expiration_year = "$bt4|javascript_1_3_9$9asdjaIjashuUHsj+saiUSj...";
            const string encrypted_number = "$bt4|javascript_1_3_9$FyreT+o2W/9VHHjS43ZJJe2SmdvTBcve58...";
            const string postal_code = "12345";
            
            string expectedRequestbody = string.Format(
                "{{" +
                    "\"credit_card\": {{" +
                        "\"encrypted_cvv\": \"{0}\"," +
                        "\"encrypted_expiration_month\": \"{1}\"," +
                        "\"encrypted_expiration_year\": \"{2}\"," +
                        "\"encrypted_number\": \"{3}\"," +
                        "\"postal_code\": \"{4}\" " +
                    "}}" +
                "}}", encrypted_cvv, encrypted_expiration_month, encrypted_expiration_year, encrypted_number, postal_code);
        
            RestResponse expectedResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = "{" +
                              "\"credit_card\": {" +
                                  "\"bin\": \"123456\"," +
                                  "\"description\": \"JCB ending in 1234\"," +
                                  "\"expiration_month\": 7," +
                                  "\"expiration_year\": 2015," +
                                  "\"id\": 305999," +
                                  "\"last_4\": \"1234\"," +
                                  "\"promoted\": true," +
                                  "\"state\": \"active\"," +
                                  "\"type\": \"JCB\"" +
                              "}" +
                          "}"
            };

            const string accessToken = "abc";

            ICreateCreditCards client = ClientModuleFunctionalTestingUtilities.GetMockedLevelUpModule<ICreateCreditCards, CreateCreditCardRequest>(
                expectedResponse, expectedRequestbody, expectedAccessToken: accessToken, expectedRequestUrl: expectedRequestUrl);
            var card = client.CreateCreditCard(accessToken, encrypted_number, encrypted_expiration_month, encrypted_expiration_year, encrypted_cvv, postal_code);

            Assert.AreEqual(card.Bin, "123456");
            Assert.AreEqual(card.Description, "JCB ending in 1234");
            Assert.AreEqual(card.ExpirationMonth, 7);
            Assert.AreEqual(card.ExpirationYear, 2015);
            Assert.AreEqual(card.Id, 305999);
            Assert.AreEqual(card.Last4Numbers, "1234");
            Assert.AreEqual(card.Promoted, true);
            Assert.AreEqual(card.State, "active");
            Assert.AreEqual(card.Type, "JCB");
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.FunctionalTests)]
        public void CreateCreditCardShouldFailForInvalidCardDetails()
        {
            RestResponse expectedResponse = new RestResponse
            {
                StatusCode = (HttpStatusCode) 422,
                Content = "[{\"error\":{\"object\":\"credit_card\",\"property\":\"expiration_year\",\"message\":\"Expiration year is invalid.\"}}]"
            };

            try
            {
                ICreateCreditCards client = ClientModuleFunctionalTestingUtilities.GetMockedLevelUpModule<ICreateCreditCards>(expectedResponse);
                var card = client.CreateCreditCard("abc", new CreateCreditCardRequestBody("we", "aren't", "checking", "these", "params"));
                Assert.IsTrue(false, "Failed to throw a LevelUpApiException for invalid card data (422 response code)");
            }
            catch (LevelUpApiException ex)
            {
                Assert.AreEqual((int) ex.StatusCode, 422);
                Assert.AreEqual(ex.Message.Trim(), "Expiration year is invalid.");
            }
        }
    }
}
