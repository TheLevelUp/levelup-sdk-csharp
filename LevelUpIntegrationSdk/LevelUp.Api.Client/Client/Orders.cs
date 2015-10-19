//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="Orders.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using LevelUp.Api.Client.Filters;
using LevelUp.Api.Client.Models.Requests;
using LevelUp.Api.Client.Models.Responses;
using LevelUp.Api.Http;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Net;

namespace LevelUp.Api.Client
{
    public partial class LevelUpClient
    {
        /// <summary>
        /// Lists the orders from page range specified. First page is numbered 1
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method</param>
        /// <param name="locationId">Identifies the location for which to return the list of orders</param>
        /// <param name="startPageNum">Page number from which to begin listing orders. Page numbering starts at 1.
        /// Default value is 1</param>
        /// <param name="endPageNum">Page number at which to end listing orders (inclusive). 
        /// If a number less than the starting page number is specified, only a single page of orders will be returned.
        /// Default value is 1</param>
        /// <returns>The collection of orders from pages "startPageNum" to "endPageNum" inclusive</returns>
        public IList<OrderDetailsResponse> ListOrders(string accessToken, 
                                                      int locationId, 
                                                      int startPageNum = 1, 
                                                      int endPageNum = 1)
        {
            bool areThereMorePages;
            return ListOrders(accessToken, locationId, startPageNum, endPageNum, out areThereMorePages);
        }

        /// <summary>
        /// Lists the orders from page range specified. First page is numbered 1
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method</param>
        /// <param name="locationId">Identifies the location for which to return the list of orders</param>
        /// <param name="startPageNum">Page number from which to begin listing orders. Page numbering starts at 1.
        /// </param>
        /// <param name="endPageNum">Page number at which to end listing orders (inclusive). 
        /// If a number less than the starting page number is specified, only a single page of orders will be returned.
        /// </param>
        /// <param name="areThereMorePages">Returns true if there are more pages, otherwise false</param>
        /// <returns>The collection of orders from pages "startPageNum" to "endPageNum" inclusive</returns>
        public IList<OrderDetailsResponse> ListOrders(string accessToken, 
                                                      int locationId, 
                                                      int startPageNum, 
                                                      int endPageNum, 
                                                      out bool areThereMorePages)
        {
            if (endPageNum < startPageNum)
            {
                endPageNum = startPageNum;
            }

            areThereMorePages = true;

            List<OrderDetailsResponse> orders = new List<OrderDetailsResponse>();

            for (int i = startPageNum; i <= endPageNum; i++)
            {
                string nextPageUrl;
                orders.AddRange(GetOnePageOfOrders(accessToken,
                                                   _endpoints.OrdersByLocation(locationId, i),
                                                   out nextPageUrl));

                areThereMorePages = !string.IsNullOrEmpty(nextPageUrl);

                if (!areThereMorePages)
                {
                    break;
                }
            }

            return orders;
        }

        /// <summary>
        /// Place an order and pay through LevelUp
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method. 
        /// This will be the .Token member on the returned object</param>
        /// <param name="orderData">An object containing the order data and spend amounts to process through LevelUp</param>
        /// <returns>A response object indicating whether the order was charged successfully and 
        /// the final amount paid including the customer specified tip amount</returns>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        public OrderResponse PlaceOrder(string accessToken, Order orderData)
        {
            string placeOrderUri = _endpoints.Order();

            // Create the body content of the order request
            string body = JsonConvert.SerializeObject(orderData);

            IRestResponse response = _restService.Post(placeOrderUri, body, accessToken, Identifier.ToString());

            ThrowExceptionOnBadResponseCode(response);

            return JsonConvert.DeserializeObject<OrderResponse>(response.Content);
        }

