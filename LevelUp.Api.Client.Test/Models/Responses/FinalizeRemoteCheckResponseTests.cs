#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="FinalizeRemoteCheckResponseTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using FluentAssertions;
using LevelUp.Api.Client.Models.Responses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Test.Models.Responses
{
    [TestClass]
    public class FinalizeRemoteCheckResponseTests : ModelTestsBase
    {
        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.FunctionalTests)]
        public void Deserialize()
        {
            string levelUpOrderId = Guid.NewGuid().ToString("N");
            const int totalGift = 5432;
            const int giftTip = 123;
            const int tip = 1212;
            const int spend = 9999;
            const int total = spend + tip;

            string json = CreateSerializedObjectJsonString(totalGift, giftTip, levelUpOrderId, spend, tip, total);

            FinalizeRemoteCheckResponse response = JsonConvert.DeserializeObject<FinalizeRemoteCheckResponse>(json);
            response.Should().NotBeNull();
            response.OrderIdentifier.ShouldBeEquivalentTo(levelUpOrderId);
            response.GiftCardCreditTotalAmount.Should().Be(totalGift);
            response.GiftCardCreditTipAmount.Should().Be(giftTip);
            response.SpendAmount.Should().Be(spend);
            response.TipAmount.Should().Be(tip);
            response.TotalAmount.Should().Be(total);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.FunctionalTests)]
        public void Deserialize_NoGiftTip()
        {
            string levelUpOrderId = Guid.NewGuid().ToString("N");
            const int totalGift = 300;
            const int giftTip = 0;
            const int tip = 209;
            const int spend = 4545;
            const int total = spend + tip;

            string json = CreateSerializedObjectJsonString(totalGift, giftTip, levelUpOrderId, spend, tip, total);

            FinalizeRemoteCheckResponse response = JsonConvert.DeserializeObject<FinalizeRemoteCheckResponse>(json);
            response.Should().NotBeNull();
            response.OrderIdentifier.ShouldBeEquivalentTo(levelUpOrderId);
            response.GiftCardCreditTotalAmount.Should().Be(totalGift);
            response.GiftCardCreditTipAmount.Should().Be(giftTip);
            response.SpendAmount.Should().Be(spend);
            response.TipAmount.Should().Be(tip);
            response.TotalAmount.Should().Be(total);
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.FunctionalTests)]
        public void Deserialize_NoGift()
        {
            string levelUpOrderId = Guid.NewGuid().ToString("N");
            const int totalGift = 0;
            const int giftTip = 0;
            const int tip = 159;
            const int spend = 6000;
            const int total = spend + tip;

            string json = CreateSerializedObjectJsonString(totalGift, giftTip, levelUpOrderId, spend, tip, total);

            FinalizeRemoteCheckResponse response = JsonConvert.DeserializeObject<FinalizeRemoteCheckResponse>(json);

            response.Should().NotBeNull();
            response.OrderIdentifier.ShouldBeEquivalentTo(levelUpOrderId);
            response.GiftCardCreditTotalAmount.Should().Be(totalGift);
            response.GiftCardCreditTipAmount.Should().Be(giftTip);
            response.SpendAmount.Should().Be(spend);
            response.TipAmount.Should().Be(tip);
            response.TotalAmount.Should().Be(total);
        }

        private string CreateSerializedObjectJsonString(int giftCardTotal,
                                                        int giftCardTip,
                                                        string orderUuid,
                                                        int spendAmount,
                                                        int tipAmount,
                                                        int totalAmount)
        {
            return "{ \"order\":{\"gift_card_total_amount\": " + giftCardTotal +
                   ",\"gift_card_tip_amount\":" + giftCardTip +
                   ",\"uuid\": \"" + orderUuid + "\",\"spend_amount\": " + spendAmount +
                   ",\"tip_amount\": " + tipAmount +
                   ",\"total_amount\": " + totalAmount + "}}";
        }
    }
}
