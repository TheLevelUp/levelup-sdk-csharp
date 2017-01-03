#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="Money.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using System.Globalization;

namespace LevelUp.Api.Utilities
{
    public static class Money
    {
        private const int CENTS_TO_DOLLARS_CONVERSION_FACTOR = 100;

        static Money()
        {
            //Use negative sign instead of parens to indicate negative currency values.
            //http://msdn.microsoft.com/en-us/library/system.globalization.numberformatinfo.currencynegativepattern(v=vs.110).aspx
            Constants.EnUsCulture.NumberFormat.CurrencyNegativePattern = 1;
        }

        /// <summary>
        /// Converts a dollar amount to cents
        /// </summary>
        /// <param name="dollars">Amount in dollars</param>
        /// <returns>The dollar amount converted to cents</returns>
        public static int ToCents(decimal dollars)
        {
            return decimal.ToInt32(dollars*CENTS_TO_DOLLARS_CONVERSION_FACTOR);
        }

        /// <summary>
        /// Converts a dollar amount to cents
        /// </summary>
        /// <param name="dollars">Amount in dollars</param>
        /// <returns>The dollar amount converted to cents</returns>
        public static int? ToCents(decimal? dollars)
        {
            return dollars.HasValue
                       ? ToCents(dollars.Value)
                       : (int?)null;
        }

        /// <summary>
        /// Converts a dollar amount to cents
        /// </summary>
        /// <param name="dollars">Amount in dollars</param>
        /// <returns>The dollar amount converted to cents</returns>
        public static int ToCents(string dollars)
        {
            decimal dollarsDecimal;

            if (!decimal.TryParse(dollars, NumberStyles.Currency, Constants.EnUsCulture, out dollarsDecimal))
            {
                throw new ArgumentOutOfRangeException("dollars", "Value must be able to be parsed to a decimal.");
            }

            return ToCents(dollarsDecimal);
        }

        /// <summary>
        /// Converts a nullable cent amount to dollars
        /// </summary>
        /// <param name="cents">Amount in cents</param>
        /// <returns>The cent amount converted to dollars</returns>
        public static decimal? ToDollars(int? cents)
        {
            return cents.HasValue
                       ? ToDollars(cents.Value)
                       : (decimal?) null;
        }

        /// <summary>
        /// Converts an amount in cents to dollars
        /// </summary>
        /// <param name="cents">Amount in cents</param>
        /// <returns>The amount in cents converted to dollars</returns>
        public static decimal ToDollars(int cents)
        {
            return ToDollars((long) cents);
        }

        /// <summary>
        /// Converts an amount in cents to dollars
        /// </summary>
        /// <param name="cents">Amount in cents</param>
        /// <returns>The amount in cents converted to dollars</returns>
        public static decimal ToDollars(long cents)
        {
            return decimal.Divide(cents, CENTS_TO_DOLLARS_CONVERSION_FACTOR);
        }

        /// <summary>
        /// Converts an amount in cents to dollars
        /// </summary>
        /// <param name="cents">Amount in cents</param>
        /// <returns>The amount in cents converted to dollars</returns>
        public static decimal ToDollars(string cents)
        {
            int centsInt;

            if (!Int32.TryParse(cents, out centsInt))
            {
                throw new ArgumentOutOfRangeException("cents", "Value must be able to be parsed to an integer.");
            }

            return ToDollars(centsInt);
        }

        /// <summary>
        /// Returns a currency formatted string (2 decimal places) prefixed with the currency symbol.
        /// </summary>
        /// <param name="dollars">The dollar amount to format as string</param>
        /// <returns>a string in the format "$12.34"</returns>
        public static string ToCurrencyString(decimal dollars)
        {
            return dollars.ToString("C2", Constants.EnUsCulture);
        }
    }
}
