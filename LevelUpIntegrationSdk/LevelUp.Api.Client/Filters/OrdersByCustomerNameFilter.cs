//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="OrdersByCustomerNameFilter.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using LevelUp.Api.Client.Models.Responses;
using System;

namespace LevelUp.Api.Client.Filters
{
    /// <summary>
    /// Defines a filter for selecting orders placed by a customer based on their first name and last initial
    /// </summary>
    public class OrdersByCustomerNameFilter : FilterBase<OrderDetailsResponse>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="customerFirstName">The full first name of the customer in the LevelUp system</param>
        /// <param name="customerLastInitial">The first letter of the customer's last name in the LevelUp system</param>
        public OrdersByCustomerNameFilter(string customerFirstName, string customerLastInitial)
        {
            if (string.IsNullOrEmpty(customerFirstName))
            {
                throw new ArgumentNullException("customerFirstName");
            }

            if (string.IsNullOrEmpty(customerLastInitial))
            {
                throw new ArgumentNullException("customerLastInitial");
            }
            
            this.FirstName = customerFirstName;
            this.LastInitial = customerLastInitial.Substring(0, 1);
        }

        private string FirstName { get; set; }

        private string LastInitial { get; set; }

        protected override bool ItemMatchesFilter(OrderDetailsResponse item)
        {
            string[] customerNameAndInitial = item.UserName.TrimEnd('.').Split(' ');

            return customerNameAndInitial[0].Equals(FirstName, StringComparison.InvariantCultureIgnoreCase) &&
                   customerNameAndInitial[1].Equals(LastInitial, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
