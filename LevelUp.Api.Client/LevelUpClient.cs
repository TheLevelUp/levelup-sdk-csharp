#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="LevelUpClient.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using System.Collections.Generic;
using System.Net;
using LevelUp.Api.Client.Models.Requests;
using LevelUp.Api.Client.Models.Responses;
using LevelUp.Api.Http;
using LevelUp.Pos.ProposedOrders;

namespace LevelUp.Api.Client
{
    /// <summary>
    /// A unified implementation for all of the various client interfaces.  
    /// </summary>
    internal class LevelUpClient : ILevelUpClientSuperset, ILevelUpClient
    {
        private readonly LevelUpRestWrapper _restWrapper;
        private readonly LevelUpEnvironment _targetEnviornment;

        internal LevelUpClient(IRestfulService restService, AgentIdentifier identifier, LevelUpEnvironment targetEnviornment)
        {
            _restWrapper = new LevelUpRestWrapper(restService, identifier);
            _targetEnviornment = targetEnviornment;
        }

        #region IAuthenticate Implementation
        
        public AccessToken Authenticate(string apiKey, string username, string password)
        {
            AccessTokenRequest request = new AccessTokenRequest(apiKey, username, password);

            return _restWrapper.Post<AccessTokenRequestBody, AccessToken>(
                request.Body,
                BuildUri(request.ApiVersion, "access_tokens"),
                actions: null);
        }

        #endregion

        #region ICreateCreditCards Implementation

        public CreditCard CreateCreditCard(string accessToken, string encryptedNumber, string encryptedExpirationMonth,
            string encryptedExpirationYear, string encryptedCvv, string postalCode)
        {
            CreateCreditCardRequest request = new CreateCreditCardRequest(accessToken, encryptedNumber,
                encryptedExpirationMonth, encryptedExpirationYear, encryptedCvv, postalCode);

            return _restWrapper.Post<CreateCreditCardRequestBody, CreditCard>(
                request.Body,
                uri: BuildUri(request.ApiVersion, "credit_cards"),
                accessTokenHeader: FormatAccessTokenString(unspecifiedUserAccessToken: request.AccessToken),
                actions: null);
        }
        
        public CreditCard CreateCreditCard(string accessToken, CreateCreditCardRequestBody createCreditCard)
        {
            return CreateCreditCard(accessToken, createCreditCard.EncryptedNumber, 
                createCreditCard.EncryptedExpirationMonth, createCreditCard.EncryptedExpirationYear, 
                createCreditCard.EncryptedCvv, createCreditCard.PostalCode);
        }

        #endregion

        #region ICreateDetachedRefund Implementation

        public DetachedRefundResponse CreateDetachedRefund(string accessToken, 
                              int locationId,
                              string qrPaymentData,
                              int creditAmountCents,
                              string register = null,
                              string cashier = null,
                              string identifierFromMerchant = null,
                              string managerConfirmation = null,
                              string customerFacingReason = null,
                              string internalReason = null)
        {
            DetachedRefundRequest request = new DetachedRefundRequest(accessToken, locationId, qrPaymentData, creditAmountCents, 
                register, cashier, identifierFromMerchant, managerConfirmation, customerFacingReason, internalReason);

            return _restWrapper.Post<DetachedRefundRequestBody, DetachedRefundResponse>(
                request.Body,
                uri: BuildUri(request.ApiVersion, "detached_refunds"),
                accessTokenHeader: FormatAccessTokenString(unspecifiedUserAccessToken: request.AccessToken),
                actions: null);
        }

        public DetachedRefundResponse CreateDetachedRefund(string accessToken, DetachedRefundRequestBody detachedRefund)
        {
            return CreateDetachedRefund(accessToken, detachedRefund.LocationId, detachedRefund.PaymentData, 
                detachedRefund.CreditAmountCents, detachedRefund.Register, detachedRefund.Cashier, 
                detachedRefund.Identifier, detachedRefund.ManagerConfirmation, detachedRefund.CustomerFacingReason, 
                detachedRefund.InternalReason);
        }

        #endregion

        #region ICreateRefund Implementation
        
