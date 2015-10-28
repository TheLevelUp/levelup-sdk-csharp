//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="GiftCardTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using LevelUp.Api.Client.Models.Requests;
using LevelUp.Api.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace LevelUp.Api.Client.Test
{
    [TestClass]
    [DeploymentItem("LevelUpBaseUri.txt")]
    [DeploymentItem("test_config_settings.xml")]
    public class GiftCardTests : ApiUnitTestsBase
    {
        private const int VALUE_TO_ADD_REMOVE = 1000;
        private const int NEGATIVE_VALUE = -1;
        private const string MUST_LOAD_POSITIVE_ERROR_MESSAGE = "You must load a positive amount";
        private const string MINIMUM_AMOUNT_ERROR_MESSAGE = "The minimum amount you can load";
        private const string MUST_REMOVE_POSITIVE_ERROR_MESSAGE = "You may only remove a positive value";

        [TestMethod]
        public void GiftCard_AddValue()
        {
            AddValueToGiftCard(VALUE_TO_ADD_REMOVE);
        }

        [TestMethod]
        [ExpectedException(typeof(LevelUpApiException))]
        public void GiftCard_AddValue_Zero()
        {
            const int expectedStatus = (int)HttpStatusCodeExtended.UnprocessableEntity;

            GiftCardAddValueRequest request = new GiftCardAddValueRequest(LevelUpTestConfiguration.Current.User_GiftCardPaymentToken,
                                                                          0,
                                                                          LevelUpTestConfiguration.Current.Merchant_LocationId_Visible,
                                                                          "abc123",
                                                                          new List<string>() {"cash", "Credit - Discover"},
                                                                          null);

            try
            {
                var response = base.Api.GiftCardAddValue(base.AccessToken.Token,
                                                         LevelUpTestConfiguration.Current.Merchant_Id,
                                                         request);
                Assert.Fail("Expected LevelUpApiException on add zero value but did not catch it.");
            }
            catch (LevelUpApiException luEx)
            {
                //Catch expected exception
                Assert.AreEqual(expectedStatus, (int)luEx.StatusCode);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(LevelUpApiException))]
        public void GiftCard_AddValue_Negative()
        {
            const int expectedStatus = (int) HttpStatusCodeExtended.UnprocessableEntity;

            GiftCardAddValueRequest request = new GiftCardAddValueRequest(LevelUpTestConfiguration.Current.User_GiftCardPaymentToken,
                                                                          NEGATIVE_VALUE,
                                                                          LevelUpTestConfiguration.Current.Merchant_LocationId_Visible,
                                                                          "abc123",
                                                                          new List<string>() { "cash", "Credit - Discover" });

            try
            {
                var response = base.Api.GiftCardAddValue(base.AccessToken.Token, LevelUpTestConfiguration.Current.Merchant_Id, request);
                Assert.Fail("Expected LevelUpApiException on add negative value but did not catch it.");
            }
            catch (LevelUpApiException luEx)
            {
                //Catch expected exception
                Assert.AreEqual(expectedStatus, (int)luEx.StatusCode);
                throw;
            }
        }

        [TestMethod]
        public void GiftCard_DestroyValue()
        {
            var addValueResponse = AddValueToGiftCard(VALUE_TO_ADD_REMOVE);

            var request = new GiftCardRemoveValueRequest(LevelUpTestConfiguration.Current.User_GiftCardPaymentToken,
                                                         VALUE_TO_ADD_REMOVE);

            var response = Api.GiftCardDestroyValue(AccessToken.Token, LevelUpTestConfiguration.Current.Merchant_Id, request);

            Assert.IsNotNull(response);
            Assert.AreEqual(VALUE_TO_ADD_REMOVE, response.AmountRemovedInCents);
            Assert.AreEqual(VALUE_TO_ADD_REMOVE,
                            response.PreviousGiftCardAmountInCents - response.NewGiftCardAmountInCents);
        }

        [TestMethod]
        [ExpectedException(typeof(LevelUpApiException))]
        public void GiftCard_DestroyValue_Zero()
        {
            const int expectedStatus = (int)HttpStatusCodeExtended.UnprocessableEntity;
            const string expectedMessage = MUST_REMOVE_POSITIVE_ERROR_MESSAGE;

            GiftCardRemoveValueRequest request =
                new GiftCardRemoveValueRequest(LevelUpTestConfiguration.Current.User_GiftCardPaymentToken, 0);

            try
            {
                var response = base.Api.GiftCardDestroyValue(base.AccessToken.Token,
                                                            LevelUpTestConfiguration.Current.Merchant_Id,
                                                            request);
                Assert.Fail("Expected LevelUpApiException on remove zero value but did not catch it.");
            }
            catch (LevelUpApiException luEx)
            {
                //Catch expected exception
                Assert.AreEqual(expectedStatus, (int)luEx.StatusCode);
                Assert.IsTrue(luEx.Message.ToLower().Contains(expectedMessage.ToLower()));
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(LevelUpApiException))]
        public void GiftCard_DestroyValue_Negative()
        {
            const int expectedStatus = (int)HttpStatusCodeExtended.UnprocessableEntity;
            const string expectedMessage = MUST_REMOVE_POSITIVE_ERROR_MESSAGE;

            GiftCardRemoveValueRequest request =
                new GiftCardRemoveValueRequest(LevelUpTestConfiguration.Current.User_GiftCardPaymentToken, NEGATIVE_VALUE);

            try
            {
                var response = base.Api.GiftCardDestroyValue(base.AccessToken.Token,
                                                            LevelUpTestConfiguration.Current.Merchant_Id,
                                                            request);
                Assert.Fail("Expected LevelUpApiException on remove negative value but did not catch it.");
            }
            catch (LevelUpApiException luEx)
            {
                //Catch expected exception
                Assert.AreEqual(expectedStatus, (int)luEx.StatusCode);
                Assert.IsTrue(luEx.Message.ToLower().Contains(expectedMessage.ToLower()));
                throw;
            }
        }

        [TestMethod]
        public void GiftCard_DestroyValue_TotalValue()
        {
            var addValueResponse = AddValueToGiftCard(VALUE_TO_ADD_REMOVE);

            var request = new GiftCardRemoveValueRequest(LevelUpTestConfiguration.Current.User_GiftCardPaymentToken,
                                                         addValueResponse.NewGiftCardAmountInCents);

            var response = Api.GiftCardDestroyValue(AccessToken.Token, LevelUpTestConfiguration.Current.Merchant_Id, request);

            Assert.IsNotNull(response);
            Assert.AreEqual(0, response.NewGiftCardAmountInCents);
            Assert.AreEqual(addValueResponse.NewGiftCardAmountInCents, response.AmountRemovedInCents);
            Assert.AreEqual(addValueResponse.NewGiftCardAmountInCents,
                            response.PreviousGiftCardAmountInCents - response.NewGiftCardAmountInCents);
        }

        [TestMethod]
        [ExpectedException(typeof (LevelUpApiException))]
        public void GiftCard_DestroyValue_ExceedsTotalValue()
        {
            const int expectedStatus = (int) HttpStatusCodeExtended.UnprocessableEntity;
            const string expectedMessageBeginning = "This gift card has a balance of ";
            const string expectedMessageEnding = ". Please retry with that amount.";

            var addValueResponse = AddValueToGiftCard(1);

            var request = new GiftCardRemoveValueRequest(LevelUpTestConfiguration.Current.User_GiftCardPaymentToken,
                                                         addValueResponse.NewGiftCardAmountInCents + 1);

            try
            {
                var response = Api.GiftCardDestroyValue(AccessToken.Token, LevelUpTestConfiguration.Current.Merchant_Id, request);
                Assert.Fail("Expected LevelUpApiException on remove more value than is available but did not catch it.");
            }
            catch (LevelUpApiException luEx)
            {
                //Catch expected exception
                Assert.AreEqual(expectedStatus, (int)luEx.StatusCode);
                string exceptionMessage = luEx.Message.Trim().ToLower();
                Assert.IsTrue(exceptionMessage.StartsWith(expectedMessageBeginning.ToLower()) &&
                              exceptionMessage.EndsWith(expectedMessageEnding.ToLower()));
                throw;
            }
        }
    }
}
