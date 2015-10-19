//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="ILevelUpClient.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using LevelUp.Api.Client.Models.Requests;
using LevelUp.Api.Client.Models.Responses;
using System.Collections.Generic;
using LevelUp.Api.Http;

namespace LevelUp.Api.Client
{
    /// <summary>
    /// Interface definition for the LevelUp POS integration API
    /// </summary>
    public interface ILevelUpClient
    {
        /// <summary>
        /// Base Url for LevelUp API
        /// </summary>
        string ApiUrlBase { get; }

        /// <summary>
        /// Obtain a LevelUp access token 
        /// </summary>
        /// <param name="apiKey">Your LevelUp API key which LevelUp will have sent to you. 
        /// This is your LevelUp API key</param>
        /// <param name="username">Your LevelUp username</param>
        /// <param name="password">Your LevelUp password</param>
        /// <returns>A LevelUp access token object which includes a .Token member which stores 
        /// the access token you will need to use for subsequent LevelUp calls</returns>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        AccessToken Authenticate(string apiKey, string username, string password);

        /// <summary>
        /// Associates a new credit card with the user's account.  If the new credit card is the user's only payment
        /// instrument, it will be automatically promoted. If the user has existing payment instruments, no automatic 
        /// promotion will take place; you must call the PromoteCreditCard method
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method</param>
        /// <param name="creditCard">The credit card to be added</param>
        /// <returns>Information about the newly added credit card</returns>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        CreditCard CreateCreditCard(string accessToken, CreditCardRequest creditCard);

        /// <summary>
        /// Creates a new detached refund
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method</param>
        /// <param name="detachedRefund">Data about the detached refund to create</param>
        /// <returns>Information about the newly created detached refund</returns>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        DetachedRefundResponse CreateDetachedRefund(string accessToken, DetachedRefund detachedRefund);

        /// <summary>
        /// Registers and returns a newly-created user
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method</param>
        /// <param name="request">The request to create a user</param>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        User CreateUser(string accessToken, CreateUserRequest request);

        /// <summary>
        /// Deletes a credit card
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method</param>
        /// <param name="creditCardId">The id of the credit card</param>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        void DeleteCreditCard(string accessToken, int creditCardId);

        /// <summary>
        /// Gets the details of the specified contribution target
        /// </summary>
        /// <param name="contributionTargetId">Identifies the contribution target for which to return the details</param>
        /// <returns>Detailed info about the contibution target</returns>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        ContributionTarget GetContributionTarget(string contributionTargetId);

        /// <summary>
        /// Gets the location details for the specified location id
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method</param>
        /// <param name="locationId">Identifies the location for which to return the details</param>
        /// <returns>Detailed location info for the location specified</returns>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        LocationDetails GetLocationDetails(string accessToken, int locationId);

        /// <summary>
        /// Gets details about a loyalty - i.e. the relationship between a user and a merchant.
        /// If a user has no existing loyalty with the merchant, an "empty" loyalty is returned
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method</param>
        /// <param name="merchantId">Identifies the merchant to get the loyalty info for</param>
        /// <returns>Detailed info about the loyalty</returns>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        Loyalty GetLoyalty(string accessToken, int merchantId);

        /// <summary>
        /// Gets the amount of credit the user has at the current location
        /// </summary>
        /// <param name="accessToken">The LevelUp accesstoken obtained from the Authenticate() method</param>
        /// <param name="locationId">Identifies the location for which to return the details</param>
        /// <param name="qrData">The customer's QR code payment data as a string</param>
        /// <returns>Details about the amount of credit available</returns>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        MerchantFundedCreditResponse GetMerchantFundedCredit(string accessToken, int locationId, string qrData);

        /// <summary>
        /// Gets the amount of credit the user has at the current location based on the items on the check passed in
        /// </summary>
        /// <param name="accessToken">The LevelUp accesstoken obtained from the Authenticate() method</param>
        /// <param name="locationId">Identifies the location for which to return the details</param>
        /// <param name="qrData">The customer's QR code payment data as a string</param>
        /// <param name="identifierFromMerchant">A unique identifier for the check at the merchant location</param>
        /// <param name="items">A collection of items which may or may not contain items eligible for discount</param>
        /// <returns>Details about the amount of credit available</returns>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        MerchantFundedCreditResponse GetMerchantFundedCredit(string accessToken,
                                                             int locationId,
                                                             string qrData,
                                                             string identifierFromMerchant,
                                                             IList<Item> items);

