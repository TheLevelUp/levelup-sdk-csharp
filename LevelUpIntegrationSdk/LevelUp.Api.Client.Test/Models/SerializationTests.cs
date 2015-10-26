//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="SerializationTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using System.Collections.Generic;
using LevelUp.Api.Client.Models.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Test
{
    [TestClass]
    public class SerializationTests : ApiUnitTestsBase
    {
        [TestMethod]
        public void SerializeDeserialize_AccessTokenRequest()
        {
            AccessTokenRequest token = new AccessTokenRequest(LevelUpTestConfiguration.Current.ApiKey,
                                                              LevelUpTestConfiguration.Current.Username,
                                                              LevelUpTestConfiguration.Current.Password);

            AccessTokenRequest deserialized = TestUtilities.SerializeThenDeserialize<AccessTokenRequest>(token);

            Assert.IsNotNull(deserialized);
            Assert.IsTrue(TestUtilities.PublicPropertiesAreEqual<AccessTokenRequest>(token, deserialized));
        }

        [TestMethod]
        public void SerializeDeserialize_CreditCardRequest()
        {
            CreditCardRequest request = new CreditCardRequest("ABC123", "DEF456", "HIJ789", "KLM000", "02110");

            CreditCardRequest deserialized = TestUtilities.SerializeThenDeserialize<CreditCardRequest>(request);

            Assert.IsNotNull(deserialized);
            Assert.IsTrue(TestUtilities.PublicPropertiesAreEqual<CreditCardRequest>(request, deserialized));
        }

        [TestMethod]
        public void SerializeDeserialize_DetachedRefund()
        {
            DetachedRefund refund = new DetachedRefund(TestData.Valid.INVISIBLE_LOCATION_ID, 
                                                       LevelUpTestConfiguration.Current.QrData,
                                                       creditAmountCents: 1);

            DetachedRefund deserialized = TestUtilities.SerializeThenDeserialize(refund);

            Assert.IsNotNull(deserialized);
            Assert.IsTrue(TestUtilities.PublicPropertiesAreEqual(refund, deserialized));
        }

        [TestMethod]
        public void SerializeDeserialize_Item()
        {
            Item item = new Item("TestItem", "TestDescription", "123456", "987654", "Test", 1, 1);

            Item deserialized = TestUtilities.SerializeThenDeserialize<Item>(item);

            Assert.IsNotNull(deserialized);
            Assert.IsTrue(TestUtilities.PublicPropertiesAreEqual<Item>(item, deserialized));
        }

        [TestMethod]
        public void SerializeDeserialize_Order()
        {
            List<Item> items = new List<Item>();
            
            Item mod = new Item("Extra Testing", "Test Modifier", "111", null, "MODS", 1, 1, 1);
            items.Add(new Item("Test Item", "This is a delicious test item", "abc123", null, "MISC", 5, 5, 1, new List<Item>{mod}));
            
            Order order = new Order(TestData.Valid.INVISIBLE_LOCATION_ID, 
                                    LevelUpTestConfiguration.Current.QrData,
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
        public void SerializeDeserialize_Refund()
        {
            Refund refund = new Refund("MgrConfirm");

            Refund deserialized = TestUtilities.SerializeThenDeserialize<Refund>(refund);

            Assert.IsNotNull(deserialized);
            Assert.IsTrue(TestUtilities.PublicPropertiesAreEqual<Refund>(refund, deserialized));
        }

        [TestMethod]
        public void SerializeDeserialize_GiftCardAddValue()
        {
            GiftCardAddValueRequest gcAdd = new GiftCardAddValueRequest(LevelUpTestConfiguration.Current.QrData,
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

            GiftCardAddValueRequest deserialized = TestUtilities.SerializeThenDeserialize<GiftCardAddValueRequest>(gcAdd);

            Assert.IsNotNull(deserialized);
            Assert.IsTrue(TestUtilities.PublicPropertiesAreEqual(gcAdd, deserialized));
        }

        [TestMethod]
        public void SerializeDeserialize_GiftCardRemoveValue()
        {
            GiftCardRemoveValueRequest gcAdd = new GiftCardRemoveValueRequest(LevelUpTestConfiguration.Current.QrData, 10);

            GiftCardRemoveValueRequest deserialized = TestUtilities.SerializeThenDeserialize<GiftCardRemoveValueRequest>(gcAdd);

            Assert.IsNotNull(deserialized);
            Assert.IsTrue(TestUtilities.PublicPropertiesAreEqual(gcAdd, deserialized));
        }

        [TestMethod]
        public void SerializeDeserialize_MerchantCreditQuery()
        {
            List<Item> items = new List<Item>();
            
            Item mod = new Item("Extra Testing", "Test Modifier", "111", null, "MODS", 1, 1, 1);
            items.Add(new Item("Test Item", "This is a delicious test item", "abc123", null, "MISC", 5, 5, 1, new List<Item>{mod}));

            MerchantCreditQuery creditQuery = new MerchantCreditQuery(LevelUpTestConfiguration.Current.QrData,
                                                                      "12345",
                                                                      items);

            MerchantCreditQuery deserialized = TestUtilities.SerializeThenDeserialize<MerchantCreditQuery>(creditQuery);

            Assert.IsNotNull(deserialized);
            Assert.IsTrue(TestUtilities.PublicPropertiesAreEqual(creditQuery, deserialized));
        }
    }
}
