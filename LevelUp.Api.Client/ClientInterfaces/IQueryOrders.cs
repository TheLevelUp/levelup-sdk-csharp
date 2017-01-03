#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="IQueryOrders.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using LevelUp.Api.Client.Models.Responses;
using LevelUp.Api.Utilities;

namespace LevelUp.Api.Client.ClientInterfaces
{
    public interface IQueryOrders : ILevelUpClientModule
    {
        /// <summary>
        /// Get information on a list of orders associated with a particular location ID.  The LevelUp REST API returns 
        /// this list of order details using paging.
        /// </summary>
        /// <param name="accessToken">The LevelUp access token associated with the merchant account obtained from 
        /// the Authenticate() method</param>
        /// <param name="locationId">LevelUp Location ID whose order history you wish to query</param>
        /// <param name="startPageNum">Page number from which to begin listing orders. Page numbering starts at 1.</param>
        /// <param name="endPageNum">Page number at which to end listing orders (inclusive). If a number less than the 
        /// starting page number is specified, only a single page of orders will be returned.</param>
        /// <returns>The collection of orders from pages "startPageNum" to "endPageNum" inclusive</returns>
        /// <exception cref="LevelUpApiException"> The returned HTTP status code for the request was something other 
        /// than 200 (OK)</exception>
        IList<OrderDetailsResponse> ListOrders(string accessToken,
            int locationId,
            int startPageNum = 1,
            int endPageNum = 1);

        /// <summary>
        /// Get information on a list of orders associated with a particular location ID.  The LevelUp REST API returns 
        /// this list of order details using paging.
        /// </summary>
        /// <param name="accessToken">The LevelUp access token associated with the merchant account obtained from 
        /// the Authenticate() method</param>
        /// <param name="locationId">LevelUp Location ID whose order history you wish to query</param>
        /// <param name="startPageNum">Page number from which to begin listing orders. Page numbering starts at 1.</param>
        /// <param name="endPageNum">Page number at which to end listing orders (inclusive). If a number less than the 
        /// starting page number is specified, only a single page of orders will be returned.</param>
        /// <param name="areThereMorePages">Returns true if there are more pages, otherwise false</param>
        /// <returns>The collection of orders from pages "startPageNum" to "endPageNum" inclusive</returns>
        /// <exception cref="LevelUpApiException"> The returned HTTP status code for the request was something other 
        /// than 200 (OK)</exception>
        IList<OrderDetailsResponse> ListOrders(string accessToken,
            int locationId,
            int startPageNum,
            int endPageNum,
            out bool areThereMorePages);

        /// <summary>
        /// Get information on a list of orders associated with a particular location ID.  The LevelUp REST API returns 
        /// this list of order details using paging.
        /// </summary>
        /// <param name="accessToken">The LevelUp access token associated with the merchant account obtained from 
        /// the Authenticate() method</param>
        /// <param name="locationId">LevelUp Location ID whose order history you wish to query</param>
        /// <param name="startPageNum">Page number from which to begin listing orders. Page numbering starts at 1.</param>
        /// <param name="endPageNum">Page number at which to end listing orders (inclusive). If a number less than the 
        /// starting page number is specified, only a single page of orders will be returned.</param>
        /// <param name="filter">A filter function that will be applied to the returned OrderDetailsResponse
        /// collection. The function should return true for any OrderDetailsResponse objects that should
        /// remain in the set.  If this is null, no filter will be applied.</param>
        /// <param name="orderby">An order function that will be applied to the returned OrderDetailsResponse
        /// collection.  Given two OrderDetailsResponse objects, the function should return 0 if they are
        /// equal, a negative integer if the second object should be placed before the first, and a positive
        /// integer if the second object should be placed after the first.  Note that this is the same pattern
        /// which is used by the .Net framework for IComparable and various types of collection sorting. If
        /// this parameter is null, no ordering will be applied.</param>
        /// <returns>The set of orders at the specified location that match the passed filter object's logic</returns>
        /// <exception cref="LevelUpApiException"> The returned HTTP status code for the request was something other 
        /// than 200 (OK)</exception>
        IList<OrderDetailsResponse> ListFilteredOrders(string accessToken,
            int locationId,
            int startPageNum,
            int endPageNum,
            Func<OrderDetailsResponse, bool> filter = null,
            Func<OrderDetailsResponse, OrderDetailsResponse, int> @orderby = null);
    }
}
