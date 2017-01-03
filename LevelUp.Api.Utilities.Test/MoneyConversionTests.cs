#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="MoneyConversionTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LevelUp.Api.Utilities.Test
{
    [TestClass]
    public class MoneyConversionTests
    {
        [TestClass]
        public class ToDollars
        {
            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void OneDollar()
            {
                const decimal ONE_DOLLAR = 1.00m;

                decimal dollars = Money.ToDollars("100");
                decimal dollarsInt = Money.ToDollars(100);
                decimal dollarsLong = Money.ToDollars(100L);
                decimal? dollarsNull = Money.ToDollars((int?) 100);

                Assert.AreEqual(ONE_DOLLAR, dollars);
                Assert.AreEqual(ONE_DOLLAR, dollarsInt);
                Assert.AreEqual(ONE_DOLLAR, dollarsLong);
                Assert.AreEqual(ONE_DOLLAR, dollarsNull);
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void UnderOneDollar()
            {
                const decimal SEVENTY_SIX_CENTS = 0.76m;

                decimal dollars = Money.ToDollars("76");
                decimal dollarsInt = Money.ToDollars(76);
                decimal dollarsLong = Money.ToDollars(76L);
                decimal? dollarsNull = Money.ToDollars((int?)76);

                Assert.AreEqual(SEVENTY_SIX_CENTS, dollars);
                Assert.AreEqual(SEVENTY_SIX_CENTS, dollarsInt);
                Assert.AreEqual(SEVENTY_SIX_CENTS, dollarsLong);
                Assert.AreEqual(SEVENTY_SIX_CENTS, dollarsNull);
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void OverOneDollar()
            {
                const decimal ONE_DOLLAR_THREE_CENTS = 1.03m;

                decimal dollars = Money.ToDollars("103");
                decimal dollarsInt = Money.ToDollars(103);
                decimal dollarsLong = Money.ToDollars(103L);
                decimal? dollarsNull = Money.ToDollars((int?)103);

                Assert.AreEqual(ONE_DOLLAR_THREE_CENTS, dollars);
                Assert.AreEqual(ONE_DOLLAR_THREE_CENTS, dollarsInt);
                Assert.AreEqual(ONE_DOLLAR_THREE_CENTS, dollarsLong);
                Assert.AreEqual(ONE_DOLLAR_THREE_CENTS, dollarsNull);
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void OneCent()
            {
                const decimal ONE_CENT = 0.01m;

                decimal dollars = Money.ToDollars("1");
                decimal dollarsInt = Money.ToDollars(1);
                decimal dollarsLong = Money.ToDollars(1L);
                decimal? dollarsNull = Money.ToDollars((int?)1);

                Assert.AreEqual(ONE_CENT, dollars);
                Assert.AreEqual(ONE_CENT, dollarsInt);
                Assert.AreEqual(ONE_CENT, dollarsLong);
                Assert.AreEqual(ONE_CENT, dollarsNull);
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void ZeroCents()
            {
                const decimal ZERO = 0.00m;

                decimal dollars = Money.ToDollars("0");
                decimal dollarsInt = Money.ToDollars(0);
                decimal dollarsLong = Money.ToDollars(0L);
                decimal? dollarsNull = Money.ToDollars((int?)0);

                Assert.AreEqual(ZERO, dollars);
                Assert.AreEqual(ZERO, dollarsInt);
                Assert.AreEqual(ZERO, dollarsLong);
                Assert.AreEqual(ZERO, dollarsNull);
            }

            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void BadlyFormattedNumber()
            {
                Money.ToDollars("10qr3");
            }

            [TestMethod]
            public void Null()
            {
                decimal? nullableDollars = Money.ToDollars((int?)null);
                Assert.IsNull(nullableDollars);
            }
        }

        [TestClass]
        public class ToCents
        {
            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void OneDollar()
            {
                const int ONE_DOLLAR = 100;

                int cents = Money.ToCents("1.00");
                int centsDec = Money.ToCents(1.00m);
                int? centsNull = Money.ToCents((decimal?) 1.00m);
                Assert.AreEqual(ONE_DOLLAR, cents);
                Assert.AreEqual(ONE_DOLLAR, centsDec);
                Assert.AreEqual(ONE_DOLLAR, centsNull);
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void UnderOneDollar()
            {
                const int SEVENTY_SIX_CENTS = 76;

                int cents = Money.ToCents("0.76");
                int centsDec = Money.ToCents(0.76m);
                int? centsNull = Money.ToCents((decimal?)0.76m);

                Assert.AreEqual(SEVENTY_SIX_CENTS, cents);
                Assert.AreEqual(SEVENTY_SIX_CENTS, centsDec);
                Assert.AreEqual(SEVENTY_SIX_CENTS, centsNull);
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void OverOneDollar()
            {
                const int ONE_DOLLAR_THREE_CENTS = 103;

                int cents = Money.ToCents("1.03");
                int centsDec = Money.ToCents(1.03m);
                int? centsNull = Money.ToCents((decimal?)1.03m);

                Assert.AreEqual(ONE_DOLLAR_THREE_CENTS, cents);
                Assert.AreEqual(ONE_DOLLAR_THREE_CENTS, centsDec);
                Assert.AreEqual(ONE_DOLLAR_THREE_CENTS, centsNull);
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void OneCent()
            {
                const int ONE_CENT = 1;

                int cents = Money.ToCents("0.01");
                int centsDec = Money.ToCents(0.01m);
                int? centsNull = Money.ToCents((decimal?)0.01m);

                Assert.AreEqual(ONE_CENT, cents);
                Assert.AreEqual(ONE_CENT, centsDec);
                Assert.AreEqual(ONE_CENT, centsNull);
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void ZeroCents()
            {
                const int ZERO = 0;

                int cents = Money.ToCents("0");
                int centsDec = Money.ToCents(0m);
                int? centsNull = Money.ToCents((decimal?)0m);

                Assert.AreEqual(ZERO, cents);
                Assert.AreEqual(ZERO, centsDec);
                Assert.AreEqual(ZERO, centsNull);
            }

            [ExpectedException(typeof(ArgumentOutOfRangeException))]
            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void BadlyFormattedNumber()
            {
                Money.ToCents("10qr3");
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void Null()
            {
                int? nullableCents = Money.ToCents((decimal?)null);
                Assert.IsNull(nullableCents);
            }
        }

        [TestClass]
        public class ToCurrencyString
        {
            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void OneDollar()
            {
                Money.ToCurrencyString(1.00m).Should().BeEquivalentTo("$1.00");
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void UnderOneDollar()
            {
                Money.ToCurrencyString(0.25m).Should().BeEquivalentTo("$0.25");
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void OverOneDollar()
            {
                Money.ToCurrencyString(6.54m).Should().BeEquivalentTo("$6.54");
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void OverTenDollars()
            {
                Money.ToCurrencyString(99.95m).Should().BeEquivalentTo("$99.95");
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void Over100Dollars()
            {
                Money.ToCurrencyString(123.45m).Should().BeEquivalentTo("$123.45");
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void OneCent()
            {
                Money.ToCurrencyString(0.01m).Should().BeEquivalentTo("$0.01");
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void ZeroCents()
            {
                Money.ToCurrencyString(0m).Should().BeEquivalentTo("$0.00");
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void NegativeFiveDollars()
            {
                Money.ToCurrencyString(-5.55m).Should().BeEquivalentTo("-$5.55");
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void ExtraPrecision()
            {
                Money.ToCurrencyString(1.234m).Should().BeEquivalentTo("$1.23");
            }

            [TestMethod]
            [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
            public void ExtraPrecisionWithRoundUp()
            {
                Money.ToCurrencyString(1.235m).Should().BeEquivalentTo("$1.24");
            }
        }
    }
}
