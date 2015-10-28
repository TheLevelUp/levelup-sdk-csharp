//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="Program.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using LevelUp.Api.Client.Filters;
using LevelUp.Api.Client.Models.Responses;
using System;
using System.Collections;
using System.Collections.Generic;

namespace LevelUp.Api.Client.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                ILevelUpClient v14 = LevelUpClientFactory.Create(TestData.Valid.COMPANY_NAME,
                                                                 TestData.Valid.PRODUCT_NAME,
                                                                 TestData.Valid.PRODUCT_VERSION,
                                                                 TestData.Valid.OS_NAME,
                                                                 TestConstants.BASE_URL_CONFIG_FILE);

                Console.WriteLine("========== Authenticate ==========\n");

                AccessToken token = v14.Authenticate(LevelUpTestConfiguration.Current.ApiKey,
                                                     LevelUpTestConfiguration.Current.Username,
                                                     LevelUpTestConfiguration.Current.Password);
                Console.WriteLine(token.ToString());

                Console.WriteLine("========== Locations ==========\n");

                int merchantId = token.MerchantId.Value;
                IList<Location> locations = v14.ListLocations(token.Token, merchantId);
                Console.WriteLine(string.Join(Environment.NewLine, locations));

                Console.WriteLine("========== Filtered Orders ==========\n");

                int locationId = locations[0].LocationId;
                var nameFilter = new OrdersByCustomerNameFilter(TestData.Valid.POS_TEST_USER_FIRST_NAME,
                                                                TestData.Valid.POS_TEST_USER_LAST_INITIAL);
                const int numPagesToSearch = 5;

                var filteredOrders = ((LevelUpClient) v14).ListFilteredOrders(token.Token,
                                                                              locationId,
                                                                              nameFilter,
                                                                              numPagesToSearch);
                Console.WriteLine(string.Join(Environment.NewLine, filteredOrders));

                DateTime start = DateTime.Now - new TimeSpan(3, 12, 0, 0);
                DateTime end = DateTime.Now - new TimeSpan(2, 0, 0, 0, 0);
                var dateFilter = new OrdersByDateFilter(start, end);
                filteredOrders = ((LevelUpClient) v14).ListFilteredOrders(token.Token,
                                                                          locationId,
                                                                          dateFilter);
                Console.WriteLine(string.Join(Environment.NewLine, filteredOrders));

                var orFilter = new LogicalOrFilter<OrderDetailsResponse>(nameFilter, dateFilter);
                filteredOrders = ((LevelUpClient) v14).ListFilteredOrders(token.Token, locationId, orFilter, 10);

                Console.WriteLine(string.Join(Environment.NewLine, filteredOrders));

                var andFilter = new LogicalAndFilter<OrderDetailsResponse>(nameFilter, dateFilter);
                filteredOrders = ((LevelUpClient) v14).ListFilteredOrders(token.Token,
                                                                          locationId,
                                                                          andFilter,
                                                                          10);

                Console.WriteLine(string.Join(Environment.NewLine, filteredOrders));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
