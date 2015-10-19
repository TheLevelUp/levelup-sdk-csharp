using System;
using System.Collections.Generic;

namespace LevelUp.Api.Utilities
{
    public static class Exemptions
    {
        /// <summary>
        /// Determines if any items are exempted and Marks items as exempted according to the specified list
        /// of exempted items. Return the total of the exempted item amounts.
        /// </summary>
        /// <param name="items">A collection of items to mark and total exemptions for</param>
        /// <param name="exemptedItemsList">A list of exempted item ids</param>
        /// <returns>The total value of exempted items for the collections passed in</returns>
        public static decimal MarkExempted(IEnumerable<IExemptableItem> items, ICollection<string> exemptedItemsList)
        {
            decimal totalExemptedAmountInDollars = 0m;

            if (null != items)
            {
                foreach (IExemptableItem item in items)
                {
                    totalExemptedAmountInDollars += MarkExempted(item, exemptedItemsList);
                }
            }

            return totalExemptedAmountInDollars;
        }

        /// <summary>
        /// Determines whether a specified item or any of its children is in the specified list of exempted items. 
        /// If so, the item or sub item is marked as exempt and the value of it and all its exempted sub items is returned
        /// </summary>
        /// <param name="item">The item to test and mark</param>
        /// <param name="exemptedItemsList">A list of exempted item ids</param>
        /// <returns>The total value of the exempted items and all of its children</returns>
        public static decimal MarkExempted(IExemptableItem item, ICollection<string> exemptedItemsList)
        {
            decimal totalItemExemptedAmountInDollars = 0m;

            if (null != item && null != exemptedItemsList && exemptedItemsList.Count > 0)
            {
                if (!string.IsNullOrEmpty(item.Sku) && exemptedItemsList.Contains(item.Sku.ToLowerInvariant()))
                {
                    //Found exempted item
                    item.IsExempt = true;
                    totalItemExemptedAmountInDollars += item.TotalInDollars.GetValueOrDefault(0);
                }

                if (null != item.Children)
                {
                    //Depth first search. Recursive!
                    foreach (IExemptableItem child in item.Children)
                    {
                        totalItemExemptedAmountInDollars += MarkExempted(child, exemptedItemsList);
                    }
                }
            }

            return totalItemExemptedAmountInDollars;
        }
    }
}