        public RefundResponse RefundOrder(string accessToken, string orderIdentifier, string managerConfirmation = null)
        {
            RefundRequest request = new RefundRequest(accessToken, orderIdentifier, managerConfirmation);
            var accessTokenHeader = (request.ApiVersion == Http.LevelUpApiVersion.v14) ? 
                FormatAccessTokenString(unspecifiedUserAccessToken: request.AccessToken) :
                FormatAccessTokenString(merchantUserAccessToken: request.AccessToken);

            return _restWrapper.Post<RefundRequestBody, RefundResponse>(
                request.Body,
                uri: BuildUri(request.ApiVersion, $"orders/{request.OrderIdentifier}/refund"),
                accessTokenHeader: accessTokenHeader,
                actions: null);
        }

        #endregion

        #region IDestroyCreditCards Implementation

        public void DeleteCreditCard(string accessToken, int creditCardId)
        {
            DeleteCreditCardRequest request = new DeleteCreditCardRequest(accessToken, creditCardId);

            _restWrapper.Delete(
                uri: BuildUri(request.ApiVersion, $"credit_cards/{request.CardId}"),
                accessTokenHeader: FormatAccessTokenString(unspecifiedUserAccessToken: request.AccessToken),
                actions: null);
        }

        #endregion

        #region ILookupUserLoyalty Implementation

        public Loyalty GetLoyalty(string accessToken, int merchantId)
        {
            UserLoyaltyQueryRequest request = new UserLoyaltyQueryRequest(accessToken, merchantId);

            return _restWrapper.Get<Loyalty>(
                uri: BuildUri(request.ApiVersion, $"merchants/{request.MerchantId}/loyalty"),
                accessTokenHeader: FormatAccessTokenString(consumerUserAccessToken: request.AccessToken),
                actions: null);
        }

        #endregion

        #region IModifyUser Implementation

        public User CreateUser(string apiKey, string firstName, string lastName, string email, string password)
        {
            CreateUserRequest request = new CreateUserRequest(apiKey, firstName, lastName, email, password);

            return _restWrapper.Post<CreateUserRequestBody, User>(
                request.Body,
                uri: BuildUri(request.ApiVersion, "users"),
                accessTokenHeader: FormatAccessTokenString(unspecifiedUserAccessToken: request.AccessToken),
                actions: null);
        }

        public User CreateUser(string apiKey, CreateUserRequestBodyUserSection requestBody)
        {
            return CreateUser(apiKey, requestBody.FirstName, requestBody.LastName, requestBody.Email, requestBody.Password);
        }

        public User UpdateUser(string accessToken, UpdateUserRequestBody requestBody)
        {
            UpdateUserRequest request = new UpdateUserRequest(accessToken, requestBody);

            return _restWrapper.Put<UpdateUserRequestBody, User>(
                request.Body,
                uri: BuildUri(request.ApiVersion, $"users/{request.Body.Id}"),
                accessTokenHeader: FormatAccessTokenString(unspecifiedUserAccessToken: request.AccessToken),
                actions: null);
        }

        public void PasswordResetRequest(string email)
        {
            PasswordResetRequest request = new PasswordResetRequest(email);
            _restWrapper.Post<PasswordResetRequestBody>(
                request.Body, 
                uri: BuildUri(request.ApiVersion, "passwords"), 
                actions: null);
        }

        #endregion

        #region IQueryCreditCards Implementation

        public IList<CreditCard> ListCreditCards(string accessToken)
        {
            CreditCardQueryRequest request = new CreditCardQueryRequest(accessToken);

            return _restWrapper.Get<List<CreditCard>>(
                uri: BuildUri(request.ApiVersion, "credit_cards"),
                accessTokenHeader: FormatAccessTokenString(unspecifiedUserAccessToken: request.AccessToken),
                actions: null);
        }

        #endregion

        #region IQueryMerchantData Implementation
        
