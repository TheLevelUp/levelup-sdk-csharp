//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="PaymentCalculator.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using System.Globalization;
using System.Text;

namespace LevelUp.Api.Utilities
{
    public class PaymentCalculator
    {
        private static readonly CultureInfo EN_US_CULTURE = CultureInfo.CreateSpecificCulture("en-US");
        protected PaymentTypes _requiredPaymentTypes;
        protected decimal? _calculatedDiscount;

        #region Static Discount Application Calculation Methods

        /// <summary>
        /// Calculates the LevelUp discount to apply based on the arguments passed
        /// </summary>
        /// <param name="merchantFundedCreditAvailableInCents">The merchant funded credit amount available in cents returned from the LevelUp API</param>
        /// <param name="remainingAmountDueBeforeTaxInCents">The remaining pre-tax balance due on the check in cents</param>
        /// <returns>The LevelUp discount to apply in dollars</returns>
        /// <see cref="http://developer.thelevelup.com/api-reference/v14/merchant-funded-credit/"/>
        public static decimal CalculateDiscountToApply(long merchantFundedCreditAvailableInCents,
                                                       long remainingAmountDueBeforeTaxInCents)
        {
            return CalculateDiscountToApply(merchantFundedCreditAvailableInCents,
                                            remainingAmountDueBeforeTaxInCents, 
                                            0);
        }

        /// <summary>
        /// Calculates the LevelUp discount to apply based on the arguments passed
        /// </summary>
        /// <param name="merchantFundedCreditAvailableInCents">The merchant funded credit amount available in cents returned from the LevelUp API</param>
        /// <param name="amountDueInCents">The remaining amount due on the check including tax in cents</param>
        /// <param name="taxAmountDueInCents">The tax amount due on the check in cents</param>
        /// <returns>The LevelUp discount to apply in dollars</returns>
        /// <see cref="http://developer.thelevelup.com/api-reference/v14/merchant-funded-credit/"/>
        public static decimal CalculateDiscountToApply(long merchantFundedCreditAvailableInCents,
                                                       long amountDueInCents,
                                                       long taxAmountDueInCents)
        {
            return CalculateDiscountToApply(merchantFundedCreditAvailableInCents,
                                            amountDueInCents,
                                            amountDueInCents,
                                            taxAmountDueInCents,
                                            0);
        }

        /// <summary>
        /// Calculates the LevelUp discount to apply based on the arguments passed
        /// </summary>
        /// <param name="merchantFundedCreditAvailableInCents">The merchant funded credit amount available in cents returned from the LevelUp API</param>
        /// <param name="paymentAmountInCents">The requested payment amount from the cashier/customer in cents</param>
        /// <param name="amountDueInCents">The remaining amount due on the check including tax in cents</param>
        /// <param name="taxAmountDueInCents">The tax amount due on the check in cents</param>
        /// <param name="exemptedItemsTotalInCents">Total cost of all exempted items on the check in cents</param>
        /// <returns>The LevelUp discount to apply in dollars</returns>
        /// <see cref="http://developer.thelevelup.com/api-reference/v14/merchant-funded-credit/"/>
        public static decimal CalculateDiscountToApply(long merchantFundedCreditAvailableInCents,
                                                       long paymentAmountInCents,
                                                       long amountDueInCents,
                                                       long taxAmountDueInCents,
                                                       long exemptedItemsTotalInCents)
        {
            return CalculateDiscountToApply(Money.ToDollars(merchantFundedCreditAvailableInCents),
                                            Money.ToDollars(paymentAmountInCents),
                                            Money.ToDollars(amountDueInCents),
                                            Money.ToDollars(taxAmountDueInCents),
                                            Money.ToDollars(exemptedItemsTotalInCents));
        }

        /// <summary>
        /// Calculates the LevelUp discount to apply based on the arguments passed
        /// </summary>
        /// <param name="merchantFundedCreditAvailableInDollars">The merchant funded credit amount available in dollars returned from the LevelUp API</param>
        /// <param name="remainingAmountDueBeforeTaxInDollars">The remaining pre-tax balance due on the check in dollars</param>
        /// <returns>The LevelUp discount to apply in dollars</returns>
        /// <see cref="http://developer.thelevelup.com/api-reference/v14/merchant-funded-credit/"/>
        public static decimal CalculateDiscountToApply(decimal merchantFundedCreditAvailableInDollars,
                                                       decimal remainingAmountDueBeforeTaxInDollars)
        {
            return CalculateDiscountToApply(merchantFundedCreditAvailableInDollars,
                                            remainingAmountDueBeforeTaxInDollars,
                                            0);
        }

