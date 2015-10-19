//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="MoneyConversionTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

namespace LevelUp.Api.Utilities.Test
{
    [TestClass]
    public class MoneyConversionTests
    {
        [TestClass]
        public class ToDollars
        {
            [TestMethod]
            public void OneDollar()
            {
                decimal dollars = Money.ToDollars("100");
                decimal dollarsInt = Money.ToDollars(100);
                decimal dollarsLong = Money.ToDollars(100L);
                Assert.AreEqual(1.00m, dollars);
                Assert.AreEqual(1.00m, dollarsInt);
                Assert.AreEqual(1.00m, dollarsLong);
            }
            [TestMethod]
            public void UnderOneDollar()
            {
                decimal dollars = Money.ToDollars("76");
                decimal dollarsInt = Money.ToDollars(76);
                decimal dollarsLong = Money.ToDollars(76L);
                Assert.AreEqual(0.76m, dollars);
                Assert.AreEqual(0.76m, dollarsInt);
                Assert.AreEqual(0.76m, dollarsLong);
            }
            [TestMethod]
            public void OverOneDollar()
            {
                decimal dollars = Money.ToDollars("103");
                decimal dollarsInt = Money.ToDollars(103);
                decimal dollarsLong = Money.ToDollars(103L);
                Assert.AreEqual(1.03m, dollars);
                Assert.AreEqual(1.03m, dollarsInt);
                Assert.AreEqual(1.03m, dollarsLong);
            }
            [TestMethod]
            public void OneCent()
            {
                decimal dollars = Money.ToDollars("1");
                decimal dollarsInt = Money.ToDollars(1);
                decimal dollarsLong = Money.ToDollars(1L);
                Assert.AreEqual(0.01m, dollars);
                Assert.AreEqual(0.01m, dollarsInt);
                Assert.AreEqual(0.01m, dollarsLong);
            }
            [TestMethod]
            public void ZeroCents()
            {
                decimal dollars = Money.ToDollars("0");
                decimal dollarsInt = Money.ToDollars(0);
                decimal dollarsLong = Money.ToDollars(0L);
                Assert.AreEqual(0.00m, dollars);
                Assert.AreEqual(0.00m, dollarsInt);
                Assert.AreEqual(0.00m, dollarsLong);
            }
            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            [TestMethod]
            public void BadlyFormattedNumber()
            {
                Money.ToDollars("10qr3");
            }

            [TestMethod]
            public void LargeValue()
            {
                Money.ToDollars(int.MaxValue);
                Money.ToDollars(int.MaxValue.ToString());
            }
        }

        [TestClass]
        public class ToCents
        {
            [TestMethod]
            public void OneDollar()
            {
                int cents = Money.ToCents("1.00");
                int centsDec = Money.ToCents(1.00m);
                Assert.AreEqual(100, cents);
                Assert.AreEqual(100, centsDec);
            }
            [TestMethod]
            public void UnderOneDollar()
            {
                int cents = Money.ToCents("0.76");
                int centsDec = Money.ToCents(0.76m);
                Assert.AreEqual(76, cents);
                Assert.AreEqual(76, centsDec);
            }
            [TestMethod]
            public void OverOneDollar()
            {
                int cents = Money.ToCents("1.03");
                int centsDec = Money.ToCents(1.03m);
                Assert.AreEqual(103, cents);
                Assert.AreEqual(103, centsDec);
            }
            [TestMethod]
            public void OneCent()
            {
                int cents = Money.ToCents("0.01");
                int centsDec = Money.ToCents(0.01m);
                Assert.AreEqual(1, cents);
                Assert.AreEqual(1, centsDec);
            }
            [TestMethod]
            public void ZeroCents()
            {
                int cents = Money.ToCents("0");
                int centsDec = Money.ToCents(0);
                Assert.AreEqual(0, cents);
                Assert.AreEqual(0, centsDec);
            }

            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            [TestMethod]
            public void BadlyFormattedNumber()
            {
                Money.ToCents("10qr3");
            }

            [TestMethod]
            public void Null()
            {
                int? nullableCents = Money.ToCents(default(int?));
                Assert.IsNull(nullableCents);
            }
        }

        [TestClass]
        public class ToCurrencyString
        {
            [TestMethod]
            public void OneDollar()
            {
                Money.ToCurrencyString(1.00m).Should().BeEquivalentTo("$1.00");
            }

            [TestMethod]
            public void UnderOneDollar()
            {
                Money.ToCurrencyString(0.25m).Should().BeEquivalentTo("$0.25");
            }

            [TestMethod]
            public void OverOneDollar()
            {
                Money.ToCurrencyString(6.54m).Should().BeEquivalentTo("$6.54");
            }

            [TestMethod]
            public void OverTenDollars()
            {
                Money.ToCurrencyString(99.95m).Should().BeEquivalentTo("$99.95");
            }

            [TestMethod]
            public void Over100Dollars()
            {
                Money.ToCurrencyString(123.45m).Should().BeEquivalentTo("$123.45");
            }

            [TestMethod]
            public void OneCent()
            {
                Money.ToCurrencyString(0.01m).Should().BeEquivalentTo("$0.01");
            }

            [TestMethod]
            public void ZeroCents()
            {
                Money.ToCurrencyString(0m).Should().BeEquivalentTo("$0.00");
            }

            [TestMethod]
            public void NegativeFiveDollars()
            {
                Money.ToCurrencyString(-5.55m).Should().BeEquivalentTo("-$5.55");
            }

            [TestMethod]
            public void ExtraPrecision()
            {
                Money.ToCurrencyString(1.234m).Should().BeEquivalentTo("$1.23");
            }

            [TestMethod]
            public void ExtraPrecisionWithRoundUp()
            {
                Money.ToCurrencyString(1.235m).Should().BeEquivalentTo("$1.24");
            }
        }
    }
}