        /// <summary>
        /// Refund an order placed through LevelUp.
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method. 
        /// This will be the .Token member on the returned object</param>
        /// <param name="orderIdentifier">UUID identifier for the order to refund. 
        /// This information is returned when the order is placed</param>
        /// <param name="managerConfirmation">Some systems require manager confirmation. 
        /// This field should be omitted or set to null if your system does not require manager confirmation</param>
        /// <returns>A response object indicating whether the refund was successful</returns>
        /// <exception cref="LevelUpApiException">Thrown when the order to refund could not be found or 
        /// when the HTTP status returned was not 200 OK</exception>
        public RefundResponse RefundOrder(string accessToken, string orderIdentifier, string managerConfirmation = null)
        {
            string body = JsonConvert.SerializeObject(new Refund(managerConfirmation));

            string orderRefundUri = _endpoints.Refund(orderIdentifier);

            IRestResponse response = _restService.Post(orderRefundUri, body, accessToken, Identifier.ToString());

            if (response.StatusCode == HttpStatusCode.Unauthorized && string.IsNullOrEmpty(response.Content))
            {
                throw new LevelUpApiException(string.Format("The order \"{0}\" does not exist.", orderIdentifier),
                                              response.ErrorException);
            }

            ThrowExceptionOnBadResponseCode(response);

            return JsonConvert.DeserializeObject<RefundResponse>(response.Content);
        }


        /// <summary>
        /// Lists the set of orders specified by the passed filter object
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method</param>
        /// <param name="locationId">Identifies the location for which to return the orders</param>
        /// <param name="filter">An object containing logic to filter the raw order results</param>
        /// <param name="startPageNum">Beginning of the page range to search. Page numbering starts at 1</param>
        /// <param name="endPageNum">Page number at which to end listing orders (inclusive). 
        /// If a number less than the starting page number is specified, only a single page of orders will be returned.</param>
        /// <returns>The set of orders at the specified location that match the passed filter object's logic</returns>
        public IList<OrderDetailsResponse> ListFilteredOrders(string accessToken,
                                                              int locationId,
                                                              IFilter<OrderDetailsResponse> filter,
                                                              int startPageNum,
                                                              int endPageNum)
        {
            if (endPageNum <= startPageNum)
            {
                endPageNum = startPageNum;
            }

            List<OrderDetailsResponse> filteredOrders = new List<OrderDetailsResponse>();

            for (int i = startPageNum; i <= endPageNum; i++)
            {
                string nextPageUrl;
                IEnumerable<OrderDetailsResponse> onePageOfOrders = GetOnePageOfOrders(accessToken,
                                                                                 _endpoints.OrdersByLocation(
                                                                                     locationId,
                                                                                     i),
                                                                                 out nextPageUrl);

                IEnumerable<OrderDetailsResponse> filterResult = filter.Apply(onePageOfOrders);

                if (null != filterResult)
                {
                    filteredOrders.AddRange(filterResult);
                }

                if (string.IsNullOrEmpty(nextPageUrl))
                {
                    break;
                }
            }

            return filteredOrders;
        }

        /// <summary>
        /// Lists the set of orders specified by the passed filter object
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method</param>
        /// <param name="locationId">Identifies the location for which to return the orders</param>
        /// <param name="filter">An object containing logic to filter the raw order results</param>
        /// <param name="maxNumPagesToSearch">A maximum number of pages to search for matches to the filter. 
        /// Default is 10 pages. There are 30 items per page for a default search space of 300 orders</param>
        /// <returns>The set of orders at the specified location that match the passed filter object's logic</returns>
        public IList<OrderDetailsResponse> ListFilteredOrders(string accessToken,
                                                              int locationId,
                                                              IFilter<OrderDetailsResponse> filter,
                                                              int maxNumPagesToSearch = 10)
        {
            return ListFilteredOrders(accessToken, locationId, filter, 1, maxNumPagesToSearch);
        }

        
        private IEnumerable<OrderDetailsResponse> GetOnePageOfOrders(string accessToken,
                                                                     string pageUrl,
                                                                     out string nextPageUrl)
        {
            IRestResponse response = _restService.Get(pageUrl, accessToken, Identifier.ToString());

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                nextPageUrl = string.Empty;
                return new List<OrderDetailsResponse>();
            }

            ThrowExceptionOnBadResponseCode(response);

            nextPageUrl = ParseNextPageUrl(response.Headers);

            return JsonConvert.DeserializeObject<List<OrderDetailsResponse>>(response.Content);
        }
    }
}
