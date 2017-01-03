#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="ItemTestClass.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using System.Collections.Generic;

namespace LevelUp.Api.Utilities.Test
{
    internal class ItemTestClass : IExemptableItem
    {
        public ItemTestClass(string sku,
                             decimal chargedPriceInDollars,
                             bool isExempt = false,
                             IEnumerable<IExemptableItem> children = null)
        {
            Sku = sku;
            ChargedPriceTotalInDollars = chargedPriceInDollars;
            IsExempt = isExempt;
            Children = children;
        }

        public IEnumerable<IExemptableItem> Children { get; set; }

        public bool IsExempt { get; set; }

        public string Sku { get; set; }

        public decimal ChargedPriceTotalInDollars { get; set; }

        public void AddChild(IExemptableItem childItem)
        {
            Children = new List<IExemptableItem>(Children)
                {
                    childItem,
                };
        }

        public void AddChildren(ICollection<IExemptableItem> children)
        {
            IList<IExemptableItem> temp = new List<IExemptableItem>(this.Children);
            foreach (IExemptableItem item in children)
            {
                temp.Add(item);
            }

            Children = temp;
        }
    }
}
