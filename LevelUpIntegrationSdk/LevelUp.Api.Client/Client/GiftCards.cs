//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="GiftCards.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using System.Collections.Generic;
using LevelUp.Api.Client.Models.Requests;
using LevelUp.Api.Client.Models.Responses;
using Newtonsoft.Json;
using RestSharp;

namespace LevelUp.Api.Client
{
    public partial class LevelUpClient
    {
        /// <summary>
        /// Activates and/or adds value to a LevelUp gift card or account
        /// </summary>
        /// <param name="accessToken">The access token retrieved from the Authenticate endpoint</param>
        /// <param name="merchantId">The id of the merchant for which the value added would be spendable</param>
        /// <param name="giftCardQrData">QR Code on the target LevelUp gift card or user account</param>
        /// <param name="valueToAddInCents">The value to add to the account in US Cents</param>
        /// <param name="locationId">The LevelUp location from whence the add value operation originates</param>
        /// <returns>A response indicating the amount of value successfully added</returns>
        public GiftCardAddValueResponse GiftCardAddValue(string accessToken,
                                                         int merchantId,
                                                         int locationId,
                                                         string giftCardQrData,
                                                         int valueToAddInCents)
        {
            return GiftCardAddValue(accessToken,
                                    merchantId,
                                    new GiftCardAddValueRequest(giftCardQrData,
                                                                valueToAddInCents,
                                                                locationId));
        }

        /// <summary>
        /// Activates and/or adds value to a LevelUp gift card or account
        /// </summary>
        /// <param name="accessToken">The access token retrieved from the Authenticate endpoint</param>
        /// <param name="merchantId">The id of the merchant for which the value added would be spendable</param>
        /// <param name="giftCardQrData">QR Code on the target LevelUp gift card or user account</param>
        /// <param name="valueToAddInCents">The value to add to the account in US Cents</param>
        /// <param name="locationId">The LevelUp location from whence the add value operation originates</param>
        /// <param name="identifierFromMerchant">A unique identifier for the check on which this gift card is purchased</param>
        /// <param name="tenderTypes">A collection of the tender type names used to pay for the check</param>
        /// <param name="levelUpOrderId">If applicable, an associated LevelUp order id that paid for this 
        /// add value operation</param>
        /// <returns>A response indicating the amount of value successfully added</returns>
        public GiftCardAddValueResponse GiftCardAddValue(string accessToken,
                                                        int merchantId,
                                                        int locationId,
                                                        string giftCardQrData,
                                                        int valueToAddInCents,
                                                        string identifierFromMerchant,
                                                        IList<string> tenderTypes = null,
                                                        string levelUpOrderId = null)
        {
            return GiftCardAddValue(accessToken,
                                    merchantId,
                                    new GiftCardAddValueRequest(giftCardQrData,
                                                                valueToAddInCents,
                                                                locationId,
                                                                identifierFromMerchant,
                                                                tenderTypes,
                                                                levelUpOrderId));
        }

        /// <summary>
        /// Activates and/or adds value to a LevelUp gift card or account
        /// </summary>
        /// <param name="accessToken">The access token retrieved from the Authenticate endpoint</param>
        /// <param name="merchantId">The id of the merchant for which the value added would be spendable</param>
        /// <param name="addValueRequest">An add value request that contains the QR Code of the target account and
        /// the amount of value to add in US cents</param>
        /// <returns>A response indicating the amount of value successfully added</returns>
        public GiftCardAddValueResponse GiftCardAddValue(string accessToken,
                                                         int merchantId,
                                                         GiftCardAddValueRequest addValueRequest)
        {
            // Build the uri for the add value endpoint
            string giftCardAddValueUri = _endpoints.GiftCardAddValue(merchantId);

            // Create the body content of the add value request
            string body = JsonConvert.SerializeObject(addValueRequest);

            // Make the request
            IRestResponse response = _restService.Post(giftCardAddValueUri, body, accessToken, Identifier.ToString());

            ThrowExceptionOnBadResponseCode(response);

            return JsonConvert.DeserializeObject<GiftCardAddValueResponse>(response.Content);
        }

        /// <summary>
        /// Deletes/Destroys/Removes value from a LevelUp gift card. This method should only be used to correct entry errors
        /// NOTE: This method should NOT be used to redeem gift card value. Use the PlaceOrder method instead to redeem value
        /// </summary>
        /// <param name="accessToken">The access token retrieved from the Authenticate endpoint</param>
        /// <param name="merchantId">The id of the merchant for which the value added would be spendable</param>
        /// <param name="giftCardQrData">QR Code on the target LevelUp gift card or user account</param>
        /// <param name="valueToRemoveInCents">Value to destroy in US Cents</param>
        /// <returns>A response indicating the amount of value successfully destroyed</returns>
        public GiftCardRemoveValueResponse GiftCardDestroyValue(string accessToken,
                                                                int merchantId,
                                                                string giftCardQrData,
                                                                int valueToRemoveInCents)
        {
            return GiftCardDestroyValue(accessToken, merchantId,
                                        new GiftCardRemoveValueRequest(giftCardQrData,
                                                                       valueToRemoveInCents));
        }

        /// <summary>
        /// Deletes/Destroys/Removes value from a LevelUp gift card. This method should only be used to correct entry errors
        /// NOTE: This method should NOT be used to redeem gift card value. Use the PlaceOrder method instead to redeem value
        /// </summary>
        /// <param name="accessToken">The access token retrieved from the Authenticate endpoint</param>
        /// <param name="merchantId">The id of the merchant for which the value added would be spendable</param>
        /// <param name="removeValueRequest">A remove value request that contains the QR code identifying the target
        /// account and the amount of value in US cents to remove</param>
        /// <returns>A response indicating the amount of value successfully destroyed</returns>
        public GiftCardRemoveValueResponse GiftCardDestroyValue(string accessToken,
                                                                int merchantId,
                                                                GiftCardRemoveValueRequest removeValueRequest)
        {
            string giftCardRemoveValueUri = _endpoints.GiftCardRemoveValue(merchantId);

            string body = JsonConvert.SerializeObject(removeValueRequest);

            IRestResponse response = _restService.Post(giftCardRemoveValueUri, body, accessToken, Identifier.ToString());

            ThrowExceptionOnBadResponseCode(response);

            return JsonConvert.DeserializeObject<GiftCardRemoveValueResponse>(response.Content);
        }
    }
}
