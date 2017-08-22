#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="SerializationTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using System.Collections.Generic;
using LevelUp.Api.Client.Models.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LevelUp.Api.Client.Test
{
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.UnitTests)]
        public void SerializeDeserialize_AccessTokenRequest()
        {
            AccessTokenRequestBody token = new AccessTokenRequestBody("54321 This is an example of a client id 12345",
                                                              "test_merchant_username@thelevelup.com",
                                                              "T3$tM3rch@ntP@$$w0rd_12345");

            AccessTokenRequestBody deserialized = TestUtilities.SerializeThenDeserialize<AccessTokenRequestBody>(token);

            Assert.IsNotNull(deserialized);
            Assert.IsTrue(TestUtilities.PublicPropertiesAreEqual<AccessTokenRequestBody>(token, deserialized));
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.UnitTests)]
        public void SerializeDeserialize_CreditCardRequest()
        {
            CreateCreditCardRequestBody request = new CreateCreditCardRequestBody("ABC123", "DEF456", "HIJ789", "KLM000", "02110");

            CreateCreditCardRequestBody deserialized = TestUtilities.SerializeThenDeserialize<CreateCreditCardRequestBody>(request);

            Assert.IsNotNull(deserialized);
            Assert.IsTrue(TestUtilities.PublicPropertiesAreEqual<CreateCreditCardRequestBody>(request, deserialized));
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.UnitTests)]
        public void SerializeDeserialize_DetachedRefund()
        {
            const int testLocationId = 3141592;
            
            DetachedRefundRequestBody refund = new DetachedRefundRequestBody(testLocationId, 
                                                       "LU6789-test-payment-token-data-9876LU",
                                                       creditAmountCents: 1);

            DetachedRefundRequestBody deserialized = TestUtilities.SerializeThenDeserialize(refund);

            Assert.IsNotNull(deserialized);
            Assert.IsTrue(TestUtilities.PublicPropertiesAreEqual(refund, deserialized));
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.UnitTests)]
        public void SerializeDeserialize_Item()
        {
            Item item = new Item("TestItem", "TestDescription", "123456", "987654", "Test", 1, 1);

            Item deserialized = TestUtilities.SerializeThenDeserialize<Item>(item);

            Assert.IsNotNull(deserialized);
            Assert.IsTrue(TestUtilities.PublicPropertiesAreEqual<Item>(item, deserialized));
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.UnitTests)]
        public void SerializeDeserialize_Order()
        {
            List<Item> items = new List<Item>();
            
            Item mod = new Item("Extra Testing", "Test Modifier", "111", null, "MODS", 1, 1, 1);
            items.Add(new Item("Test Item", "This is a delicious test item", "abc123", null, "MISC", 5, 5, 1, new List<Item>{mod}));
            
            Order order = new Order(54321, 
                                    "LU0011_This_is_my_qr_code_1100LU",
                                    1, 
                                    null,
                                    null, 
                                    0, 
                                    "Register 0",
                                    "Cashier 1",
                                    "check # 123", 
                                    true,
                                    items);

            Order deserialized = TestUtilities.SerializeThenDeserialize<Order>(order);

            Assert.IsNotNull(deserialized);
            Assert.IsTrue(TestUtilities.PublicPropertiesAreEqual<Order>(order, deserialized));
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.UnitTests)]
        public void SerializeDeserialize_Refund()
        {
            RefundRequestBody refund = new RefundRequestBody("MgrConfirm");

            RefundRequestBody deserialized = TestUtilities.SerializeThenDeserialize<RefundRequestBody>(refund);

            Assert.IsNotNull(deserialized);
            Assert.IsTrue(TestUtilities.PublicPropertiesAreEqual<RefundRequestBody>(refund, deserialized));
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.UnitTests)]
        public void SerializeDeserialize_GiftCardAddValue()
        {
            GiftCardAddValueRequestBody gcAdd = new GiftCardAddValueRequestBody("LU0123TestPaymentTokenData3210LU",
                                                                        10,
                                                                        123,
                                                                        "ATestThisIs",
                                                                        new List<string>()
                                                                            {
                                                                                "Cash",
                                                                                "Credit - Visa",
                                                                                "Cheque",
                                                                                "LevelUp",
                                                                                "LevelUp",
                                                                                "Barter"
                                                                            },
                                                                        "abc1234567890def");

            GiftCardAddValueRequestBody deserialized = TestUtilities.SerializeThenDeserialize<GiftCardAddValueRequestBody>(gcAdd);

            Assert.IsNotNull(deserialized);
            Assert.IsTrue(TestUtilities.PublicPropertiesAreEqual(gcAdd, deserialized));
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.UnitTests)]
        public void SerializeDeserialize_GiftCardRemoveValue()
        {
            GiftCardRemoveValueRequestBody gcAdd = new GiftCardRemoveValueRequestBody("LU_88_TestConsumerQrData_88_LU",
                                                                                      10);

            GiftCardRemoveValueRequestBody deserialized =
                TestUtilities.SerializeThenDeserialize<GiftCardRemoveValueRequestBody>(gcAdd);

            Assert.IsNotNull(deserialized);
            Assert.IsTrue(TestUtilities.PublicPropertiesAreEqual(gcAdd, deserialized));
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Http.Test.TestCategory.UnitTests)]
        public void SerializeDeserialize_MerchantCreditQuery()
        {
            List<Item> items = new List<Item>();
            
            Item mod = new Item("Extra Testing", "Test Modifier", "111", null, "MODS", 1, 1, 1);
            items.Add(new Item("Test Item", "This is a delicious test item", "abc123", null, "MISC", 5, 5, 1, new List<Item>{mod}));

            MerchantCreditQueryRequestBody creditQuery = new MerchantCreditQueryRequestBody("LlulululululululuLU",
                                                                                            "12345",
                                                                                            items);

            MerchantCreditQueryRequestBody deserialized = TestUtilities.SerializeThenDeserialize<MerchantCreditQueryRequestBody>(creditQuery);

            Assert.IsNotNull(deserialized);
            Assert.IsTrue(TestUtilities.PublicPropertiesAreEqual(creditQuery, deserialized));
        }
    }
}
