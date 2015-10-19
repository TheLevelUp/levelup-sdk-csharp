//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="LogicalAndFilter.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
    /// Class representing a combination of multiple filters combined with a logical AND operation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LogicalAndFilter<T> : IFilter<T>
    {
        private readonly IFilter<T>[] _filters;

        public LogicalAndFilter(params IFilter<T>[] filters)
        {
            _filters = filters;
        }

        public IEnumerable<T> Apply(IEnumerable<T> setToFilter)
        {
            IEnumerable<T> filteredSet = setToFilter;

            foreach (IFilter<T> filter in _filters)
            {
                filteredSet = filter.Apply(new List<T>(filteredSet));
            }

            return filteredSet;
        }
    }
}