        /// <summary>
        /// Calculates the LevelUp discount to apply based on the arguments passed
        /// </summary>
        /// <param name="merchantFundedCreditAvailableInDollars">The merchant funded credit amount available in dollars returned from the LevelUp API</param>
        /// <param name="amountDueInDollars">The remaining amount due on the check including tax in dollars</param>
        /// <param name="taxAmountDueInDollars">The tax amount due on the check in dollars</param>
        /// <param name="exemptedItemsTotalInDollars">Total cost of all exempted items on the check in dollars</param>
        /// <returns>The LevelUp discount to apply in dollars</returns>
        /// <see cref="http://developer.thelevelup.com/api-reference/v14/merchant-funded-credit/"/>
        public static decimal CalculateDiscountToApply(decimal merchantFundedCreditAvailableInDollars,
                                                       decimal amountDueInDollars,
                                                       decimal taxAmountDueInDollars,
                                                       decimal exemptedItemsTotalInDollars)
        {
            return CalculateDiscountToApply(merchantFundedCreditAvailableInDollars,
                                            Math.Max(0, amountDueInDollars - taxAmountDueInDollars),
                                            exemptedItemsTotalInDollars);
        }

        /// <summary>
        /// Calculates the LevelUp discount to apply based on the arguments passed
        /// </summary>
        /// <param name="merchantFundedCreditAvailableInDollars">The merchant funded credit amount available in dollars returned from the LevelUp API</param>
        /// <param name="paymentAmountInDollars">The requested payment amount from the cashier/customer in dollars</param>
        /// <param name="amountDueInDollars">The remaining amount due on the check including tax in dollars</param>
        /// <param name="taxAmountDueInDollars">The tax amount due on the check in dollars</param>
        /// <param name="exemptedItemsTotalInDollars">Total cost of all exempted items on the check in dollars</param>
        /// <returns>The LevelUp discount to apply in dollars</returns>
        /// <see cref="http://developer.thelevelup.com/api-reference/v14/merchant-funded-credit/"/>
        public static decimal CalculateDiscountToApply(decimal merchantFundedCreditAvailableInDollars,
                                                       decimal paymentAmountInDollars,
                                                       decimal amountDueInDollars,
                                                       decimal taxAmountDueInDollars,
                                                       decimal exemptedItemsTotalInDollars)
        {
            decimal amountDueLessTax = Math.Max(0, amountDueInDollars - taxAmountDueInDollars);

            decimal paymentRequested = Math.Max(0, Math.Min(paymentAmountInDollars, amountDueLessTax));

            if (paymentRequested < amountDueLessTax)
            {
                decimal remainingAmountDueAfterPayment = amountDueLessTax - paymentRequested;

                exemptedItemsTotalInDollars = Math.Max(0, exemptedItemsTotalInDollars - remainingAmountDueAfterPayment);
            }

            return CalculateDiscountToApply(merchantFundedCreditAvailableInDollars,
                                            paymentRequested,
                                            exemptedItemsTotalInDollars);
        }

        /// <summary>
        /// Calculates the LevelUp discount to apply based on the arguments passed
        /// </summary>
        /// <param name="merchantFundedCreditAvailableInDollars">The merchant funded credit amount available in dollars returned from the LevelUp API</param>
        /// <param name="paymentAmountInDollars">The requested, pre-tax payment amount from the cashier/customer in dollars</param>
        /// <param name="exemptedItemsTotalInDollars">Total cost of all exempted items on the check in dollars</param>
        /// <returns>The LevelUp discount to apply in dollars</returns>
        /// <see cref="http://developer.thelevelup.com/api-reference/v14/merchant-funded-credit/"/>
        public static decimal CalculateDiscountToApply(decimal merchantFundedCreditAvailableInDollars,
                                                       decimal paymentAmountInDollars,
                                                       decimal exemptedItemsTotalInDollars)
        {
            return Math.Min(merchantFundedCreditAvailableInDollars,
                            Math.Max(0, paymentAmountInDollars - exemptedItemsTotalInDollars));
        }

