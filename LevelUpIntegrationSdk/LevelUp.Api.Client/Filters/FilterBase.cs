//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="FilterBase.cs" company="SCVNGR, Inc. d/b/a LevelUp">
//   Copyright(c) 2014 SCVNGR, Inc. d/b/a LevelUp. All rights reserved.
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

using System.Collections.Generic;

namespace LevelUp.Api.Client.Filters
{
    /// <summary>
    /// Base, abstract implementation of the filter class
    /// </summary>
    /// <typeparam name="T">Generic type</typeparam>
    public abstract class FilterBase<T> : IFilter<T>
    {
        #region IFilter Implementation
        
        /// <summary>
        /// Filters the given set and returns the result as an IEnumerable
        /// </summary>
        /// <param name="setToFilter">The raw set on which to apply the filter</param>
        /// <returns>The filtered set</returns>
        public IEnumerable<T> Apply(IEnumerable<T> setToFilter)
        {
            foreach (T item in setToFilter)
            {
                if (ItemMatchesFilter(item))
                {
                    yield return item;
                }
            }
        }

        #endregion IFilter Implementation

        /// <summary>
        /// Logic to determine whether a given item from the raw set belongs in the filtered set
        /// </summary>
        /// <param name="item">The item that should be evaluated against the filter logic</param>
        /// <returns>If the item should be in the filtered set, returns true. False otherwise</returns>
        protected abstract bool ItemMatchesFilter(T item);
    }
}
