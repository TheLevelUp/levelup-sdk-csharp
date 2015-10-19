//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="Exemptions.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