        #endregion Static Discount Application Calculation Methods

        #region Payment Applications Calculation Methods

        /// <summary>
        /// Class to aid in the calculation of LevelUp payment/discount types and amounts to apply to a check
        /// </summary>
        /// <param name="discountAppliedInDollars">The total LevelUp discount amount in dollars applied to the check.
        /// If discounts are disabled, this is the amount of discount that would be applied</param>
        /// <param name="giftCardCreditAvailableInDollars">The total LevelUp gift card credit amount available at time of payment</param>
        /// <param name="spendAmountFromLevelUpInDollars">The spend amount returned from the LevelUp API Create Order endpoint</param>
        /// <param name="tipAmountFromLevelUpInDollars">The tip amount returned from the LevelUp API Create Order endpoint</param>
        /// <see cref="http://developer.thelevelup.com/api-reference/v14/orders-create/"/>
        public PaymentCalculator(decimal discountAppliedInDollars,
                                 decimal giftCardCreditAvailableInDollars,
                                 decimal spendAmountFromLevelUpInDollars,
                                 decimal tipAmountFromLevelUpInDollars)
        {
            _requiredPaymentTypes = PaymentTypes.None;
            CalculatedDiscount = null;

            DiscountApplied = discountAppliedInDollars;
            GiftCardCreditAvailable = giftCardCreditAvailableInDollars;

            SpendAmount = spendAmountFromLevelUpInDollars;
            TipAmount = tipAmountFromLevelUpInDollars;
        }

        /// <summary>
        /// Boolean value to indicate whether a LevelUp discount amount ought to be applied as payment
        /// </summary>
        public virtual bool ApplyLevelUpDiscount
        {
            get { return (RequiredPaymentTypes & PaymentTypes.Discount) == PaymentTypes.Discount; }
        }

        /// <summary>
        /// Boolean value to indicate whether a LevelUp gift card amount ought to be applied as payment
        /// </summary>
        public virtual bool ApplyLevelUpGiftCard
        {
            get { return (RequiredPaymentTypes & PaymentTypes.GiftCard) == PaymentTypes.GiftCard; }
        }

        /// <summary>
        /// Boolean value to indicate whether a LevelUp payment amount ought to be applied as payment
        /// </summary>
        public virtual bool ApplyLevelUpPayment
        {
            get { return (RequiredPaymentTypes & PaymentTypes.LevelUp) == PaymentTypes.LevelUp; }
        }

        /// <summary>
        /// Calculated Dollar discount amount that would be applied IFF discounts were not disabled on the POS
        /// WARNING: Do NOT use this field unless discounts are disabled by the POS
        /// </summary>
        public decimal? CalculatedDiscount
        {
            private get { return _calculatedDiscount; }

            set
            {
                if (value.HasValue)
                {
                    DiscountApplied = value.Value;
                }

                _calculatedDiscount = value;
            }
        }

        /// <summary>
        /// Dollar amount of LevelUp discount applied
        /// </summary>
        public decimal DiscountApplied { get; private set; }

        /// <summary>
        /// Dollar amount of LevelUp gift card credit available
        /// </summary>
        public decimal GiftCardCreditAvailable { get; private set; }

        /// <summary>
        /// Dollar amount of LevelUp gift card payment to apply NOT including tip
        /// </summary>
        public virtual decimal GiftCardPaymentToApply
        {
            // (SpendAmount - DiscountApplied) or GiftCardCredit available, whichever is lesser
            // Reduces to MIN(SpendAmountAfterDiscount, GiftCardCreditAvailable)
            get { return Math.Min(SpendAmountAfterDiscount, GiftCardCreditAvailable); }
        }

        /// <summary>
        /// Dollar amount of LevelUp gift card tip to apply NOT including LevelUp gift card payment
        /// </summary>
        public virtual decimal GiftCardTipToApply
        {
            // TotalGiftCardPaymentToApplyIncludingTip - GiftCardPaymentToApply
            get { return TotalGiftCardPaymentToApplyIncludingTip - GiftCardPaymentToApply; }
        }

