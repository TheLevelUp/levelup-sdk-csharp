#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="IRetrieveMerchantFundedCredit.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using LevelUp.Api.Client.Models.Requests;
using LevelUp.Api.Client.Models.Responses;
using LevelUp.Api.Http;

namespace LevelUp.Api.Client.ClientInterfaces
{
    public interface IRetrieveMerchantFundedCredit : ILevelUpClientModule
    {
        /// <summary>
        /// Gets the amount of credit the user has at the current location
        /// </summary>
        /// <param name="accessToken">The LevelUp access token associated with the merchant account obtained from 
        /// the Authenticate() method</param>
        /// <param name="locationId">LevelUp Location ID to query</param>
        /// <param name="qrData">The customer's scanned QR code payment data</param>
        /// <returns>Details about the amount of credit available</returns>
        /// <exception cref="LevelUpApiException"> The returned HTTP status code for the request was something other 
        /// than 200 (OK)</exception>
        MerchantFundedCreditResponse GetMerchantFundedCredit(string accessToken, int locationId, string qrData);

        /// <summary>
        /// Gets the amount of credit the user has at the current location. This will include any item-specific 
        /// discounts or campaigns related to items on the current check.
        /// </summary>
        /// <param name="accessToken">The LevelUp access token associated with the merchant account obtained from 
        /// the Authenticate() method</param>
        /// <param name="locationId">>LevelUp Location ID to query</param>
        /// <param name="qrData">The customer's scanned QR code payment data</param>
        /// <param name="identifierFromMerchant">An unique order identifier specific to the POS system which will be 
        /// used to resolved possible duplicate orders. This should be the POS internal order number in most cases.</param>
        /// <param name="items">A list of items that comprise the order</param>
        /// <exception cref="LevelUpApiException"> The returned HTTP status code for the request was something other 
        /// than 200 (OK)</exception>
        MerchantFundedCreditResponse GetMerchantFundedCredit(string accessToken,
            int locationId,
            string qrData,
            string identifierFromMerchant,
            IList<Item> items);
    }
}