        public LocationDetails GetLocationDetails(string accessToken, int locationId)
        {
            // Special case 404 error since this means the location does not exist, is not visible, 
            // or the merchant owner of the location is not live
            var custom_response = new Dictionary<HttpStatusCode, LevelUpRestWrapper.ResponseAction>
            {
                {
                    HttpStatusCode.NotFound,
                    response =>
                    {
                        if (response.StatusCode == HttpStatusCode.NotFound)
                        {
                            throw new LevelUpApiException($"Cannot get location details for location {locationId}." +
                                " This location may not exist, not be visible, or the merchant owner may not be live.",
                                response.StatusCode,
                                response.ErrorException);
                        }
                    }
                }
            };

            LocationDetailsQueryRequest request = new LocationDetailsQueryRequest(accessToken, locationId);

            return _restWrapper.Get<LocationDetails>(
                uri: BuildUri(request.ApiVersion, $"locations/{request.LocationId}"),
                accessTokenHeader: FormatAccessTokenString(unspecifiedUserAccessToken: request.AccessToken),
                actions: custom_response);
        }

        public OrderDetailsResponse GetMerchantOrderDetails(string accessToken, int merchantId, string orderIdentifier)
        {
            MerchantOrderDetailsRequest request = new MerchantOrderDetailsRequest(accessToken, merchantId, orderIdentifier);

            return _restWrapper.Get<OrderDetailsResponse>(
                uri: BuildUri(request.ApiVersion, $"merchants/{request.MerchantId}/orders/{request.OrderIdentifier}"),
                accessTokenHeader: FormatAccessTokenString(unspecifiedUserAccessToken: request.AccessToken),
                actions: null);
        }

        public IList<Location> ListLocations(string accessToken, int merchantId)
        {
            LocationQueryRequest request = new LocationQueryRequest(accessToken, merchantId);

            return _restWrapper.Get<List<Location>>(
                uri: BuildUri(request.ApiVersion, $"merchants/{request.MerchantId}/locations"),
                accessTokenHeader: FormatAccessTokenString(unspecifiedUserAccessToken: request.AccessToken),
                actions: null);
        }

        public IList<ManagedLocation> ListManagedLocations(string accessToken)
        {
            ManagedLocationQueryRequest request = new ManagedLocationQueryRequest(accessToken);

            return _restWrapper.Get<List<ManagedLocation>>(
                uri: BuildUri(request.ApiVersion, "managed_locations"),
                accessTokenHeader: FormatAccessTokenString(merchantUserAccessToken: request.AccessToken),
                actions: null);
        }

        #endregion

        #region IQueryOrders Implementation

        public IList<OrderDetailsResponse> ListOrders(string userAccessToken, int startPageNum = 1, int endPageNum = 1)
        {
            return ListOrders(userAccessToken, startPageNum, endPageNum, out _);
        }

        public IList<OrderDetailsResponse> ListOrders(string userAccessToken, int startPageNum, int endPageNum, out bool areThereMorePages)
        {
            if (endPageNum < startPageNum)
            {
                endPageNum = startPageNum;
            }

            ListOrderQueryRequest request = new ListOrderQueryRequest(userAccessToken, startPageNum);

            PagedList<OrderDetailsResponse> orders = _restWrapper.GetWithPaging<OrderDetailsResponse>(
                uri: GetOrderQueryRequestEndpoint(request),
                accessTokenHeader: FormatAccessTokenString(unspecifiedUserAccessToken: request.AccessToken), 
                currentPageNumber: request.PageNumber);
            
            // Repackage the returned PagedList as an IList<OrderDetailsResponse>
            List<OrderDetailsResponse> retval = new List<OrderDetailsResponse>();

            do
            {
                retval.AddRange(orders);
                areThereMorePages = orders.HasNextPage;
                if (areThereMorePages)
                {
                    orders = orders.NextPage();
                }
            } while (areThereMorePages && orders.CurrentPage <= endPageNum);

            return retval;
        }

        public IList<OrderDetailsResponse> ListFilteredOrders(string userAccessToken, int startPageNum, int endPageNum,
            Func<OrderDetailsResponse, bool> filter = null, Func<OrderDetailsResponse, OrderDetailsResponse, int> @orderby = null)
        {
            var allOrders = new List<OrderDetailsResponse> (ListOrders(userAccessToken, startPageNum, endPageNum));

            List<OrderDetailsResponse> filtered = (filter == null) ? allOrders : allOrders.FindAll(x => filter(x));

            if (null != orderby)
            {
                filtered.Sort((x, y) => @orderby(x, y));
            }

            return filtered;
        }