        /// <summary>
        /// Remaining balance in dollars on LevelUp gift card after gift card payment has been applied
        /// </summary>
        public virtual decimal GiftCardRemainingBalanceAfterPayment
        {
            // GiftCardCreditAvailable - ((SpendAmount - DiscountApplied) + TipAmount) or zero, whichever is greater
            // Reduces to MAX(0, GiftCardCreditAvailable - (SpendAmount - DiscountApplied) + TipAmount)
            // Which reduceds to MAX(0, GiftCardCreditAvailable - SpendAmountAfterDiscount + TipAmount)
            get { return Math.Max(0, GiftCardCreditAvailable - TotalGiftCardPaymentToApplyIncludingTip); }
        }

        /// <summary>
        /// Dollar amount of LevelUp discount to apply
        /// </summary>
        public virtual decimal LevelUpDiscountToApply
        {
            get { return DiscountsDisabled ? 0m : DiscountApplied; }
        }

        /// <summary>
        /// Dollar amount of LevelUp payment to apply NOT including the tip amount
        /// </summary>
        public virtual decimal LevelUpPaymentToApply
        {
            // ((SpendAmount - DiscountApplied) - GiftCardCreditAvailable) or 0, whichever is greater
            // Reduces to (SpendAmountAfterDiscount - GiftCardCreditAvailable)
            get
            {
                decimal levelUp = Math.Max(0, SpendAmountAfterDiscount - GiftCardCreditAvailable);

                if (DiscountsDisabled)
                {
                    levelUp += CalculatedDiscount.Value;
                }

                return levelUp;
            }
        }

        /// <summary>
        /// Dollar amount of LevelUp tip to apply NOT including payment amount
        /// </summary>
        public virtual decimal LevelUpTipToApply
        {
            // TotalLevelUpPaymentToApplyIncludingTip - LevelUpPaymentToApply
            get { return TotalLevelUpPaymentToApplyIncludingTip - LevelUpPaymentToApply; }
        }

        /// <summary>
        /// Flags enumeration of the types of payment that ought to be applied
        /// </summary>
        public virtual PaymentTypes RequiredPaymentTypes
        {
            get
            {
                if (_requiredPaymentTypes == PaymentTypes.None)
                {
                    _requiredPaymentTypes = GetPaymentTypesRequired();
                }

                return _requiredPaymentTypes;
            }
        }

        /// <summary>
        /// Spend amount in dollars from LevelUp
        /// </summary>
        public decimal SpendAmount { get; private set; }

        /// <summary>
        /// Spend amount in dollars after discount is subtracted
        /// </summary>
        public virtual decimal SpendAmountAfterDiscount
        {
            // (SpendAmount - DiscountApplied) or 0, whichever is greater
            // if discounts are disabled, this should just be the spend amount
            get { return Math.Max(0, SpendAmount - DiscountApplied); }
        }

        /// <summary>
        /// Tip amount in dollars from LevelUp
        /// </summary>
        public decimal TipAmount { get; private set; }

        public decimal TotalAmount { get { return SpendAmount + TipAmount; } }

        /// <summary>
        /// Total amount in dollars including tip after discount is subtracted
        /// </summary>
        public virtual decimal TotalAmountAfterDiscount
        {
            // (SpendAmount - DiscountApplied) + TipAmount
            // Reduces to (SpendAmountAfterDiscount + TipAmount)
            get { return SpendAmountAfterDiscount + TipAmount; }
        }

        /// <summary>
        /// Total amount in dollars of LevelUp gift card payment to apply including tip
        /// </summary>
        public virtual decimal TotalGiftCardPaymentToApplyIncludingTip
        {
            // ((SpendAmount - DiscountApplied) + TipAmount or Gift card credit available, whichever is lesser
            // Reduces to MIN(TotalAmountAfterDiscount, GiftCardCreditAvailable)
            get { return Math.Min(TotalAmountAfterDiscount, GiftCardCreditAvailable); }
        }

        /// <summary>
        /// Total amount in dollars of LevelUp payment to apply including both tip AND payment amounts
        /// </summary>
        public virtual decimal TotalLevelUpPaymentToApplyIncludingTip
        {
            // ((SpendAmount - DiscountApplied) + TipAmount) - GiftCardCreditAvailable or 0, whichever is greater
            // Reduces to MAX(0, TotalAmountAfterDiscount - GiftCardCreditAvailable)
            get
            {
                decimal totalLevelUp = Math.Max(0, TotalAmountAfterDiscount - GiftCardCreditAvailable);

                if (DiscountsDisabled)
                {
                    totalLevelUp += CalculatedDiscount.Value;
                }

                return totalLevelUp;
            }
        }
        
