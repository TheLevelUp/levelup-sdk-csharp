#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="Exemptions.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using System.Collections.Generic;

namespace LevelUp.Api.Utilities
{
    public static class Exemptions
    {
        /// <summary>
        /// Determines if any items are exempted and marks items as exempted according to the specified list
        /// of items which should be exempt.
        /// </summary>
        /// <param name="items">A collection of items to mark and total exemptions for</param>
        /// <param name="exemptedItemsList">A list of exempted item ids</param>
        public static void MarkExempted(IEnumerable<IExemptableItem> items, ICollection<string> exemptedItemsList)
        {
            if (null == items || null == exemptedItemsList || exemptedItemsList.Count == 0)
            {
                return;
            }

            OperateOnItemDataTree(new Stack<IExemptableItem>(items),
                             o =>
                                 {
                                     if (!string.IsNullOrEmpty(o.Sku) &&
                                         exemptedItemsList.Contains(o.Sku.ToLowerInvariant()))
                                     {
                                         o.IsExempt = true;
                                     }

                                 });
        }

        /// <summary>
        /// Sums the total exemption amount in dollars for a specified collection of items
        /// </summary>
        /// <param name="items">A collection of items where some items may have been 
        /// previously marked as IsExempt = true</param>
        /// <returns>The total of the charged price in dollars for all the items in the collection 
        /// and all of their children.</returns>
        public static decimal Sum(IEnumerable<IExemptableItem> items)
        {
            if (null == items)
            {
                return 0m;
            }

            decimal totalItemExemptedAmountInDollars = 0m;

            OperateOnItemDataTree(new Stack<IExemptableItem>(items),
                o =>
                    {
                        if (o.IsExempt)
                        {
                            totalItemExemptedAmountInDollars += o.ChargedPriceTotalInDollars;
                        }
                    });

            return totalItemExemptedAmountInDollars;
        }

        private static void OperateOnItemDataTree(Stack<IExemptableItem> itemStack, Action<IExemptableItem> operation)
        {
            while (itemStack.Count > 0)
            {
                IExemptableItem item = itemStack.Pop();

                if (item == null)
                {
                    continue;
                }

                operation.Invoke(item);

                if (item.Children != null)
                {
                    foreach (IExemptableItem child in item.Children)
                    {
                        itemStack.Push(child);
                    }
                }
            }
        }
    }
}
