#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="ICreateOrders.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using LevelUp.Api.Client.Models.Requests;
using LevelUp.Api.Client.Models.Responses;
using LevelUp.Api.Http;

namespace LevelUp.Api.Client.ClientInterfaces
{
    public interface ICreateOrders : ILevelUpClientModule
    {
        /// <summary>
        /// Place an order and pay through LevelUp.
        /// </summary>
        /// <remarks> We encourage users of this SDK to use the CreateProposedOrder/CompleteProposedOrder workflow
        /// to create orders in lieu of this method.  The Proposed Order workflow leverages the LevelUp platform to 
        /// calculate discount, both merchant-funded and non-merchant-funded, to apply to the check and ultimately
        /// requires less implementation work (vis-a-vis determining what amount of discount to apply) on the 
        /// part of the integrator. </remarks>
        /// <param name="accessToken">The LevelUp access token associated with the merchant account obtained from 
        /// the Authenticate() method</param>
        /// <param name="orderData">An object containing the order data and spend amounts to process through LevelUp</param>
        /// <returns>A response object indicating whether the order was charged successfully and 
        /// the final amount paid including the customer specified tip amount</returns>
        /// <exception cref="LevelUpApiException"> The returned HTTP status code for the request was something other 
        /// than 200 (OK)</exception>
        [Obsolete("Refer instead to the Proposed Orders workflow in IManageProposedOrders.cs")]
        OrderResponse PlaceOrder(string accessToken, Order orderData);
    }
}
