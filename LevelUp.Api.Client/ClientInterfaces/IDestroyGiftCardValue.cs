#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="IDestroyGiftCardValue.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

namespace LevelUp.Api.Client.ClientInterfaces
{
    public interface IDestroyGiftCardValue : ILevelUpClientModule
    {
        /// <summary>
        /// Reverses specific GiftCard addition transaction by removing the added value from the GiftCard balance.
        /// </summary>
        /// <param name="accessToken">The LevelUp access token associated with the merchant account obtained from 
        /// the Authenticate() method</param>
        /// <param name="merchantId">The ID of the merchant to which the gift card applies.</param>
        /// <param name="giftCardQrData">QR code associated with the gift card that should have value removed from it.</param>
        /// <param name="giftCardTransactionUuid">GiftCard Add Value Transaction UUID to reverse.<seealso cref="GiftCardAddValueResponse"/></param>
        /// <returns>A response indicating the amount on the gift card before, the new amount, and the amount removed</returns>
        GiftCardRemoveValueResponse GiftCardDestroyValue(
            string accessToken,
            int merchantId,
            string giftCardQrData,
            Guid giftCardTransactionUuid);

        /// <summary>
        /// Deletes/Destroys/Removes value from a LevelUp gift card. This method should only be used to correct entry 
        /// errors. Note that this method should NOT be used to redeem gift card value. To redeem value, simply create
        /// an order using the standard order methods.
        /// </summary>
        /// <param name="accessToken">The LevelUp access token associated with the merchant account obtained from 
        /// the Authenticate() method</param>
        /// <param name="merchantId">The ID of the merchant to which the gift card applies.</param>
        /// <param name="removeValueRequest">An object containing the gift card qr code and the amount to remove.</param>
        /// <returns>A response indicating the amount on the gift card before, the new amount, and the amount removed</returns>
        GiftCardRemoveValueResponse GiftCardDestroyValue(
            string accessToken,
            int merchantId,
            GiftCardRemoveValueRequestBody removeValueRequest);

        /// <summary>
        /// Deletes/Destroys/Removes value from a LevelUp gift card. This method should only be used to correct entry 
        /// errors. Note that this method should NOT be used to redeem gift card value. To redeem value, simply create
        /// an order using the standard order methods.
        /// </summary>
        /// <param name="accessToken">Access token for the location</param>
        /// <param name="merchantId">The merchant Id</param>
        /// <param name="giftCardQrData">The qr code of the target card or account</param>
        /// <param name="valueToRemoveInCents">The amount of value to destroy in US Cents</param>
        /// <returns>A response indicating the amount on the gift card before, the new amount, and the amount removed</returns>
        GiftCardRemoveValueResponse GiftCardDestroyValue(
            string accessToken,
            int merchantId,
            string giftCardQrData,
            int valueToRemoveInCents);
    }
}
