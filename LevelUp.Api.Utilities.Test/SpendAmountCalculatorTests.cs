#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="SpendAmountCalculatorTests.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
    public class SpendAmountCalculatorTests
    {
        private TestContext context;

        public TestContext TestContext
        {
            get { return context; }
            set { context = value; }
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
        [DeploymentItem("spend_amount_calculator_test_data.xlsx")]
        [DataSource("System.Data.OleDb",
            "Provider=Microsoft.ACE.OLEDB.12.0;" +
            "Data Source=spend_amount_calculator_test_data.xlsx;" +
            "Extended Properties='Excel 12.0;" +
            "HDR=Yes'",
            "SpendAmounts",  //NOTE: This refers to a named range of data in the Excel table. 
            //To edit it, use the Name Manager on the Formulas page 
            //or follow the instructions here: http://www.dummies.com/how-to/content/managing-range-names-in-excel-2010.html
            DataAccessMethod.Sequential)]
        public void SpendAmountCalculator_DataDriven_AdjustSpend()
        {
            const string TEST_CATEGORY = "A";

            int currentRowIndex = context.DataRow.Table.Rows.IndexOf(context.DataRow);
            System.Diagnostics.Debug.WriteLine("Current Data Row: " + currentRowIndex);

            if (!ShouldRunTest(TEST_CATEGORY))
            {
                System.Diagnostics.Debug.WriteLine("Skipping row " + currentRowIndex);
                return;
            }

            CompareValues();
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
        [DeploymentItem("spend_amount_calculator_test_data.xlsx")]
        [DataSource("System.Data.OleDb",
            "Provider=Microsoft.ACE.OLEDB.12.0;" +
            "Data Source=spend_amount_calculator_test_data.xlsx;" +
            "Extended Properties='Excel 12.0;" +
            "HDR=Yes'",
            "SpendAmounts",  //NOTE: This refers to a named range of data in the Excel table. 
            //To edit it, use the Name Manager on the Formulas page 
            //or follow the instructions here: http://www.dummies.com/how-to/content/managing-range-names-in-excel-2010.html
            DataAccessMethod.Sequential)]
        public void SpendAmountCalculator_DataDriven_AdjustTax()
        {
            const string TEST_CATEGORY = "B";

            int currentRowIndex = context.DataRow.Table.Rows.IndexOf(context.DataRow);
            System.Diagnostics.Debug.WriteLine("Current Data Row: " + currentRowIndex);

            if (!ShouldRunTest(TEST_CATEGORY))
            {
                System.Diagnostics.Debug.WriteLine("Skipping row " + currentRowIndex);
                return;
            }

            CompareValues();
        }

        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
        [DeploymentItem("spend_amount_calculator_test_data.xlsx")]
        [DataSource("System.Data.OleDb",
            "Provider=Microsoft.ACE.OLEDB.12.0;" +
            "Data Source=spend_amount_calculator_test_data.xlsx;" +
            "Extended Properties='Excel 12.0;" +
            "HDR=Yes'",
            "SpendAmounts",  //NOTE: This refers to a named range of data in the Excel table. 
            //To edit it, use the Name Manager on the Formulas page 
            //or follow the instructions here: http://www.dummies.com/how-to/content/managing-range-names-in-excel-2010.html
            DataAccessMethod.Sequential)]
        public void SpendAmountCalculator_DataDriven_TaxAndExemptionDeferred()
        {
            const string TEST_CATEGORY = "C";

            int currentRowIndex = context.DataRow.Table.Rows.IndexOf(context.DataRow);
            System.Diagnostics.Debug.WriteLine("Current Data Row: " + currentRowIndex);

            if (!ShouldRunTest(TEST_CATEGORY))
            {
                System.Diagnostics.Debug.WriteLine("Skipping row " + currentRowIndex);
                return;
            }

            CompareValues();
        }


        [TestMethod]
        [TestCategory(LevelUp.Api.Utilities.Test.TestCategories.UnitTests)]
        [DeploymentItem("spend_amount_calculator_test_data.xlsx")]
        [DataSource("System.Data.OleDb",
            "Provider=Microsoft.ACE.OLEDB.12.0;" +
            "Data Source=spend_amount_calculator_test_data.xlsx;" +
            "Extended Properties='Excel 12.0;" +
            "HDR=Yes'",
            "SpendAmounts",  //NOTE: This refers to a named range of data in the Excel table. 
            //To edit it, use the Name Manager on the Formulas page 
            //or follow the instructions here: http://www.dummies.com/how-to/content/managing-range-names-in-excel-2010.html
            DataAccessMethod.Sequential)]
        public void SpendAmountCalculator_DataDriven_AdjustExemption()
        {
            const string TEST_CATEGORY = "D";

            int currentRowIndex = context.DataRow.Table.Rows.IndexOf(context.DataRow);
            System.Diagnostics.Debug.WriteLine("Current Data Row: " + currentRowIndex);

            if (!ShouldRunTest(TEST_CATEGORY))
            {
                System.Diagnostics.Debug.WriteLine("Skipping row " + currentRowIndex);
                return;
            }

            CompareValues();
        }


        private void CompareValues()
        {
            //The index values for each data row depend on the format of the data in the excel spreadsheet. 
            //Take care when modifying!
            decimal paymentRequested = decimal.Parse(context.DataRow[1].ToString());
            decimal amountDue = decimal.Parse(context.DataRow[2].ToString());
            decimal taxDue = decimal.Parse(context.DataRow[3].ToString());
            decimal exemptionAmount = decimal.Parse(context.DataRow[5].ToString());
            string message = string.Format("Amount due (including tax): {0}" +
                                           ", Tax due: {1}" +
                                           ", Payment amount requested: {2}" +
                                           ", Exemption amount: {3}.",
                                           Money.ToCurrencyString(amountDue),
                                           Money.ToCurrencyString(taxDue),
                                           Money.ToCurrencyString(paymentRequested),
                                           Money.ToCurrencyString(exemptionAmount));
            System.Diagnostics.Debug.WriteLine(message);

            decimal adjustedSpend = decimal.Parse(context.DataRow[6].ToString());
            decimal adjustedTax = decimal.Parse(context.DataRow[12].ToString());
            decimal adjustedExemption = decimal.Parse(context.DataRow[13].ToString());

            decimal discountCreditAvailable = decimal.Parse(context.DataRow[14].ToString());

            SpendAmountCalculator calc = new SpendAmountCalculator(paymentRequested,
                                                                   amountDue,
                                                                   taxDue,
                                                                   exemptionAmount);

            calc.SpendAmount.Should().Be(Money.ToCents(adjustedSpend));
            calc.AdjustedExemptionAmount.Should().Be(Money.ToCents(adjustedExemption));
            calc.AdjustedTaxAmount.Should().Be(Money.ToCents(adjustedTax));
            System.Diagnostics.Trace.WriteLine(calc.ToString());

            decimal calculatedDiscountToApplyTarget = PaymentCalculator.CalculateDiscountToApply(
                discountCreditAvailable,
                paymentRequested,
                amountDue,
                taxDue,
                exemptionAmount);

            decimal calculatedDiscountToApplyActual = Math.Min(discountCreditAvailable,
                                                               Math.Max(0,
                                                               adjustedSpend - adjustedTax - adjustedExemption));

            calculatedDiscountToApplyActual.Should().Be(calculatedDiscountToApplyTarget);

            System.Diagnostics.Debug.WriteLine("Discount Credit Available: {0}, Discount to apply: {1}.",
                                               Money.ToCurrencyString(discountCreditAvailable),
                                               Money.ToCurrencyString(calculatedDiscountToApplyActual));
        }

        private bool ShouldRunTest(string desiredTestCategory)
        {
            string testCategory = context.DataRow[0].ToString();

            return testCategory.Equals(desiredTestCategory, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