        /// <summary>
        /// Gets the order details for a given order. This the merchant facing order data so the merchant id is required as well
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method</param>
        /// <param name="merchantId">Identifies the merchant on whose behalf you are querying. 
        /// If the access token specified does not belong to the merchant specified, this method will throw an exception</param>
        /// <param name="orderIdentifier">Identifies the order for which to return the details. This should be the order UUID</param>
        /// <returns>Details for the specified order</returns>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        OrderDetailsResponse GetMerchantOrderDetails(string accessToken, int merchantId, string orderIdentifier);

        /// <summary>
        /// This endpoint returns details about a user account. Normal users, including merchants, may only retrieve their own user details. Admins may retrieve any user
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method</param>
        /// <param name="userId">Identifies the user to get</param>
        /// <returns>Detailed info about the user</returns>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        User GetUser(string accessToken, int userId);

        /// <summary>
        /// Retrieve a user's active payment token
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method</param>
        /// <returns>Current payment token for the user</returns>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        PaymentToken GetPaymentToken(string accessToken);

        /// <summary>
        /// Activates and/or adds value to a gift card for a merchant
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method</param>
        /// <param name="merchantId">The id merchant to which the gift card applies. Only locations associated 
        /// with this merchant id will be able to redeem gift card value associated with the specified QR code</param>
        /// <param name="addValueRequest">An add value request model that contains the gift card qr code and 
        /// the amount in cents to add</param>
        /// <returns>A response indicating the amount on the card before, the new amount, and the amount added</returns>
        GiftCardAddValueResponse GiftCardAddValue(string accessToken,
                                                  int merchantId,
                                                  GiftCardAddValueRequest addValueRequest);

        /// <summary>
        /// Activates and/or adds value to a gift card for a merchant
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method</param>
        /// <param name="merchantId">The id merchant to which the gift card applies. Only locations associated 
        /// with this merchant id will be able to redeem gift card value associated with the specified QR code</param>
        /// <param name="locationId">The LevelUp location id from whence this add value operation originates</param>
        /// <param name="giftCardQrData">QR code on the gift card that should have value added to it</param>
        /// <param name="valueToAddInCents">Value to add to the card in US cents</param>
        /// <returns>A response indicating the amount on the card before, the new amount, and the amount added</returns>
        GiftCardAddValueResponse GiftCardAddValue(string accessToken,
                                                  int merchantId,
                                                  int locationId,
                                                  string giftCardQrData,
                                                  int valueToAddInCents);

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
        GiftCardAddValueResponse GiftCardAddValue(string accessToken,
                                                  int merchantId,
                                                  int locationId,
                                                  string giftCardQrData,
                                                  int valueToAddInCents,
                                                  string identifierFromMerchant,
                                                  IList<string> tenderTypes = null,
                                                  string levelUpOrderId = null);

        /// <summary>
        /// Deletes/Destroys/Removes value from a LevelUp gift card. This method should only be used to correct entry errors
        /// NOTE: This method should NOT be used to redeem gift card value. Use the PlaceOrder() method instead to redeem value
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method</param>
        /// <param name="merchantId">The id merchant to which the gift card applies. Only locations associated 
        /// with this merchant id will be able to redeem gift card value associated with the specified QR code</param>
        /// <param name="removeValueRequest">A remove value request model that contains the gift card qr code and
        /// the amount in cents to remove</param>
        /// <returns>A response indicating the amount on the gift card before, the new amount, and the amount removed</returns>
        GiftCardRemoveValueResponse GiftCardDestroyValue(string accessToken,
                                                        int merchantId,
                                                        GiftCardRemoveValueRequest removeValueRequest);