        private string GetOrderQueryRequestEndpoint(ListOrderQueryRequest request)
        {
            string path = $"apps/orders";
            LevelUpUriBuilder builder = new LevelUpUriBuilder(_targetEnviornment);
            builder.SetApiVersion(request.ApiVersion).SetPath(path);

            if (request.PageNumber > 1)
            {
                builder.AppendQuery("page", request.PageNumber.ToString());
            }

            return builder.Build();
        }

        #endregion

        #region IQueryUser Implementation

        public IList<UserAddress> ListUserAddresses(string accessToken)
        {
            UserAddressesQueryRequest request = new UserAddressesQueryRequest(accessToken);

            return _restWrapper.Get<List<UserAddress>>(
                uri: BuildUri(request.ApiVersion, "user_addresses"),
                accessTokenHeader: FormatAccessTokenString(unspecifiedUserAccessToken: request.AccessToken),
                actions: null);
        }

        public User GetUser(string accessToken, int userId)
        {
            UserDetailsQueryRequest request = new UserDetailsQueryRequest(accessToken, userId);

            return _restWrapper.Get<User>(
                uri: BuildUri(request.ApiVersion, $"users/{request.UserId}"),
                accessTokenHeader: FormatAccessTokenString(unspecifiedUserAccessToken: request.AccessToken),
                actions: null);
        }

        public Credit GetLocationUserCredit(string userAccessToken, int locationId)
        {
            GetCreditRequest request = new GetCreditRequest(userAccessToken);

            return _restWrapper.Get<Credit>(
                uri: BuildUri(request.ApiVersion, $"locations/{locationId}/credit"),
                accessTokenHeader: FormatAccessTokenString(consumerUserAccessToken: request.AccessToken),
                actions: null);
        }

        #endregion

        #region IRetrievePaymentToken Implementation

        public Models.Responses.PaymentToken GetPaymentToken(string accessToken)
        {
            PaymentTokenQueryRequest request = new PaymentTokenQueryRequest(accessToken);

            return _restWrapper.Get<PaymentToken>(uri: BuildUri(request.ApiVersion, "payment_token"),
                accessTokenHeader: FormatAccessTokenString(unspecifiedUserAccessToken: request.AccessToken),
                actions: null);
        }

        #endregion

        #region ICreateGiftCardValue Implementation

        public GiftCardAddValueResponse GiftCardAddValue(string accessToken, int merchantId, GiftCardAddValueRequestBody addValueRequest)
        {
            GiftCardAddValueRequest request = new GiftCardAddValueRequest(accessToken, merchantId, addValueRequest);

            return _restWrapper.Post<GiftCardAddValueRequestBody, GiftCardAddValueResponse>(
                request.Body,
                uri: BuildUri(request.ApiVersion, $"merchants/{request.MerchantId}/gift_card_value_additions"),
                accessTokenHeader: FormatAccessTokenString(unspecifiedUserAccessToken: request.AccessToken),
                actions: null);
        }

        public GiftCardAddValueResponse GiftCardAddValue(string accessToken, int merchantId, int locationId, string giftCardQrData, int valueToAddInCents)
        {
            return GiftCardAddValue(accessToken, merchantId, locationId, giftCardQrData, valueToAddInCents, string.Empty, null, null);
        }

        public GiftCardAddValueResponse GiftCardAddValue(string accessToken, int merchantId, int locationId, string giftCardQrData, 
            int valueToAddInCents, string identifierFromMerchant, IList<string> tenderTypes = null, string levelUpOrderId = null)
        {
            var requestBody = new GiftCardAddValueRequestBody(giftCardQrData, valueToAddInCents, locationId,
                identifierFromMerchant, tenderTypes, levelUpOrderId);

            return GiftCardAddValue(accessToken, merchantId, requestBody);
        }


        #endregion

        #region IDestroyGiftCardValue Implementation

