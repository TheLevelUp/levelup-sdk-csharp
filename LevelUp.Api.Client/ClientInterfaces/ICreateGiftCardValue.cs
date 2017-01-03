#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="ICreateGiftCardValue.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

namespace LevelUp.Api.Client.ClientInterfaces
{
    public interface ICreateGiftCardValue : ILevelUpClientModule
    {
        /// <summary>
        /// Activates and/or adds value to a gift card.  The gift card value is attached to a QR code, which can either
        /// be the QR code associated with a LevelUp user's account, or can be printed on a physical gift card.
        /// </summary>
        /// <remarks>
        /// When integrating gift cards into a POS system, the gift card value should not be created until after the 
        /// check/order is fully paid but before closing the check. Note also that the minimum amount that can be 
        /// loaded on a gift card is $10.
        /// </remarks>
        /// <param name="accessToken">The LevelUp access token associated with the merchant account that is retrieved 
        /// from the Authenticate() method</param>
        /// <param name="merchantId">The ID of the merchant to which the gift card applies. Only locations associated 
        /// with this merchant ID will be able to redeem gift card value for the attached QR code</param>
        /// <param name="giftCardQrData">QR Code on the target LevelUp gift card or user account</param>
        /// <param name="valueToAddInCents">The value to add to the account in US Cents</param>
        /// <param name="locationId">The location ID of the location that is giving the money for the gift card.</param>
        /// <param name="identifierFromMerchant">The POS-specific order ID or number for the check</param>
        /// <param name="tenderTypes">A collection of the tender type names used to pay for the check (levelup, cash, 
        /// credit_card, gift_card, comp, house_account etc.)</param>
        /// <param name="levelUpOrderId">If applicable, an associated LevelUp order ID (UUID) that paid for this 
        /// add value operation</param>
        /// <returns>A response indicating the amount of value successfully added</returns>
        GiftCardAddValueResponse GiftCardAddValue(string accessToken,
                                                  int merchantId,
                                                  int locationId,
                                                  string giftCardQrData,
                                                  int valueToAddInCents,
                                                  string identifierFromMerchant,
                                                  IList<string> tenderTypes = null,
                                                  string levelUpOrderId = null);

        /// <summary>
        /// Activates and/or adds value to a gift card.  The gift card value is attached to a QR code, which can either
        /// be the QR code associated with a LevelUp user's account, or can be printed on a physical gift card.
        /// </summary>
        /// <remarks>
        /// When integrating gift cards into a POS system, the gift card value should not be created until after the 
        /// check/order is fully paid but before closing the check. Note also that the minimum amount that can be 
        /// loaded on a gift card is $10.
        /// </remarks>
        /// <param name="accessToken">The LevelUp access token associated with the merchant account that is retrieved 
        /// from the Authenticate() method</param>
        /// <param name="merchantId">The ID of the merchant to which the gift card applies. Only locations associated 
        /// with this merchant ID will be able to redeem gift card value for the attached QR code</param>
        /// <param name="locationId">The location ID of the location that is giving the money for the gift card.</param>
        /// <param name="giftCardQrData">QR Code on the target LevelUp gift card or user account</param>
        /// <param name="valueToAddInCents">The value to add to the account in US Cents</param>
        /// <returns>A response indicating the amount on the card before, the new amount, and the amount added</returns>
        GiftCardAddValueResponse GiftCardAddValue(string accessToken,
                                                  int merchantId,
                                                  int locationId,
                                                  string giftCardQrData,
                                                  int valueToAddInCents);

        /// <summary>
        /// Activates and/or adds value to a gift card.  The gift card value is attached to a QR code, which can either
        /// be the QR code associated with a LevelUp user's account, or can be printed on a physical gift card.
        /// </summary>
        /// <remarks>
        /// When integrating gift cards into a POS system, the gift card value should not be created until after the 
        /// check/order is fully paid but before closing the check. Note also that the minimum amount that can be 
        /// loaded on a gift card is $10.
        /// </remarks>
        /// <param name="accessToken">The LevelUp access token associated with the merchant account that is retrieved 
        /// from the Authenticate() method</param>
        /// <param name="merchantId">The ID of the merchant to which the gift card applies. Only locations associated 
        /// with this merchant ID will be able to redeem gift card value for the attached QR code</param>
        /// <param name="addValueRequest">An object containing details about the gift card value amount, etc.</param>
        /// <returns>A response indicating the amount on the card before, the new amount, and the amount added</returns>
        GiftCardAddValueResponse GiftCardAddValue(string accessToken,
                                                  int merchantId,
                                                  GiftCardAddValueRequestBody addValueRequest);
    }
}