        /// <summary>
        /// Deletes/Destroys/Removes value from a LevelUp gift card. This method should only be used to correct entry errors
        /// NOTE: This method should NOT be used to redeem gift card value. Use the PlaceOrder() method instead to redeem value
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method</param>
        /// <param name="merchantId">The id merchant to which the gift card applies. Only locations associated 
        /// with this merchant id will be able to redeem gift card value associated with the specified QR code</param>
        /// <param name="giftCardQrData">QR code on the gift card that should have value removed from it</param>
        /// <param name="valueToRemoveInCents">Value to remove from the card in US cents</param>
        /// <returns>A response indicating the amount on the gift card before, the new amount, and the amount removed</returns>
        GiftCardRemoveValueResponse GiftCardDestroyValue(string accessToken,
                                                        int merchantId,
                                                        string giftCardQrData,
                                                        int valueToRemoveInCents);

        /// <summary>
        /// List of all contribution targets to which users may donate their LevelUp savings.
        /// </summary>
        /// <returns>A list of all the contribution targets.</returns>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        IList<ContributionTarget> ListContributionTargets();

        /// <summary>
        /// Returns a list of the current user's active credit cards. Inactive cards include deleted cards and 
        /// duplicate cards. These records will not appear in the list.
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method</param>
        /// <returns>List of credit cards associated with the user's account</returns>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        IList<CreditCard> ListCreditCards(string accessToken);

        /// <summary>
        /// Lists all locations for a specified merchant
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method. 
        /// This will be the .Token member on the returned object</param>
        /// <param name="merchantId">Identifies the merchant for which to return a list of locations</param>
        /// <returns>A list of locations for the specified merchant.</returns>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        IList<Location> ListLocations(string accessToken, int merchantId);

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
        IList<OrderDetailsResponse> ListOrders(string accessToken,
                                               int locationId,
                                               int startPageNum = 1,
                                               int endPageNum = 1);

        /// <summary>
        /// Returns a list of a user's associated street addresses.
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
        IList<OrderDetailsResponse> ListOrders(string accessToken,
                                               int locationId,
                                               int startPageNum,
                                               int endPageNum,
                                               out bool areThereMorePages);

        /// <summary>
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method</param>
        /// <returns>A list of the user's addresses</returns>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        IList<UserAddress> ListUserAddresses(string accessToken);

        /// <summary>
        /// Issues a password reset request
        /// </summary>
        /// <param name="email">The users email address</param>
        void PasswordResetRequest(string email);

        /// <summary>
        /// Place an order and pay through LevelUp
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method. 
        /// This will be the .Token member on the returned object</param>
        /// <param name="orderData">An object containing the order data and spend amounts to process through LevelUp</param>
        /// <returns>A response object indicating whether the order was charged successfully and 
        /// the final amount paid including the customer specified tip amount</returns>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        OrderResponse PlaceOrder(string accessToken, Order orderData);

        /// <summary>
        /// Promotes a user's credit card so that it will be used as their preferred payment instrument. Only one 
        /// credit card at a time may be promoted. Promoting a card will demote any other promoted card.
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method</param>
        /// <param name="creditCardId">The id of the credit card</param>
        /// <returns>Information about the promoted credit card</returns>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        CreditCard PromoteCreditCard(string accessToken, int creditCardId);

        /// <summary>
        /// Refund an order placed through LevelUp.
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method. 
        /// This will be the .Token member on the returned object</param>
        /// <param name="orderIdentifier">UUID for the order to refund. 
        /// This information is returned when the order is placed</param>
        /// <param name="managerConfirmation">Some systems require manager confirmation. 
        /// This field should be omitted or set to null if your system does not require manager confirmation</param>
        /// <returns>A response object indicating whether the refund was successfull</returns>
        /// <exception cref="LevelUpApiException">Thrown when the order to refund could not be found or 
        /// when the HTTP status returned was not 200 OK</exception>
        RefundResponse RefundOrder(string accessToken,
                                   string orderIdentifier,
                                   string managerConfirmation = null);

        /// <summary>
        /// Updates a user
        /// </summary>
        /// <param name="accessToken">The LevelUp access token obtained from the Authenticate() method</param>
        /// <param name="request">The request to update the user</param>
        /// <exception cref="LevelUpApiException">Thrown when the HTTP status returned was not 200 OK</exception>
        User UpdateUser(string accessToken, UpdateUserRequest request);
    }
}