        public GiftCardRemoveValueResponse GiftCardDestroyValue(string accessToken, int merchantId, GiftCardRemoveValueRequestBody removeValueRequest)
        {
            GiftCardRemoveValueRequest request = new GiftCardRemoveValueRequest(accessToken, merchantId, removeValueRequest);

            return _restWrapper.Post<GiftCardRemoveValueRequestBody, GiftCardRemoveValueResponse>(
                request.Body,
                uri: BuildUri(request.ApiVersion, $"merchants/{request.MerchantId}/gift_card_value_removals"),
                accessTokenHeader: FormatAccessTokenString(unspecifiedUserAccessToken: request.AccessToken),
                actions: null);
        }

        /// <summary>
        /// Reverses specific GiftCard addition  transaction. 
        /// </summary>
        /// <param name="accessToken">Access token for the location</param>
        /// <param name="merchantId">The merchant Id</param>
        /// <param name="giftCardQrData">The qr code of the target card or account</param>
        /// <param name="giftCardTransactionUuid">GiftCard Add Value Transaction UUID to reverse.<seealso cref="GiftCardAddValueResponse"/></param>
        public GiftCardRemoveValueResponse GiftCardDestroyValue(string accessToken, int merchantId, string giftCardQrData, Guid giftCardTransactionUuid)
        {
            return GiftCardDestroyValue(
                accessToken, 
                merchantId,
                new GiftCardRemoveValueRequestBody(giftCardQrData, giftCardTransactionUuid));
        }

        /// <summary>
        /// Destroys Gift Card value by certain amount.
        /// </summary>
        /// <param name="accessToken">Access token for the location</param>
        /// <param name="merchantId">The merchant Id</param>
        /// <param name="giftCardQrData">The qr code of the target card or account</param>
        /// <param name="valueToRemoveInCents">The amount of value to destroy in US Cents</param>
        public GiftCardRemoveValueResponse GiftCardDestroyValue(string accessToken, int merchantId, string giftCardQrData, int valueToRemoveInCents)
        {
            return GiftCardDestroyValue(
                accessToken, 
                merchantId,
                new GiftCardRemoveValueRequestBody(giftCardQrData, valueToRemoveInCents));
        }

        #endregion

        #region IManageRemoteCheckData Implementation

        public UpdateRemoteCheckDataResponse CreateRemoteCheckData(string accessToken, RemoteCheckDataRequestBody createRequest)
        {
            CreateRemoteCheckDataRequest request = new CreateRemoteCheckDataRequest(accessToken, createRequest.LocationId, 
                createRequest.SpendAmountCents, createRequest.TaxAmountCents, createRequest.ExemptionAmountCents, 
                createRequest.IdentifierFromMerchant, createRequest.Register, createRequest.Cashier, 
                createRequest.PartialAuthorizationAllowed, createRequest.Items);


            return _restWrapper.Put<RemoteCheckDataRequestBody, UpdateRemoteCheckDataResponse>(
                request.Body,
                uri: BuildUri(request.ApiVersion, "checks"),
                accessTokenHeader: FormatAccessTokenString(unspecifiedUserAccessToken: request.AccessToken),
                actions: null);
        }

        public FinalizeRemoteCheckResponse FinalizeRemoteCheck(string accessToken, string checkUuid, FinalizeRemoteCheckRequestBody finalizeRequest)
        {
            FinalizeRemoteCheckRequest request = new FinalizeRemoteCheckRequest(accessToken, checkUuid, finalizeRequest.SpendAmountCents, 
                finalizeRequest.TaxAmountCents, finalizeRequest.AppliedDiscountAmountCents);

            return _restWrapper.Post<FinalizeRemoteCheckRequestBody, FinalizeRemoteCheckResponse>(
                request.Body,
                uri: BuildUri(request.ApiVersion, $"checks/{request.CheckUuid}/orders"),
                accessTokenHeader: FormatAccessTokenString(unspecifiedUserAccessToken: request.AccessToken),
                actions: null);
        }

