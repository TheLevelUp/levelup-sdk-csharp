#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="IQueryMerchantData.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using LevelUp.Api.Http;

namespace LevelUp.Api.Client.ClientInterfaces
{
    public interface IQueryMerchantData : ILevelUpClientModule
    {
        /// <summary>
        /// Gets the location details for the specified location id
        /// </summary>
        /// <param name="accessToken">The LevelUp access token associated with the merchant account obtained from 
        /// the Authenticate() method</param>
        /// <param name="locationId">LevelUp Location ID to query.</param>
        /// <returns>Detailed location info for the specified location</returns>
        /// <exception cref="LevelUpApiException"> The returned HTTP status code for the request was something other 
        /// than 200 (OK)</exception>
        LocationDetails GetLocationDetails(string accessToken, int locationId);

        /// <summary>
        /// Gets the order details for a given order. This the merchant facing order data so the merchant id is required as well
        /// </summary>
        /// <param name="accessToken">The LevelUp access token associated with the merchant account obtained from 
        /// the Authenticate() method</param>
        /// <param name="merchantId">The LevelUp ID for the merchant to query.  Note that the specified access token 
        /// must belong to the merchant who owns this merchant ID.</param>
        /// <param name="orderIdentifier">The LevelUp order ID (UUID) to query. This should be the order identifier 
        /// returned from a successful PlaceOrder or CompleteProposedOrder call.</param>
        /// <returns>Details for the specified order</returns>
        /// <exception cref="LevelUpApiException"> The returned HTTP status code for the request was something other 
        /// than 200 (OK)</exception>
        OrderDetailsResponse GetMerchantOrderDetails(string accessToken, int merchantId, string orderIdentifier);

        /// <summary>
        /// Lists all locations for a specified merchant
        /// </summary>
        /// <param name="accessToken">The LevelUp access token associated with the merchant account obtained from 
        /// the Authenticate() method</param>
        /// <param name="merchantId">The LevelUp ID for the merchant to query.</param>
        /// <returns>A list of locations for the specified merchant.</returns>
        /// <exception cref="LevelUpApiException"> The returned HTTP status code for the request was something other 
        /// than 200 (OK)</exception>
        IList<Location> ListLocations(string accessToken, int merchantId);

        /// <summary>
        /// Lists all locations that are managed by the authorized user.
        /// </summary>
        /// <param name="accessToken">The LevelUp access token associated with the merchant account obtained from 
        /// the Authenticate() method</param>
        /// <returns>A list of locations managed by the authorized user.</returns>
        /// <exception cref="LevelUpApiException"> The returned HTTP status code for the request was something other 
        /// than 200 (OK)</exception>
        IList<ManagedLocation> ListManagedLocations(string accessToken);
    }
}
