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
            Constants.EN_US_CULTURE.NumberFormat.CurrencyNegativePattern = 1;
        }

        /// <summary>
        /// Converts a dollar amount to cents
        /// </summary>
        /// <param name="dollars">Amount in dollars</param>
        /// <returns>The dollar amount converted to cents</returns>
        public static int ToCents(decimal dollars)
        {
            return Decimal.ToInt32(dollars*CENTS_TO_DOLLARS_CONVERSION_FACTOR);
        }

        /// <summary>
        /// Converts a dollar amount to cents
        /// </summary>
        /// <param name="dollars">Amount in dollars</param>
        /// <returns>The dollar amount converted to cents</returns>
        public static int? ToCents(decimal? dollars)
        {
            return dollars.HasValue
                       ? ToCents(dollars.GetValueOrDefault())
                       : new int?();
        }

        /// <summary>
        /// Converts a dollar amount to cents
        /// </summary>
        /// <param name="dollars">Amount in dollars</param>
        /// <returns>The dollar amount converted to cents</returns>
        public static int ToCents(string dollars)
        {
            decimal dollarsDecimal;

            if (!Decimal.TryParse(dollars, NumberStyles.Currency, Constants.EN_US_CULTURE, out dollarsDecimal))
            {
                throw new ArgumentOutOfRangeException("dollars", "Value must be able to be parsed to a decimal.");
            }

            return ToCents(dollarsDecimal);
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
            return (decimal) cents/CENTS_TO_DOLLARS_CONVERSION_FACTOR;
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
            return dollars.ToString("C2", Constants.EN_US_CULTURE);
        }
    }
}
