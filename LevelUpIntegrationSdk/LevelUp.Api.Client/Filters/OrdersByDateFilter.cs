//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="OrdersByDateFilter.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using LevelUp.Api.Client.Models.Responses;
using System;
using System.Globalization;

namespace LevelUp.Api.Client.Filters
{
    /// <summary>
    /// Defines a filter the collects the completed orders from a specified date range
    /// </summary>
    public class OrdersByDateFilter : FilterBase<OrderDetailsResponse>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="start">The inclusive start date & time for the filter</param>
        /// <param name="end">The inclusive end date & time for the filter</param>
        public OrdersByDateFilter(DateTime start, DateTime end)
        {
            if (start > end)
            {
                throw new ArgumentException(
                    string.Format("Start date must be earlier than end date! Start date: {0} End date: {1}",
                                  start.ToShortDateString(),
                                  end.ToShortDateString()));
            }

            StartFilter = start;
            EndFilter = end;
        }

        private DateTime StartFilter { get; set; }

        private DateTime EndFilter { get; set; }

        /// <summary>
        /// Determines the logic for whether a single item belongs in the filtered set
        /// </summary>
        /// <param name="order">A single order item which may belong in the filtered set</param>
        /// <returns>true if the input order item belongs in the filtered set. False otherwise</returns>
        protected override bool ItemMatchesFilter(OrderDetailsResponse order)
        {
            DateTime creationTime = ParseTime(order.CreatedAt);

            return creationTime >= StartFilter && creationTime <= EndFilter;
        }

        private DateTime ParseTime(string timeStringToParse)
        {
            return DateTime.Parse(timeStringToParse, CultureInfo.CurrentCulture, DateTimeStyles.AssumeUniversal);
        }
    }
}
