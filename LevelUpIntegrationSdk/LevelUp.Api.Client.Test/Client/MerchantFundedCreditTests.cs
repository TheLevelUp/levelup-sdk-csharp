//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="MerchantFundedCreditTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using LevelUp.Api.Client.Models.Requests;
using LevelUp.Api.Client.Models.Responses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace LevelUp.Api.Client.Test
{
    [TestClass]
    [DeploymentItem("LevelUpBaseUri.txt")]
    [DeploymentItem("test_config_settings.xml")]
    public class MerchantFundedCreditTests : ApiUnitTestsBase
    {
        private List<Item> _items;
        private List<Item> _discountEligibleItems;

        public MerchantFundedCreditTests()
        {
            _items = new List<Item>();

            Item mod = new Item("Extra Testing", "Test Modifier", "111", null, "MODS", 1, 1, 1);
            _items.Add(new Item("Test Item", "This is a delicious test item", "abc123", null, "MISC", 5, 5, 1, new List<Item> { mod }));

            _discountEligibleItems = new List<Item>
                {
                    //This item is configured as a "type 1" discount where the customer is entitled to up to $X in 
                    //discount if some number of items of this type appear on the check.
                    new Item("10057", "babersgrub - id: 718500", "01230000007", "01230000007", "Test Discount items", 50, 51, 2, null),
                    //This item is configured as a "type 2" discount where the customer is entitled to up to $Y in 
                    //discount if this item appears on the check. Only 1 item per time period may claim the discount
                    new Item("Soccer ball", "nedsgrub - id: 718354", string.Empty, string.Empty, "Test Discount items", 123, 321, 1, null),
                    
                    //In all cases, the actual discount amount returned from the MFC endpoint should be the eligible
                    //discount amount. That is, if $X is available and only $Y (where Y < X) is sent as 
                    //charged_price * quantity, (note: discount to apply to items is calculated based on charged_price/quantity)
                    //only $Y should be added to the other discounts available for these items.
                };
        }

        [TestMethod]
        public void MerchantFundedCredit()
        {
            MerchantFundedCreditResponse creditResponse = Api.GetMerchantFundedCredit(AccessToken.Token,
                                                                                      TestData.Valid.POS_LOCATION_ID,
                                                                                      LevelUpTestConfiguration.Current
                                                                                                              .QrData);

            Assert.IsNotNull(creditResponse);
            Assert.IsNotNull(creditResponse.DiscountAmount);
            Assert.IsNotNull(creditResponse.GiftCardAmount);
        }

        [TestMethod]
        public void MerchantFundedCredit_WithItems_NoDiscount()
        {
            //Basic discount get. No item data
            MerchantFundedCreditResponse creditResponse = Api.GetMerchantFundedCredit(AccessToken.Token,
                                                                                      TestData.Valid.POS_LOCATION_ID,
                                                                                      LevelUpTestConfiguration.Current
                                                                                                              .QrData);

            Assert.IsNotNull(creditResponse);
            Assert.IsNotNull(creditResponse.DiscountAmount);
            Assert.IsNotNull(creditResponse.GiftCardAmount);

            MerchantFundedCreditResponse itemCreditResponse = Api.GetMerchantFundedCredit(AccessToken.Token,
                                                                                          TestData.Valid.POS_LOCATION_ID,
                                                                                          LevelUpTestConfiguration
                                                                                              .Current.QrData,
                                                                                          "Test the items without discount eligible item",
                                                                                          _items);

            Assert.IsNotNull(itemCreditResponse);
            Assert.IsNotNull(itemCreditResponse.DiscountAmount);
            Assert.IsNotNull(itemCreditResponse.GiftCardAmount);

            itemCreditResponse.DiscountAmount.Should().Be(creditResponse.DiscountAmount);
            itemCreditResponse.GiftCardAmount.Should().Be(creditResponse.GiftCardAmount);
        }

        [TestMethod]
        public void MerchantFundedCredit_WithItems_Discount_Type1()
        {
            Item itemWithDiscountType1Config = _discountEligibleItems[0];

            MerchantFundedCreditResponse creditResponse = Api.GetMerchantFundedCredit(AccessToken.Token,
                                                                                      TestData.Valid.POS_LOCATION_ID,
                                                                                      LevelUpTestConfiguration.Current
                                                                                                              .QrData);

            Assert.IsNotNull(creditResponse);
            Assert.IsNotNull(creditResponse.DiscountAmount);
            Assert.IsNotNull(creditResponse.GiftCardAmount);

            List<Item> itemsIncludingDiscountEligible = new List<Item>(_items) {itemWithDiscountType1Config};

            MerchantFundedCreditResponse itemCreditResponse = Api.GetMerchantFundedCredit(AccessToken.Token,
                                                                                  TestData.Valid.POS_LOCATION_ID,
                                                                                  LevelUpTestConfiguration
                                                                                      .Current.QrData,
                                                                                  "Test the items with discount eligible item",
                                                                                  itemsIncludingDiscountEligible);

            Assert.IsNotNull(itemCreditResponse);
            Assert.IsNotNull(itemCreditResponse.DiscountAmount);
            Assert.IsNotNull(itemCreditResponse.GiftCardAmount);

            itemCreditResponse.DiscountAmount.Should().BeGreaterThan(creditResponse.DiscountAmount);
            itemCreditResponse.GiftCardAmount.Should().Be(creditResponse.GiftCardAmount);

            int discountDifference = (itemCreditResponse.DiscountAmount - creditResponse.DiscountAmount);
            discountDifference.Should().Be(itemWithDiscountType1Config.ChargedPrice);
        }

        [TestMethod]
        public void MerchantFundedCredit_WithItems_Discount_Type2()
        {
            Item itemWithDiscountType2Config = _discountEligibleItems[1];

            MerchantFundedCreditResponse creditResponse = Api.GetMerchantFundedCredit(AccessToken.Token,
                                                                                      TestData.Valid.POS_LOCATION_ID,
                                                                                      LevelUpTestConfiguration.Current
                                                                                                              .QrData);

            Assert.IsNotNull(creditResponse);
            Assert.IsNotNull(creditResponse.DiscountAmount);
            Assert.IsNotNull(creditResponse.GiftCardAmount);

            List<Item> itemsIncludingDiscountEligible = new List<Item>(_items) { itemWithDiscountType2Config };

            MerchantFundedCreditResponse itemCreditResponse = Api.GetMerchantFundedCredit(AccessToken.Token,
                                                                                  TestData.Valid.POS_LOCATION_ID,
                                                                                  LevelUpTestConfiguration
                                                                                      .Current.QrData,
                                                                                  "Test the items with discount eligible item",
                                                                                  itemsIncludingDiscountEligible);

            Assert.IsNotNull(itemCreditResponse);
            Assert.IsNotNull(itemCreditResponse.DiscountAmount);
            Assert.IsNotNull(itemCreditResponse.GiftCardAmount);

            itemCreditResponse.DiscountAmount.Should().BeGreaterThan(creditResponse.DiscountAmount);
            itemCreditResponse.GiftCardAmount.Should().Be(creditResponse.GiftCardAmount);

            int discountDifference = (itemCreditResponse.DiscountAmount - creditResponse.DiscountAmount);
            discountDifference.Should().Be(itemWithDiscountType2Config.ChargedPrice);
        }

        [TestMethod]
        public void MerchantFundedCredit_WithItems()
        {
            Item itemWithDiscountType1Config = _discountEligibleItems[0];
            Item itemWithDiscountType2Config = _discountEligibleItems[1];

            MerchantFundedCreditResponse creditResponse = Api.GetMerchantFundedCredit(AccessToken.Token,
                                                                                      TestData.Valid.POS_LOCATION_ID,
                                                                                      LevelUpTestConfiguration.Current
                                                                                                              .QrData);

            Assert.IsNotNull(creditResponse);
            Assert.IsNotNull(creditResponse.DiscountAmount);
            Assert.IsNotNull(creditResponse.GiftCardAmount);

            List<Item> itemsIncludingDiscountEligible = new List<Item>(_items) { itemWithDiscountType1Config };

            MerchantFundedCreditResponse itemCreditResponse1 = Api.GetMerchantFundedCredit(AccessToken.Token,
                                                                                  TestData.Valid.POS_LOCATION_ID,
                                                                                  LevelUpTestConfiguration
                                                                                      .Current.QrData,
                                                                                  "Test the items with discount eligible item",
                                                                                  itemsIncludingDiscountEligible);

            Assert.IsNotNull(itemCreditResponse1);
            Assert.IsNotNull(itemCreditResponse1.DiscountAmount);
            Assert.IsNotNull(itemCreditResponse1.GiftCardAmount);

            itemCreditResponse1.DiscountAmount.Should().BeGreaterThan(creditResponse.DiscountAmount);
            itemCreditResponse1.GiftCardAmount.Should().Be(creditResponse.GiftCardAmount);

            int discountDifference0 = (itemCreditResponse1.DiscountAmount - creditResponse.DiscountAmount);
            discountDifference0.Should().Be(itemWithDiscountType1Config.ChargedPrice);

            itemsIncludingDiscountEligible.Add(itemWithDiscountType2Config);

            MerchantFundedCreditResponse itemCreditResponse2 = Api.GetMerchantFundedCredit(AccessToken.Token,
                                                                      TestData.Valid.POS_LOCATION_ID,
                                                                      LevelUpTestConfiguration
                                                                          .Current.QrData,
                                                                      "Test the items with discount eligible item",
                                                                      itemsIncludingDiscountEligible);

            Assert.IsNotNull(itemCreditResponse2);
            Assert.IsNotNull(itemCreditResponse2.DiscountAmount);
            Assert.IsNotNull(itemCreditResponse2.GiftCardAmount);

            itemCreditResponse2.DiscountAmount.Should().BeGreaterThan(itemCreditResponse1.DiscountAmount);
            itemCreditResponse2.GiftCardAmount.Should().Be(creditResponse.GiftCardAmount);

            int discountDifference1 = (itemCreditResponse2.DiscountAmount - itemCreditResponse1.DiscountAmount);
            discountDifference1.Should().Be(itemWithDiscountType2Config.ChargedPrice);

            int discountDifference2 = (itemCreditResponse2.DiscountAmount - creditResponse.DiscountAmount);
            int sumOfItemChargedPrices = itemWithDiscountType2Config.ChargedPrice +
                                         itemWithDiscountType1Config.ChargedPrice;
            discountDifference2.Should().Be(sumOfItemChargedPrices);
        }

    }
}
