//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="PaymentCalculatorTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LevelUp.Api.Utilities.Test
{
    [TestClass]
    public class PaymentCalculatorTests
    {
        [TestClass]
        public class DiscountToApplyTests
        {
            [TestMethod]
            public void DiscountToApply_DiscountExceedsAmountDue()
            {
                const decimal merchantCredit = 10m;
                const decimal amountDueBeforeTax = 5m;

                PaymentCalculator.CalculateDiscountToApply(merchantCredit, amountDueBeforeTax).Should().Be(5);
            }

            [TestMethod]
            public void DiscountToApply_DiscountExceedsAmountDue_Tax()
            {
                const decimal merchantCredit = 10m;
                const decimal paymentAmount = 10.5m;
                const decimal amountDueIncludingTax = paymentAmount;
                const decimal taxAmount = 1m;
                const decimal expected = amountDueIncludingTax - taxAmount;

                PaymentCalculator.CalculateDiscountToApply(merchantCredit,
                                                           paymentAmount,
                                                           amountDueIncludingTax,
                                                           taxAmount,
                                                           0).Should().Be(expected);
            }

            [TestMethod]
            public void DiscountToApply_SubtotalExceedsDiscount()
            {
                const decimal merchantCredit = 2m;
                const decimal amountDueBeforeTax = 10m;

                PaymentCalculator.CalculateDiscountToApply(merchantCredit, amountDueBeforeTax).Should().Be(2);
            }

            [TestMethod]
            public void DiscountToApply_SubtotalExceedsDiscount_Tax()
            {
                const decimal merchantCredit = 5m;
                const decimal paymentAmount = 12m;
                const decimal amountDueIncludingTax = paymentAmount;
                const decimal taxAmount = 1m;
                const decimal expected = merchantCredit;

                PaymentCalculator.CalculateDiscountToApply(merchantCredit,
                                                           paymentAmount,
                                                           amountDueIncludingTax,
                                                           taxAmount,
                                                           0).Should().Be(expected);
            }

            [TestMethod]
            public void DiscountToApply_DiscountAndSubtotalExceedsPayment()
            {
                const decimal merchantCredit = 10m;
                const decimal paymentAmount = 5m;
                const decimal amountDueIncludingTax = 9m;
                const decimal taxAmount = 0m;

                PaymentCalculator.CalculateDiscountToApply(merchantCredit,
                                                           paymentAmount,
                                                           amountDueIncludingTax,
                                                           taxAmount,
                                                           0).Should().Be(5);
            }

            [TestMethod]
            public void DiscountToApply_DiscountAndSubtotalExceedsPayment_Tax()
            {
                const decimal merchantCredit = 10m;
                const decimal paymentAmount = 5m;
                const decimal amountDueIncludingTax = 9m;
                const decimal taxAmount = 1m;
                const decimal expected = paymentAmount;

                PaymentCalculator.CalculateDiscountToApply(merchantCredit,
                                                           paymentAmount,
                                                           amountDueIncludingTax,
                                                           taxAmount,
                                                           0).Should().Be(expected);
            }

            [TestMethod]
            public void DiscountToApply_DiscountExeedsPaymentButNotSubtotal()
            {
                const decimal merchantCredit = 7m;
                const decimal paymentAmount = 5m;
                const decimal amountDueIncludingTax = 10m;
                const decimal taxAmount = 0m;

                PaymentCalculator.CalculateDiscountToApply(merchantCredit,
                                                           paymentAmount,
                                                           amountDueIncludingTax,
                                                           taxAmount,
                                                           0).Should().Be(5);
            }

            [TestMethod]
            public void DiscountToApply_DiscountExceedsExemptionItemsTotal()
            {
                decimal[,] values = new decimal[,]
                    {
                        //MFC, Payment, amount due with tax, tax, exempt, expected discount
                        {5m,      6m,     7m,    0m,     4m,    2m},
                        {13m, 16.07m, 16.07m, 1.05m, 10.02m,    5m},
                        {13m, 16.07m, 16.07m,    0m, 10.02m, 6.05m}
                    };

                for (int i = 0; i < values.GetLength(0); i++)
                {
                    PaymentCalculator.CalculateDiscountToApply(values[i, 0],
                                                               values[i, 1],
                                                               values[i, 2],
                                                               values[i, 3],
                                                               values[i, 4]).Should().Be(values[i, 5]);
                }
            }

            [TestMethod]
            public void DiscountToApply_ExemptionItemsTotalExceedsDiscount()
            {
                const decimal merchantCredit = 2m;
                const decimal paymentAmount = 5m;
                const decimal amountDueIncludingTax = 10m;
                const decimal taxAmount = 0m;
                const decimal totalExemption = 3m;

                PaymentCalculator.CalculateDiscountToApply(merchantCredit,
                                                           paymentAmount,
                                                           amountDueIncludingTax,
                                                           taxAmount,
                                                           totalExemption).Should().Be(2);
            }
        }

        [TestClass]
        public class PaymentsToApplyTests
        {
            private TestContext context;

            public TestContext TestContext
            {
                get { return context; }
                set { context = value; }
            }

            /// <summary>
            /// This test reads in test data from an Excel spreadsheet and executes the tests based on the data therein
            /// To run this test, you need to download and install the Microsoft Access Database Engine Redistributable
            /// Which is available here: https://www.microsoft.com/en-us/download/details.aspx?id=13255. 
            /// </summary>
            [TestMethod]
            [DeploymentItem("payment_calculator_test_data.xlsx")]
            [DataSource("System.Data.OleDb",
                "Provider=Microsoft.ACE.OLEDB.12.0;" +
                "Data Source=payment_calculator_test_data.xlsx;" +
                "Extended Properties='Excel 12.0;" +
                "HDR=Yes'", 
                "PaymentsToApply", 
                DataAccessMethod.Sequential)]
            public void PaymentsToApply_DataDriven()
            {
                int currentRowIndex = context.DataRow.Table.Rows.IndexOf(context.DataRow);
                System.Diagnostics.Debug.WriteLine("Current Data Row: " + currentRowIndex);

                decimal discountApplied = decimal.Parse(context.DataRow[0].ToString());
                decimal creditAvailable = decimal.Parse(context.DataRow[1].ToString());
                decimal spendAmount = decimal.Parse(context.DataRow[2].ToString());
                decimal tip = decimal.Parse(context.DataRow[3].ToString());
                bool discountsEnabled = Convert.ToBoolean(context.DataRow[4]);
                decimal giftCardPayment = decimal.Parse(context.DataRow[8].ToString());
                decimal totalGiftCardPayment = decimal.Parse(context.DataRow[12].ToString());
                decimal levelUpPayment = decimal.Parse(context.DataRow[13].ToString());
                decimal totalLevelUpPayment = decimal.Parse(context.DataRow[14].ToString());
                bool applyDiscount = Convert.ToBoolean(context.DataRow[18]);
                bool applyGiftCard = Convert.ToBoolean(context.DataRow[19]);
                bool applyLevelUp = Convert.ToBoolean(context.DataRow[20]);
                int numPayments = int.Parse(context.DataRow[21].ToString());

                PaymentCalculator calc = discountsEnabled
                                             ? new PaymentCalculator(discountApplied,
                                                                     creditAvailable,
                                                                     spendAmount,
                                                                     tip)
                                             : new PaymentCalculator(0,
                                                                     creditAvailable,
                                                                     spendAmount,
                                                                     tip)
                                                 {
                                                     CalculatedDiscount = discountApplied,
                                                 };

                calc.GetNumberOfPaymentMethodsRequired().Should().Be(numPayments);
                calc.ApplyLevelUpDiscount.Should().Be(applyDiscount);
                calc.ApplyLevelUpGiftCard.Should().Be(applyGiftCard);
                calc.ApplyLevelUpPayment.Should().Be(applyLevelUp);
                calc.LevelUpDiscountToApply.Should().Be(discountsEnabled ? discountApplied : 0);
                calc.GiftCardPaymentToApply.Should().Be(giftCardPayment);
                calc.TotalGiftCardPaymentToApplyIncludingTip.Should().Be(totalGiftCardPayment);
                calc.LevelUpPaymentToApply.Should().Be(levelUpPayment);
                calc.TotalLevelUpPaymentToApplyIncludingTip.Should().Be(totalLevelUpPayment);
            }

            [TestMethod]
            public void TotalAmountExceedsAvailableCredit()
            {
                const decimal discount = 2;
                const decimal credit = 3;
                const decimal spend = 15;
                const decimal tip = 1;

                decimal pay = spend - (discount + credit);

                PaymentCalculator calculator = new PaymentCalculator(discount, credit, spend, tip);

                calculator.GetNumberOfPaymentMethodsRequired().Should().Be(3);
                calculator.ApplyLevelUpDiscount.Should().Be(true);
                calculator.ApplyLevelUpGiftCard.Should().Be(true);
                calculator.ApplyLevelUpPayment.Should().Be(true);

                calculator.LevelUpDiscountToApply.Should().Be(discount);
                calculator.GiftCardPaymentToApply.Should().Be(credit);
                calculator.TotalGiftCardPaymentToApplyIncludingTip.Should().Be(credit);
                calculator.LevelUpPaymentToApply.Should().Be(pay);
                calculator.TotalLevelUpPaymentToApplyIncludingTip.Should().Be(pay + tip);
            }

            [TestMethod]
            public void DiscountCoversEntireAmount_NoTip()
            {
                const decimal discount = 2;
                const decimal credit = 3;
                const decimal spend = 2;
                const decimal tip = 0;

                PaymentCalculator calculator = new PaymentCalculator(discount, credit, spend, tip);

                calculator.GetNumberOfPaymentMethodsRequired().Should().Be(1);
                calculator.ApplyLevelUpDiscount.Should().Be(true);
                calculator.ApplyLevelUpGiftCard.Should().Be(false);
                calculator.ApplyLevelUpPayment.Should().Be(false);

                calculator.LevelUpDiscountToApply.Should().Be(discount);
                calculator.GiftCardPaymentToApply.Should().Be(0);
                calculator.TotalGiftCardPaymentToApplyIncludingTip.Should().Be(0);
                calculator.LevelUpPaymentToApply.Should().Be(0);
                calculator.TotalLevelUpPaymentToApplyIncludingTip.Should().Be(0);
            }

            [TestMethod]
            public void DiscountCoversEntireAmount_WithTip()
            {
                const decimal discount = 2;
                const decimal credit = 0;
                const decimal spend = 2;
                const decimal tip = 1;

                decimal pay = Math.Max(0, spend - (discount + credit));

                PaymentCalculator calculator = new PaymentCalculator(discount, credit, spend, tip);

                calculator.GetNumberOfPaymentMethodsRequired().Should().Be(2);
                calculator.ApplyLevelUpDiscount.Should().Be(true);
                calculator.ApplyLevelUpGiftCard.Should().Be(false);
                calculator.ApplyLevelUpPayment.Should().Be(true);

                calculator.LevelUpDiscountToApply.Should().Be(discount);
                calculator.GiftCardPaymentToApply.Should().Be(0);
                calculator.TotalGiftCardPaymentToApplyIncludingTip.Should().Be(0);
                calculator.LevelUpPaymentToApply.Should().Be(0);
                calculator.TotalLevelUpPaymentToApplyIncludingTip.Should().Be(pay + tip);
            }

            [TestMethod]
            public void DiscountPlusGiftCoversEntireAmount_NoTip()
            {
                const decimal discount = 2;
                const decimal credit = 3;
                const decimal spend = 5;
                const decimal tip = 0;

                decimal pay = Math.Max(0, spend - (discount + credit));

                PaymentCalculator calculator = new PaymentCalculator(discount, credit, spend, tip);

                calculator.GetNumberOfPaymentMethodsRequired().Should().Be(2);
                calculator.ApplyLevelUpDiscount.Should().Be(true);
                calculator.ApplyLevelUpGiftCard.Should().Be(true);
                calculator.ApplyLevelUpPayment.Should().Be(false);

                calculator.LevelUpDiscountToApply.Should().Be(discount);
                calculator.GiftCardPaymentToApply.Should().Be(credit);
                calculator.TotalGiftCardPaymentToApplyIncludingTip.Should().Be(credit);
                calculator.LevelUpPaymentToApply.Should().Be(pay);
                calculator.TotalLevelUpPaymentToApplyIncludingTip.Should().Be(pay + tip);
            }

            [TestMethod]
            public void DiscountPlusGiftCoversEntireAmount_WithLevelUpTip()
            {
                const decimal discount = 2;
                const decimal credit = 3;
                const decimal spend = 5;
                const decimal tip = 1;

                decimal pay = Math.Max(0, spend - (discount + credit));
                decimal gift = Math.Min(spend - discount, credit);

                PaymentCalculator calculator = new PaymentCalculator(discount, credit, spend, tip);

                calculator.GetNumberOfPaymentMethodsRequired().Should().Be(3);
                calculator.ApplyLevelUpDiscount.Should().Be(true);
                calculator.ApplyLevelUpGiftCard.Should().Be(true);
                calculator.ApplyLevelUpPayment.Should().Be(true);

                calculator.LevelUpDiscountToApply.Should().Be(discount);
                calculator.GiftCardPaymentToApply.Should().Be(gift);
                calculator.TotalGiftCardPaymentToApplyIncludingTip.Should().Be(gift);
                calculator.LevelUpPaymentToApply.Should().Be(pay);
                calculator.TotalLevelUpPaymentToApplyIncludingTip.Should().Be(pay + tip);
            }

            [TestMethod]
            public void DiscountPlusGiftCoversEntireAmount_WithGiftTip()
            {
                const decimal discount = 2;
                const decimal credit = 4;
                const decimal spend = 5;
                const decimal tip = 1;

                decimal pay = Math.Max(0, spend - (discount + credit));
                decimal gift = Math.Min(spend - discount, credit);

                PaymentCalculator calculator = new PaymentCalculator(discount, credit, spend, tip);

                calculator.GetNumberOfPaymentMethodsRequired().Should().Be(2);
                calculator.ApplyLevelUpDiscount.Should().Be(true);
                calculator.ApplyLevelUpGiftCard.Should().Be(true);
                calculator.ApplyLevelUpPayment.Should().Be(false);

                calculator.LevelUpDiscountToApply.Should().Be(discount);
                calculator.GiftCardPaymentToApply.Should().Be(gift);
                calculator.TotalGiftCardPaymentToApplyIncludingTip.Should().Be(Math.Min(credit, gift + tip));
                calculator.LevelUpPaymentToApply.Should().Be(pay);
                calculator.TotalLevelUpPaymentToApplyIncludingTip.Should().Be(0);
            }

            [TestMethod]
            public void DiscountPlusGiftCoversEntireAmount_WithSplitTip()
            {
                const decimal discount = 2;
                const decimal credit = 5;
                const decimal spend = 6;
                const decimal tip = 2;

                decimal pay = Math.Max(0, spend - (discount + credit));
                decimal gift = Math.Min(spend - discount, credit);

                PaymentCalculator calculator = new PaymentCalculator(discount, credit, spend, tip);

                calculator.GetNumberOfPaymentMethodsRequired().Should().Be(3);
                calculator.ApplyLevelUpDiscount.Should().Be(true);
                calculator.ApplyLevelUpGiftCard.Should().Be(true);
                calculator.ApplyLevelUpPayment.Should().Be(true);

                calculator.LevelUpDiscountToApply.Should().Be(discount);
                calculator.GiftCardPaymentToApply.Should().Be(gift);
                calculator.TotalGiftCardPaymentToApplyIncludingTip.Should().Be(Math.Min(credit, gift + tip));
                calculator.LevelUpPaymentToApply.Should().Be(pay);
                calculator.TotalLevelUpPaymentToApplyIncludingTip.Should().Be(1);
            }

            [TestClass]
            public class DiscountsDisabled
            {
                [TestMethod]
                public void DiscountCoversEntireAmount_NoTip()
                {
                    decimal discountApplied = 0;
                    decimal calculatedDiscount = 2;
                    decimal creditAvailable = 10;
                    decimal spendAmount = 2;
                    decimal tip = 0;
                    int numPayments = 1;
                    bool applyDiscount = false, applyGiftCard = false, applyLevelUp = true;
                    decimal giftCardPayment = 0;
                    decimal totalGiftCardPayment = 0;
                    decimal levelUpPayment = 2;
                    decimal totalLevelUpPayment = 2;

                    PaymentCalculator calc = new PaymentCalculator(discountApplied,
                                                                   creditAvailable,
                                                                   spendAmount,
                                                                   tip)
                        {
                            CalculatedDiscount = calculatedDiscount,
                        };
                    calc.GetNumberOfPaymentMethodsRequired().Should().Be(numPayments);
                    calc.ApplyLevelUpDiscount.Should().Be(applyDiscount);
                    calc.ApplyLevelUpGiftCard.Should().Be(applyGiftCard);
                    calc.ApplyLevelUpPayment.Should().Be(applyLevelUp);
                    calc.LevelUpDiscountToApply.Should().Be(discountApplied);
                    calc.GiftCardPaymentToApply.Should().Be(giftCardPayment);
                    calc.TotalGiftCardPaymentToApplyIncludingTip.Should().Be(totalGiftCardPayment);
                    calc.LevelUpPaymentToApply.Should().Be(levelUpPayment);
                    calc.TotalLevelUpPaymentToApplyIncludingTip.Should().Be(totalLevelUpPayment);
                }

                [TestMethod]
                public void DiscountCoversEntireAmount()
                {
                    decimal discountApplied = 0;
                    decimal calculatedDiscount = 1;
                    decimal creditAvailable = 10;
                    decimal spendAmount = 2;
                    decimal tip = 1;
                    int numPayments = 2;
                    bool applyDiscount = false, applyGiftCard = true, applyLevelUp = true;
                    decimal giftCardPayment = 1;
                    decimal totalGiftCardPayment = 2;
                    decimal levelUpPayment = 1;
                    decimal totalLevelUpPayment = 1;

                    PaymentCalculator calc = new PaymentCalculator(discountApplied,
                                                                   creditAvailable,
                                                                   spendAmount,
                                                                   tip)
                        {
                            CalculatedDiscount = calculatedDiscount,
                        };
                    calc.GetNumberOfPaymentMethodsRequired().Should().Be(numPayments);
                    calc.ApplyLevelUpDiscount.Should().Be(applyDiscount);
                    calc.ApplyLevelUpGiftCard.Should().Be(applyGiftCard);
                    calc.ApplyLevelUpPayment.Should().Be(applyLevelUp);
                    calc.LevelUpDiscountToApply.Should().Be(discountApplied);
                    calc.GiftCardPaymentToApply.Should().Be(giftCardPayment);
                    calc.TotalGiftCardPaymentToApplyIncludingTip.Should().Be(totalGiftCardPayment);
                    calc.LevelUpPaymentToApply.Should().Be(levelUpPayment);
                    calc.TotalLevelUpPaymentToApplyIncludingTip.Should().Be(totalLevelUpPayment);
                }

                [TestMethod]
                public void DiscountCoversEntireAmount_SplitTip()
                {
                    decimal discountApplied = 0;
                    decimal calculatedDiscount = 2;
                    decimal creditAvailable = 10;
                    decimal spendAmount = 2;
                    decimal tip = 1;
                    int numPayments = 2;
                    bool applyDiscount = false, applyGiftCard = true, applyLevelUp = true;
                    decimal giftCardPayment = 0;
                    decimal totalGiftCardPayment = 1;
                    decimal levelUpPayment = 2;
                    decimal totalLevelUpPayment = 2;

                    PaymentCalculator calc = new PaymentCalculator(discountApplied,
                                                                   creditAvailable,
                                                                   spendAmount,
                                                                   tip)
                    {
                        CalculatedDiscount = calculatedDiscount,
                    };
                    calc.GetNumberOfPaymentMethodsRequired().Should().Be(numPayments);
                    calc.ApplyLevelUpDiscount.Should().Be(applyDiscount);
                    calc.ApplyLevelUpGiftCard.Should().Be(applyGiftCard);
                    calc.ApplyLevelUpPayment.Should().Be(applyLevelUp);
                    calc.LevelUpDiscountToApply.Should().Be(discountApplied);
                    calc.GiftCardPaymentToApply.Should().Be(giftCardPayment);
                    calc.TotalGiftCardPaymentToApplyIncludingTip.Should().Be(totalGiftCardPayment);
                    calc.LevelUpPaymentToApply.Should().Be(levelUpPayment);
                    calc.TotalLevelUpPaymentToApplyIncludingTip.Should().Be(totalLevelUpPayment);
                }

                [TestMethod]
                public void DiscountPlusGiftCoversEntireAmount_NoTip()
                {
                    decimal discountApplied = 0;
                    decimal calculatedDiscount = 2;
                    decimal creditAvailable = 3;
                    decimal spendAmount = 5;
                    decimal tip = 0;
                    int numPayments = 2;
                    bool applyDiscount = false, applyGiftCard = true, applyLevelUp = true;
                    decimal giftCardPayment = 3;
                    decimal totalGiftCardPayment = 3;
                    decimal levelUpPayment = 2;
                    decimal totalLevelUpPayment = 2;

                    PaymentCalculator calc = new PaymentCalculator(discountApplied,
                                                                   creditAvailable,
                                                                   spendAmount,
                                                                   tip)
                        {
                            CalculatedDiscount = calculatedDiscount,
                        };
                    calc.GetNumberOfPaymentMethodsRequired().Should().Be(numPayments);
                    calc.ApplyLevelUpDiscount.Should().Be(applyDiscount);
                    calc.ApplyLevelUpGiftCard.Should().Be(applyGiftCard);
                    calc.ApplyLevelUpPayment.Should().Be(applyLevelUp);
                    calc.LevelUpDiscountToApply.Should().Be(discountApplied);
                    calc.GiftCardPaymentToApply.Should().Be(giftCardPayment);
                    calc.TotalGiftCardPaymentToApplyIncludingTip.Should().Be(totalGiftCardPayment);
                    calc.LevelUpPaymentToApply.Should().Be(levelUpPayment);
                    calc.TotalLevelUpPaymentToApplyIncludingTip.Should().Be(totalLevelUpPayment);
                }

                [TestMethod]
                public void DiscountPlusGiftCoversEntireAmount()
                {
                    decimal discountApplied = 0;
                    decimal calculatedDiscount = 2;
                    decimal creditAvailable = 3;
                    decimal spendAmount = 5;
                    decimal tip = 2;
                    int numPayments = 2;
                    bool applyDiscount = false, applyGiftCard = true, applyLevelUp = true;
                    decimal giftCardPayment = 3;
                    decimal totalGiftCardPayment = 3;
                    decimal levelUpPayment = 2;
                    decimal totalLevelUpPayment = 4;

                    PaymentCalculator calc = new PaymentCalculator(discountApplied,
                                                                   creditAvailable,
                                                                   spendAmount,
                                                                   tip)
                        {
                            CalculatedDiscount = calculatedDiscount,
                        };
                    calc.GetNumberOfPaymentMethodsRequired().Should().Be(numPayments);
                    calc.ApplyLevelUpDiscount.Should().Be(applyDiscount);
                    calc.ApplyLevelUpGiftCard.Should().Be(applyGiftCard);
                    calc.ApplyLevelUpPayment.Should().Be(applyLevelUp);
                    calc.LevelUpDiscountToApply.Should().Be(discountApplied);
                    calc.GiftCardPaymentToApply.Should().Be(giftCardPayment);
                    calc.TotalGiftCardPaymentToApplyIncludingTip.Should().Be(totalGiftCardPayment);
                    calc.LevelUpPaymentToApply.Should().Be(levelUpPayment);
                    calc.TotalLevelUpPaymentToApplyIncludingTip.Should().Be(totalLevelUpPayment);
                }

                [TestMethod]
                public void TotalAmountExceedsAvailableCredit()
                {
                    decimal discountApplied = 0;
                    decimal calculatedDiscount = 2;
                    decimal creditAvailable = 3;
                    decimal spendAmount = 15;
                    decimal tip = 1;
                    int numPayments = 2;
                    bool applyDiscount = false, applyGiftCard = true, applyLevelUp = true;
                    decimal giftCardPayment = 3;
                    decimal totalGiftCardPayment = 3;
                    decimal levelUpPayment = 12;
                    decimal totalLevelUpPayment = 13;

                    PaymentCalculator calc = new PaymentCalculator(discountApplied,
                                                                   creditAvailable,
                                                                   spendAmount,
                                                                   tip)
                        {
                            CalculatedDiscount = calculatedDiscount,
                        };
                    calc.GetNumberOfPaymentMethodsRequired().Should().Be(numPayments);
                    calc.ApplyLevelUpDiscount.Should().Be(applyDiscount);
                    calc.ApplyLevelUpGiftCard.Should().Be(applyGiftCard);
                    calc.ApplyLevelUpPayment.Should().Be(applyLevelUp);
                    calc.LevelUpDiscountToApply.Should().Be(discountApplied);
                    calc.GiftCardPaymentToApply.Should().Be(giftCardPayment);
                    calc.TotalGiftCardPaymentToApplyIncludingTip.Should().Be(totalGiftCardPayment);
                    calc.LevelUpPaymentToApply.Should().Be(levelUpPayment);
                    calc.TotalLevelUpPaymentToApplyIncludingTip.Should().Be(totalLevelUpPayment);
                }
            }
        }
    }
}