        /// <summary>
        /// Number of different payment types to apply
        /// </summary>
        /// <returns></returns>
        public virtual int GetNumberOfPaymentMethodsRequired()
        {
            int counter = 0;

            foreach (PaymentTypes type in Enum.GetValues(typeof (PaymentTypes)))
            {
                if (type == PaymentTypes.None)
                {
                    continue;
                }

                if ((RequiredPaymentTypes & type) == type)
                {
                    counter++;
                }
            }

            return counter;
        }

        public string ToString(bool verbose)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Spend amount is {0}{1}.",
                            SpendAmount.ToString("C2", EN_US_CULTURE),
                            TipAmount > 0
                                ? string.Format(" plus an additional tip of {0}",
                                                TipAmount.ToString("C2", EN_US_CULTURE))
                                : string.Empty);
            sb.AppendLine();

            sb.AppendFormat("Discount amount is {0}.",
                                DiscountApplied.ToString("C2", EN_US_CULTURE));

            sb.AppendLine();

            sb.AppendFormat("{0} in gift card credit is available.",
                            GiftCardCreditAvailable.ToString("C2", EN_US_CULTURE));

            sb.AppendLine();
            
            if (verbose)
            {
                if (DiscountsDisabled)
                {
                    sb.AppendFormat("Discounts disabled! Calculated discount is {0}.",
                                    CalculatedDiscount.Value.ToString("C2", EN_US_CULTURE));
                    sb.AppendLine();
                }
                else if (DiscountApplied > 0)
                {
                    sb.AppendFormat("After discount of {0} the adjusted spend amount is {1}" +
                                    " and the adjusted total amount is {2}.",
                                    DiscountApplied.ToString("C2", EN_US_CULTURE),
                                    SpendAmountAfterDiscount.ToString("C2", EN_US_CULTURE),
                                    TotalAmountAfterDiscount.ToString("C2", EN_US_CULTURE));
                    
                    sb.AppendLine();
                    
                    sb.AppendFormat("Applied a LevelUp discount of {0}.",
                                    DiscountApplied.ToString("C2", EN_US_CULTURE));
                    sb.AppendLine();
                }
                else
                {
                    sb.AppendLine("No discount applied.");
                }

                sb.AppendLine(GiftCardCreditAvailable > 0
                              ? string.Format("LevelUp gift card credit to apply after discounts is {0}. " +
                                              "Including tip, the total LevelUp gift card payment to apply is {1}.",
                                              GiftCardPaymentToApply.ToString("C2", EN_US_CULTURE),
                                              TotalGiftCardPaymentToApplyIncludingTip.ToString("C2", EN_US_CULTURE))
                              : "No gift card credit to apply.");

                sb.AppendFormat("LevelUp payment to apply after discounts and gift cards is {0}. " +
                                "Including tip, the total LevelUp payment amount to apply to the check is {1}.",
                                LevelUpPaymentToApply.ToString("C2", EN_US_CULTURE),
                                TotalLevelUpPaymentToApplyIncludingTip.ToString("C2", EN_US_CULTURE));
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public override string ToString()
        {
            return ToString(false);
        }

        protected bool DiscountsDisabled { get { return CalculatedDiscount.HasValue; } }

        protected virtual PaymentTypes GetPaymentTypesRequired()
        {
            PaymentTypes paymentTypes = PaymentTypes.None;

            //If a LevelUp discount should be applied
            if (LevelUpDiscountToApply > 0)
            {
                paymentTypes |= PaymentTypes.Discount;
            }

            //If gift card credit is available, use that to pay
            if (TotalGiftCardPaymentToApplyIncludingTip > 0)
            {
                paymentTypes |= PaymentTypes.GiftCard;
            }

            //If additional payment is still required after discount and gift cards, charge user's LevelUp account
            if (TotalLevelUpPaymentToApplyIncludingTip > 0)
            {
                paymentTypes |= PaymentTypes.LevelUp;
            }

            return paymentTypes;
        }

        #endregion Payment Applications Calculation Methods
    }
}
