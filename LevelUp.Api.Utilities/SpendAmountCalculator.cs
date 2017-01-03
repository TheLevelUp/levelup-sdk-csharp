#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="SpendAmountCalculator.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

namespace LevelUp.Api.Utilities
{
    public class SpendAmountCalculator
    {        
        /// <summary>
        /// Constructor for the SpendAmountCalculator class. All arguments should be amounts in cents
        /// This class is intended to aid in the usage of the LevelUp order creation API flow
        /// </summary>
        /// <param name="paymentRequested">The amount in cents the customer wishes to pay on this check.
        /// Note that this will not always be the same as the amount due.</param>
        /// <param name="amountDueOnCheck">The amount in cents due on this check.</param>
        /// <param name="taxDueOnCheck">The total tax in cents due on this check.</param>
        /// <param name="exemptedItemsTotal">The total amount in cents that should be exempted from loyalty progress
        /// and/or discount eligibility. This is usually a sum of the costs of the exempted items that are present on this check</param>
        public SpendAmountCalculator(int paymentRequested,
                                     int amountDueOnCheck,
                                     int taxDueOnCheck,
                                     int exemptedItemsTotal)
        {
            SpendAmount = Math.Max(0, Math.Min(paymentRequested, amountDueOnCheck));

            int checkSubtotal = amountDueOnCheck - taxDueOnCheck;
            int subtotalPortionOfSpend = Math.Min(SpendAmount, checkSubtotal);

            int remainderDueAfterSpend = amountDueOnCheck - SpendAmount;
            int subtotalRemainderAfterSpend = checkSubtotal - subtotalPortionOfSpend;

            AdjustedExemptionAmount = Math.Min(subtotalPortionOfSpend,
                                               Math.Max(0, exemptedItemsTotal - subtotalRemainderAfterSpend));
            AdjustedTaxAmount = Math.Min(SpendAmount, Math.Max(0, taxDueOnCheck - remainderDueAfterSpend));
        }

        /// <summary>
        /// Constructor for the SpendAmountCalculator class. 
        /// This class is intended to aid in the usage of the LevelUp order creation API flow
        /// </summary>
        /// <param name="paymentRequestedInDollars">The amount in dollars the customer wishes to pay on this check.
        /// Note that this will not always be the same as the amount due.</param>
        /// <param name="amountDueOnCheckInDollars">The amount in dollars due on this check.</param>
        /// <param name="taxDueOnCheckInDollars">The total tax amount in dollars due on this check.</param>
        /// <param name="exemptedItemsTotalInDollars">The total amount in dollars that should be exempted from loyalty progress
        /// and/or discount eligibility. This is usually a sum of the costs of the exempted items that are present on this check</param>
        public SpendAmountCalculator(decimal paymentRequestedInDollars,
                                     decimal amountDueOnCheckInDollars,
                                     decimal taxDueOnCheckInDollars,
                                     decimal exemptedItemsTotalInDollars)
            : this(Money.ToCents(paymentRequestedInDollars),
                   Money.ToCents(amountDueOnCheckInDollars),
                   Money.ToCents(taxDueOnCheckInDollars),
                   Money.ToCents(exemptedItemsTotalInDollars))
        {
        }

        /// <summary>
        /// The exemption amount that is pertinent to this payment in cents. Application of exemption amounts against
        /// the customer's available credit to calculate discount is deferred if possible.
        /// In the case where the customer is paying for the entire amount due on the check, there will be no reduction
        /// and this amount will be the same as the total exempted amount.
        /// In the case where the customer is paying only part of the amount due, this may be a reduced amount 
        /// of the total exemption amount.
        /// </summary>
        public int AdjustedExemptionAmount { get; private set; }

        /// <summary>
        /// The tax amount that is pertinent to this payment in cents. Application of tax amount against the customer's
        /// available credit to calculate discount is deferred if possible.
        /// In the case where the customer is paying for the entire amount due on the check, there will be no reduction
        /// and this amount will be the same as the total tax amount.
        /// In the case where the customer is paying for less than the subtotal due on the check, this amount will be zero.
        /// In the case where the customer's payment includes some but not all of the tax amount, this amount will be 
        /// less than the total tax amount but greater than zero
        /// </summary>
        public int AdjustedTaxAmount { get; private set; }

        /// <summary>
        /// The amount to charge the customer in cents. 
        /// In the case where the payment requested is greater than or equal to the amount due on the check, 
        /// this value will be equal to the amount due on the check.
        /// In the case where the payment requested is less than the amount due on the check, the value will be
        /// equal to the payment amount.
        /// </summary>
        public int SpendAmount { get; private set; }

        public override string ToString()
        {
            return string.Format("Spend Amount: {0}" +
                                 ", Adjusted Tax Amount: {1}" +
                                 ", Adjusted Exemption Amount: {2}.",
                                 Money.ToCurrencyString(Money.ToDollars(SpendAmount)),
                                 Money.ToCurrencyString(Money.ToDollars(AdjustedTaxAmount)),
                                 Money.ToCurrencyString(Money.ToDollars(AdjustedExemptionAmount)));
        }
    }
}