        public GetRemoteCheckDataResponse GetRemoteCheckData(string accessToken, string checkUuid)
        {
            GetRemoteCheckDataRequest request = new GetRemoteCheckDataRequest(accessToken, checkUuid);

            return _restWrapper.Get<GetRemoteCheckDataResponse>(
                uri: BuildUri(request.ApiVersion, $"checks/{request.CheckUuid}"),
                accessTokenHeader: FormatAccessTokenString(unspecifiedUserAccessToken: request.AccessToken),
                actions: null);
        }

        public UpdateRemoteCheckDataResponse UpdateRemoteCheckData(string accessToken, string checkUuid, RemoteCheckDataRequestBody checkDataRequest)
        {
            UpdateRemoteCheckDataRequest request = new UpdateRemoteCheckDataRequest(accessToken, checkUuid, checkDataRequest.LocationId, 
                checkDataRequest.SpendAmountCents, checkDataRequest.TaxAmountCents, checkDataRequest.ExemptionAmountCents, 
                checkDataRequest.IdentifierFromMerchant, checkDataRequest.Register, checkDataRequest.Cashier, 
                checkDataRequest.PartialAuthorizationAllowed, checkDataRequest.Items);

            var uri = BuildUri(request.ApiVersion,
                $"checks{(!string.IsNullOrEmpty(request.CheckUuid) ? "/" + request.CheckUuid : string.Empty)}");

            return _restWrapper.Put<RemoteCheckDataRequestBody, UpdateRemoteCheckDataResponse>(
                request.Body,
                uri: uri,
                accessTokenHeader: FormatAccessTokenString(unspecifiedUserAccessToken: request.AccessToken),
                actions: null);
        }

        #endregion

        #region IRetrieveMerchantFundedGiftCardCredit Implementation
        
        public GiftCardQueryResponse GetMerchantFundedGiftCardCredit(string accessToken, int locationId, string qrData)
        {
            GiftCardCreditQueryRequest request = new GiftCardCreditQueryRequest(accessToken, locationId, qrData);

            return _restWrapper.Post<GiftCardCreditQueryRequestBody, GiftCardQueryResponse>(
                request.Body,
                uri: BuildUri(request.ApiVersion, $"locations/{request.LocationId}/get_merchant_funded_gift_card_credit"),
                accessTokenHeader: FormatAccessTokenString(merchantUserAccessToken: request.AccessToken),
                actions: null);
        }

        #endregion
        
        #region IManageProposedOrders Implementation
        
        public ProposedOrderResponse CreateProposedOrder(string accessToken, int locationId, string qrPaymentData, 
                                                        int totalOutstandingAmountCents, int spendAmountCents, 
                                                        int? taxAmountCents, int exemptionAmountCents, string register, 
                                                        string cashier, string identifierFromMerchant,
                                                        string receiptMessageHtml, bool partialAuthorizationAllowed, 
                                                        bool discountOnly, IList<Item> items)
        {
            // Adjust spend/tax/exemption amounts in situations where there are multiple payments on a single check.
            var adjustmentsForPartialPayments = ProposedOrderCalculator.CalculateCreateProposedOrderValues(
                totalOutstandingAmountCents, taxAmountCents ?? 0, exemptionAmountCents, spendAmountCents);

            CreateProposedOrderRequest request = new CreateProposedOrderRequest(accessToken, locationId, qrPaymentData, 
                                                                                adjustmentsForPartialPayments.SpendAmount, 
                                                                                adjustmentsForPartialPayments.TaxAmount, 
                                                                                adjustmentsForPartialPayments.ExemptionAmount, 
                                                                                register, cashier, identifierFromMerchant, 
                                                                                receiptMessageHtml, partialAuthorizationAllowed,
                                                                                discountOnly, items);

            return _restWrapper.Post<CreateProposedOrderRequestBody, ProposedOrderResponse>(
                request.Body,
                uri: BuildUri(request.ApiVersion, "proposed_orders"),
                accessTokenHeader: FormatAccessTokenString(merchantUserAccessToken: request.AccessToken),
                actions: null);
        }

