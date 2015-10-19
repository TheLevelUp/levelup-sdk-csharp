//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="OrdersTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using LevelUp.Api.Client.Filters;
using LevelUp.Api.Client.Models.Responses;
using LevelUp.Api.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace LevelUp.Api.Client.Test
{
    [TestClass]
    [DeploymentItem("LevelUpBaseUri.txt")]
    [DeploymentItem("test_config_settings.xml")]
    public class OrdersTests : ApiUnitTestsBase
    {
        private const int NUM_PAGES_TO_SEARCH = 5;
        private readonly DateTime _filterStartDate = DateTime.Now - new TimeSpan(4, 0, 0, 0, 0); //4 days ago
        private readonly DateTime _filterEndDate = DateTime.Now - new TimeSpan(2, 0, 0, 0, 0); //2 days ago
        private readonly OrdersByDateFilter _dateFilter;
        private readonly OrdersByCustomerNameFilter _nameFilter;

        public OrdersTests()
        {
            _dateFilter = new OrdersByDateFilter(_filterStartDate, _filterEndDate);
            _nameFilter = new OrdersByCustomerNameFilter(LevelUpTestConfiguration.Current.User_FirstName,
                                                         LevelUpTestConfiguration.Current.User_LastInitial);
        }

        [TestMethod]
        public void FailedOrder_BadQrCode()
        {
            const int expectedStatus = (int)HttpStatusCodeExtended.UnprocessableEntity;
            const string expectedErrorMessage = "We couldn't recognize that as a valid LevelUp Code.";

            try
            {
                PlaceOrder(AccessToken.Token, 
                           spendAmount: EXPECTED_SPEND_AMOUNT_CENTS, 
                           qrCodeToUse: LevelUpTestConfiguration.Current.User_InvalidPaymentToken);
                Assert.Fail("Expected LevelUpApiException on order with bad Qr data but did not catch it!");
            }
            catch (LevelUpApiException luEx)
            {
                // Catch expected exception
                Assert.AreEqual(expectedStatus, (int)luEx.StatusCode);
                Assert.IsTrue(luEx.Message.ToLower().Contains(expectedErrorMessage.ToLower()));
            }
        }

        [TestMethod]
        public void Order()
        {
            const int tipPercent = 0;
            const double tipDecimal = (double)tipPercent / 100;

            OrderResponse orderResponse = PlaceOrder(AccessToken.Token,
                                                     spendAmount: EXPECTED_SPEND_AMOUNT_CENTS,
                                                     qrCodeToUse: LevelUpTestConfiguration.Current.User_PaymentToken);

            Assert.IsNotNull(orderResponse);
            Assert.AreEqual(EXPECTED_SPEND_AMOUNT_CENTS, orderResponse.SpendAmount);
            Assert.AreEqual(tipDecimal * EXPECTED_SPEND_AMOUNT_CENTS, orderResponse.TipAmount);
            Assert.AreEqual(EXPECTED_SPEND_AMOUNT_CENTS, orderResponse.Total);

            Api.RefundOrder(AccessToken.Token, orderResponse.Identifier);
        }

        [TestMethod]
        public void Order_PartialAuthEnabled()
        {
            const int tipPercent = 0;
            const double tipDecimal = (double)tipPercent / 100;

            OrderResponse orderResponse = PlaceOrder(AccessToken.Token,
                                                     spendAmount: EXPECTED_SPEND_AMOUNT_CENTS,
                                                     qrCodeToUse: LevelUpTestConfiguration.Current.User_PaymentToken,
                                                     partialAuthEnabled: true);

            Assert.IsNotNull(orderResponse);
            Assert.AreEqual(EXPECTED_SPEND_AMOUNT_CENTS, orderResponse.SpendAmount);
            Assert.AreEqual(tipDecimal * EXPECTED_SPEND_AMOUNT_CENTS, orderResponse.TipAmount);
            Assert.AreEqual(EXPECTED_SPEND_AMOUNT_CENTS, orderResponse.Total);

            Api.RefundOrder(AccessToken.Token, orderResponse.Identifier);
        }

        [Ignore]
        [TestMethod]
        public void OrderWithExemptionAmount()
        {
            const int spendAmount = 10;
            const int exemptionAmount = 5;
            const int expectedChange = spendAmount - exemptionAmount;

            Loyalty loyalty = Api.GetLoyalty(AccessToken.Token, AccessToken.MerchantId.GetValueOrDefault());

            var orderResponse = PlaceOrder(AccessToken.Token,
                                           spendAmount: spendAmount,
                                           exemptionAmount: exemptionAmount,
                                           qrCodeToUse: LevelUpTestConfiguration.Current.User_PaymentToken);

            Loyalty postLoyalty = Api.GetLoyalty(AccessToken.Token, AccessToken.MerchantId.GetValueOrDefault());

            Assert.AreEqual(loyalty.SpendRemainingAmount - postLoyalty.SpendRemainingAmount,
                            expectedChange,
                            string.Format("Spend remaining should have been reduced by {0} ", expectedChange));

            Api.RefundOrder(AccessToken.Token, orderResponse.Identifier);
        }

        [TestMethod]
        public void Order_With_DiscountAmount()
        {
            const int spendAmountCents = 20;
            const int discountAmountCents = 10;

            OrderResponse response = PlaceOrder(AccessToken.Token,
                                                spendAmount: spendAmountCents,
                                                appliedDiscountAmount: discountAmountCents,
                                                qrCodeToUse: LevelUpTestConfiguration.Current.User_PaymentToken);

            Assert.IsNotNull(response);
            Assert.AreEqual(spendAmountCents, response.SpendAmount);

            Api.RefundOrder(AccessToken.Token, response.Identifier);
        }

        [TestMethod]
        public void Order_With_GiftCardAmount()
        {
            const int spendAmountCents = 10;
            const int giftCardValueToAddIfZero = 1000;

            MerchantFundedCreditResponse creditResponse = Api.GetMerchantFundedCredit(AccessToken.Token,
                                                                                      LevelUpTestConfiguration.Current
                                                                                                              .Merchant_LocationId_Visible,
                                                                                      LevelUpTestConfiguration.Current
                                                                                                              .User_PaymentToken);

            Assert.IsNotNull(creditResponse);
            Assert.IsNotNull(creditResponse.GiftCardAmount);

            if (creditResponse.GiftCardAmount == 0)
            {
                AddValueToGiftCard(giftCardValueToAddIfZero);

                creditResponse = Api.GetMerchantFundedCredit(AccessToken.Token,
                                                             LevelUpTestConfiguration.Current.Merchant_LocationId_Visible,
                                                             LevelUpTestConfiguration.Current.User_PaymentToken);

                Assert.IsNotNull(creditResponse);
                Assert.IsNotNull(creditResponse.GiftCardAmount);
            }

            OrderResponse response = PlaceOrder(AccessToken.Token,
                                                spendAmount: spendAmountCents,
                                                availableGiftCardAmount: creditResponse.GiftCardAmount,
                                                qrCodeToUse: LevelUpTestConfiguration.Current.User_PaymentToken);

            Assert.IsNotNull(response);
            Assert.AreEqual(spendAmountCents, response.SpendAmount);

            Api.RefundOrder(AccessToken.Token, response.Identifier);
        }

        [TestMethod]
        public void Order_With_DiscountAmount_And_GiftCardAmount()
        {
            const int spendAmountCents = 10;

            MerchantFundedCreditResponse creditResponse = Api.GetMerchantFundedCredit(AccessToken.Token,
                                                                                      LevelUpTestConfiguration.Current
                                                                                                              .Merchant_LocationId_Visible,
                                                                                      LevelUpTestConfiguration.Current
                                                                                                              .User_PaymentToken);

            OrderResponse response = PlaceOrder(AccessToken.Token,
                                                spendAmount: spendAmountCents,
                                                appliedDiscountAmount: 5,
                                                availableGiftCardAmount: creditResponse.GiftCardAmount,
                                                qrCodeToUse: LevelUpTestConfiguration.Current.User_PaymentToken);

            Assert.IsNotNull(response);
            Assert.AreEqual(spendAmountCents, response.SpendAmount);

            Api.RefundOrder(AccessToken.Token, response.Identifier);
        }

        [TestMethod]
        public void Order_With_DiscountAmount_And_GiftCardAmount_PartialAuthAllowed()
        {
            const int excessAmountCents = 10;
            string qrCodeToUse = LevelUpTestConfiguration.Current.User_PaymentToken;

            MerchantFundedCreditResponse creditResponse = Api.GetMerchantFundedCredit(AccessToken.Token,
                                                                                      LevelUpTestConfiguration.Current.Merchant_LocationId_Visible,
                                                                                      qrCodeToUse);
            int discountToApply = Math.Min(creditResponse.DiscountAmount, 5);
            int spendAmountCents = creditResponse.DiscountAmount + creditResponse.GiftCardAmount + excessAmountCents;
            OrderResponse response = PlaceOrder(AccessToken.Token,
                                                spendAmount: spendAmountCents,
                                                appliedDiscountAmount: discountToApply,
                                                availableGiftCardAmount: creditResponse.GiftCardAmount,
                                                qrCodeToUse: qrCodeToUse,
                                                partialAuthEnabled: true);

            Assert.IsNotNull(response);

            Api.RefundOrder(AccessToken.Token, response.Identifier);
        }

        [TestMethod]
        [ExpectedException(typeof(LevelUpApiException))]
        public void Order_With_DiscountAmount_And_GiftCardAmount_PartialAuthNotAllowed()
        {
            const int excessAmountCents = 10;
            string qrCodeToUse = LevelUpTestConfiguration.Current.User_GiftCardPaymentToken;

            MerchantFundedCreditResponse creditResponse = Api.GetMerchantFundedCredit(AccessToken.Token,
                                                                                      LevelUpTestConfiguration.Current.Merchant_LocationId_Visible,
                                                                                      qrCodeToUse);
            int discountToApply = Math.Min(creditResponse.DiscountAmount, 5);
            int spendAmountCents = creditResponse.DiscountAmount + creditResponse.GiftCardAmount + excessAmountCents;

            try
            {
                OrderResponse response = PlaceOrder(AccessToken.Token,
                                                    spendAmount: spendAmountCents,
                                                    appliedDiscountAmount: 5,
                                                    availableGiftCardAmount: creditResponse.GiftCardAmount,
                                                    qrCodeToUse: qrCodeToUse,
                                                    partialAuthEnabled: false);

                Api.RefundOrder(AccessToken.Token, response.Identifier);
                Assert.Fail("Expected Exception not thrown!");
            }
            catch (LevelUpApiException luEx)
            {
                Assert.IsTrue(((int) luEx.StatusCode).Equals((int) HttpStatusCodeExtended.UnprocessableEntity),
                              luEx.Message);
                Assert.IsFalse(string.IsNullOrEmpty(luEx.Message), luEx.StatusCode.ToString());

                throw;
            }
        }

        [TestMethod]
        public void Refund()
        {
            OrderResponse orderResponse = PlaceOrder(AccessToken.Token,
                                                     spendAmount: EXPECTED_SPEND_AMOUNT_CENTS,
                                                     qrCodeToUse: LevelUpTestConfiguration.Current.User_PaymentToken);

            Assert.IsNotNull(orderResponse);
            Assert.AreEqual(EXPECTED_SPEND_AMOUNT_CENTS, orderResponse.SpendAmount);
            Assert.AreEqual(EXPECTED_SPEND_AMOUNT_CENTS, orderResponse.Total);
            Assert.IsNotNull(orderResponse.Identifier);

            RefundResponse refundResponse = Api.RefundOrder(AccessToken.Token, orderResponse.Identifier);

            Assert.IsNotNull(refundResponse);
            Assert.IsFalse(string.IsNullOrEmpty(refundResponse.TimeOfRefund));
            Assert.AreEqual(refundResponse.TotalAmount, orderResponse.Total);
        }

        [TestMethod]
        public void Order_Details()
        {
            OrderResponse orderResponse = PlaceOrder(AccessToken.Token,
                                                     spendAmount: EXPECTED_SPEND_AMOUNT_CENTS,
                                                     exemptionAmount: 10,
                                                     qrCodeToUse: LevelUpTestConfiguration.Current.User_PaymentToken);
            Assert.IsNotNull(orderResponse);
            Assert.AreEqual(EXPECTED_SPEND_AMOUNT_CENTS, orderResponse.SpendAmount);
            Assert.AreEqual(EXPECTED_SPEND_AMOUNT_CENTS, orderResponse.Total);
            Assert.IsNotNull(orderResponse.Identifier);

            OrderDetailsResponse detailsResponse = Api.GetMerchantOrderDetails(AccessToken.Token,
                                                                               AccessToken.MerchantId.Value,
                                                                               orderResponse.Identifier);

            Assert.IsNotNull(detailsResponse);

            Api.RefundOrder(AccessToken.Token, orderResponse.Identifier);
        }

        [TestMethod]
        public void Orders_At_Location()
        {
            Location location = Api.ListLocations(AccessToken.Token, 
                                                  AccessToken.MerchantId.GetValueOrDefault(3225))[0];
            Assert.IsNotNull(location);

            bool areMorePages;
            IList<OrderDetailsResponse> orders = 
                Api.ListOrders(AccessToken.Token, location.LocationId, 1, 1, out areMorePages);

            Assert.IsNotNull(orders);
            Assert.IsTrue(orders.Count > 0);
            Assert.IsTrue(orders.Count <= 30);
            Assert.IsTrue(areMorePages);
        }

        [TestMethod]
        public void Tip_10_Percent()
        {
            const int tipPercent = 10;
            const double tipDecimal = (double)tipPercent / 100;
            const double tipFactor = 1 + tipDecimal;

            OrderResponse orderResponse = 
                PlaceOrder(AccessToken.Token,
                           spendAmount: EXPECTED_SPEND_AMOUNT_CENTS,
                           qrCodeToUse: LevelUpTestConfiguration.Current.User_PaymentTokenWith10PercentTip);

            Assert.IsNotNull(orderResponse);
            Assert.AreEqual(EXPECTED_SPEND_AMOUNT_CENTS, orderResponse.SpendAmount);
            Assert.AreEqual(tipDecimal * EXPECTED_SPEND_AMOUNT_CENTS, orderResponse.TipAmount);
            Assert.AreEqual(tipFactor * EXPECTED_SPEND_AMOUNT_CENTS, orderResponse.Total);

            Api.RefundOrder(AccessToken.Token, orderResponse.Identifier);
        }

        [TestMethod]
        public void FilterOrders_ByName()
        {
            int locationId = GetLocationIdForMerchant(AccessToken.MerchantId);

            var filteredOrders = ((LevelUpClient)Api).ListFilteredOrders(AccessToken.Token,
                                                                         locationId,
                                                                         _nameFilter,
                                                                         NUM_PAGES_TO_SEARCH);
            Assert.IsNotNull(filteredOrders);
        }

        [TestMethod]
        public void FilterOrders_ByDate()
        {
            int locationId = GetLocationIdForMerchant(AccessToken.MerchantId);

            var filteredOrders = ((LevelUpClient)Api).ListFilteredOrders(AccessToken.Token,
                                                                         locationId,
                                                                         _dateFilter);
            Assert.IsNotNull(filteredOrders);
        }

        [TestMethod]
        public void FilterOrders_ByNameAndDate()
        {
            int locationId = GetLocationIdForMerchant(AccessToken.MerchantId);

            var andFilter = new LogicalAndFilter<OrderDetailsResponse>(_nameFilter, _dateFilter);
            var filteredOrders = ((LevelUpClient)Api).ListFilteredOrders(AccessToken.Token,
                                                                         locationId,
                                                                         andFilter,
                                                                         NUM_PAGES_TO_SEARCH);
            Assert.IsNotNull(filteredOrders);
        }

        [TestMethod]
        public void FilterOrders_ByNameOrDate()
        {
            int locationId = GetLocationIdForMerchant(AccessToken.MerchantId);

            var orFilter = new LogicalOrFilter<OrderDetailsResponse>(_nameFilter, _dateFilter);
            var filteredOrders = ((LevelUpClient)Api).ListFilteredOrders(AccessToken.Token,
                                                                         locationId,
                                                                         orFilter,
                                                                         NUM_PAGES_TO_SEARCH);

            Assert.IsNotNull(filteredOrders);
        }
    }
}