        public CompletedOrderResponse CompleteProposedOrder(string accessToken, int locationId, string qrPaymentData,
                                                            string proposedOrderUuid, int totalOutstandingAmountCents, 
                                                            int spendAmountCents, int? taxAmountCents, int exemptionAmountCents,
                                                            int? appliedDiscountAmountCents, string register, 
                                                            string cashier, string identifierFromMerchant,
                                                            string receiptMessageHtml, bool partialAuthorizationAllowed, 
                                                            bool discountOnly, IList<Item> items)
        {
            // Adjust spend/tax/exemption amounts in situations where there are multiple payments on a single check.
            var adjustmentsForPartialPayments = ProposedOrderCalculator.CalculateCompleteOrderValues(totalOutstandingAmountCents, 
                taxAmountCents ?? 0, exemptionAmountCents, spendAmountCents, appliedDiscountAmountCents ?? 0);

            CompleteProposedOrderRequest request = new CompleteProposedOrderRequest(accessToken, locationId, qrPaymentData, 
                                                                                    proposedOrderUuid, 
                                                                                    adjustmentsForPartialPayments.SpendAmount, 
                                                                                    adjustmentsForPartialPayments.TaxAmount, 
                                                                                    adjustmentsForPartialPayments.ExemptionAmount,
                                                                                    appliedDiscountAmountCents, register,
                                                                                    cashier, identifierFromMerchant, 
                                                                                    receiptMessageHtml, partialAuthorizationAllowed,
                                                                                    discountOnly, items);

            return _restWrapper.Post<CompleteProposedOrderRequestBody, CompletedOrderResponse>(
                request.Body,
                uri: BuildUri(request.ApiVersion, "completed_orders"),
                accessTokenHeader: FormatAccessTokenString(merchantUserAccessToken: request.AccessToken),
                actions: null);
        }

        #endregion

        #region ICreateMerchantFundedCredit Implementation

        public void GrantMerchantFundedCredit(
            string accessToken,
            string email,
            int durationInSeconds,
            int merchantId,
            string message,
            int valueAmount,
            bool global)
        {
            GrantMerchantFundedCreditRequest request = new GrantMerchantFundedCreditRequest(accessToken, email, durationInSeconds, merchantId, message, valueAmount, global);

            _restWrapper.Post(
                request.Body,
                uri: BuildUri(request.ApiVersion, "merchant_funded_credits"),
                accessTokenHeader: FormatAccessTokenString(merchantUserAccessToken: request.AccessToken),
                actions: null);
        }

        #endregion

        private string BuildUri(LevelUpApiVersion version, string path)
        {
            LevelUpUriBuilder builder = new LevelUpUriBuilder(_targetEnviornment);
            return builder.SetApiVersion(version).SetPath(path).Build();
        }

        private static string FormatAccessTokenString(string merchantUserAccessToken = null,
            string consumerUserAccessToken = null,
            string unspecifiedUserAccessToken = null)
        {
            if (null != unspecifiedUserAccessToken && (null != merchantUserAccessToken || null != consumerUserAccessToken))
            {
                throw new InvalidOperationException("It is invalid to specify both an untagged access token and " +
                                                    "an explicitly tagged merchant/user access token.");
            }

            string levelUpAccessToken = FormatInnerTokenString(merchantUserAccessToken, consumerUserAccessToken, unspecifiedUserAccessToken);
            if (!string.IsNullOrEmpty(levelUpAccessToken))
            {
                return $"token {levelUpAccessToken}";
            }
            return string.Empty;
        }

        private static string FormatInnerTokenString(string merchantUserAccessToken = null,
            string consumerUserAccessToken = null,
            string unspecifiedUserAccessToken = null)
        {
            List<string> tokenStrings = new List<string>();

            if (!string.IsNullOrEmpty(merchantUserAccessToken))
            {
                tokenStrings.Add($"merchant=\"{merchantUserAccessToken}\"");
            }

            if (!string.IsNullOrEmpty(consumerUserAccessToken))
            {
                tokenStrings.Add($"user=\"{consumerUserAccessToken}\"");
            }

            if (!string.IsNullOrEmpty(unspecifiedUserAccessToken))
            {
                tokenStrings.Add(unspecifiedUserAccessToken);
            }

            return string.Join(",", tokenStrings.ToArray());
        }
    }
}
