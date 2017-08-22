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
using LevelUp.Api.Client.Models.Requests;
using LevelUp.Api.Client.Models.RequestVisitors;
using LevelUp.Api.Client.Models.Responses;
using LevelUp.Pos.ProposedOrders;

namespace LevelUp.Api.Client
{
    /// <summary>
    /// A unified implementation for all of the various client interfaces.  
    /// </summary>
    /// <remarks>
    /// The meat-and-potatoes of making calls to the LevelUp platform 
    /// resides in the RequestExecutionEngine visitor that gets passed
    /// into this class in the constructor.  The role of this class is
    /// really only to create/populate IRequest objects using the data
    /// provided to these SDK methods and then provide those objects
    /// with the execution engine visitor.
    /// </remarks>
    internal class LevelUpClient : ILevelUpClientSuperset
    {
        private readonly IRequestVisitor<IResponse> _engine;

        internal LevelUpClient(IRequestVisitor<IResponse> requestExecutionEngine)
        {
            if (requestExecutionEngine == null)
            {
                throw new ArgumentNullException("The LevelUpClient constructor requires a "+ 
                    "non-null IRequestVisitor<Response>");
            }

            _engine = requestExecutionEngine;
        }

        #region IAuthenticate Implementation
        
        public AccessToken Authenticate(string apiKey, string username, string password)
        {
            AccessTokenRequest request = new AccessTokenRequest(apiKey, username, password);
            return request.Accept(_engine) as AccessToken;
        }

        #endregion

        #region ICreateCreditCards Implementation

        public CreditCard CreateCreditCard(string accessToken, string encryptedNumber, string encryptedExpirationMonth,
            string encryptedExpirationYear, string encryptedCvv, string postalCode)
        {
            CreateCreditCardRequest request = new CreateCreditCardRequest(accessToken, encryptedNumber,
                encryptedExpirationMonth, encryptedExpirationYear, encryptedCvv, postalCode);
            return request.Accept(_engine) as CreditCard;
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
            return request.Accept(_engine) as DetachedRefundResponse;
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
            return request.Accept(_engine) as RefundResponse;
        }

        #endregion

        #region IDestroyCreditCards Implementation

        public void DeleteCreditCard(string accessToken, int creditCardId)
        {
            DeleteCreditCardRequest request = new DeleteCreditCardRequest(accessToken, creditCardId);
            request.Accept(_engine);
        }

        #endregion

        #region ILookupUserLoyalty Implementation

        public Loyalty GetLoyalty(string accessToken, int merchantId)
        {
            UserLoyaltyQueryRequest request = new UserLoyaltyQueryRequest(accessToken, merchantId);
            return request.Accept(_engine) as Loyalty;
        }

        #endregion

        #region IModifyUser Implementation

        public User CreateUser(string apiKey, string firstName, string lastName, string email, string password)
        {
            CreateUserRequest request = new CreateUserRequest(apiKey, firstName, lastName, email, password);
            return request.Accept(_engine) as User;
        }

        public User CreateUser(string apiKey, CreateUserRequestBodyUserSection requestBody)
        {
            return CreateUser(apiKey, requestBody.FirstName, requestBody.LastName, requestBody.Email, requestBody.Password);
        }

        public User UpdateUser(string accessToken, UpdateUserRequestBody requestBody)
        {
            UpdateUserRequest request = new UpdateUserRequest(accessToken, requestBody);
            return request.Accept(_engine) as User;
        }

        public void PasswordResetRequest(string email)
        {
            PasswordResetRequest request = new PasswordResetRequest(email);
            request.Accept(_engine);
        }

        #endregion

        #region IQueryCreditCards Implementation

        public IList<CreditCard> ListCreditCards(string accessToken)
        {
            CreditCardQueryRequest request = new CreditCardQueryRequest(accessToken);
            CreditCardQueryResponse response = request.Accept(_engine) as CreditCardQueryResponse;
            return response.CreditCards;
        }

        #endregion

        #region IQueryMerchantData Implementation
        
        public LocationDetails GetLocationDetails(string accessToken, int locationId)
        {
            LocationDetailsQueryRequest request = new LocationDetailsQueryRequest(accessToken, locationId);
            return request.Accept(_engine) as LocationDetails;
        }

        public OrderDetailsResponse GetMerchantOrderDetails(string accessToken, int merchantId, string orderIdentifier)
        {
            MerchantOrderDetailsRequest request = new MerchantOrderDetailsRequest(accessToken, merchantId, orderIdentifier);
            return request.Accept(_engine) as OrderDetailsResponse;
        }

        public IList<Location> ListLocations(string accessToken, int merchantId)
        {
            LocationQueryRequest request = new LocationQueryRequest(accessToken, merchantId);
            return (request.Accept(_engine) as LocationQueryResponse).Details;
        }

        public IList<ManagedLocation> ListManagedLocations(string accessToken)
        {
            ManagedLocationQueryRequest request = new ManagedLocationQueryRequest(accessToken);
            return (request.Accept(_engine) as ManagedLocationQueryResponse).Details;
        }

        #endregion

        #region IQueryOrders Implementation

        public IList<OrderDetailsResponse> ListOrders(string accessToken, int locationId, int startPageNum = 1, int endPageNum = 1)
        {
            bool areThereMorePages = false;
            return ListOrders(accessToken, locationId, startPageNum, endPageNum, out areThereMorePages);
        }

        public IList<OrderDetailsResponse> ListOrders(string accessToken, int locationId, int startPageNum, int endPageNum, out bool areThereMorePages)
        {
            if (endPageNum < startPageNum)
            {
                endPageNum = startPageNum;
            }

            OrderQueryRequest request = new OrderQueryRequest(accessToken, locationId, startPageNum);
            PagedList<OrderDetailsResponse> orders = (request.Accept(_engine) as OrderQueryResponse).Orders;

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

        public IList<OrderDetailsResponse> ListFilteredOrders(string accessToken, int locationId, int startPageNum, int endPageNum,
            Func<OrderDetailsResponse, bool> filter = null, Func<OrderDetailsResponse, OrderDetailsResponse, int> @orderby = null)
        {
            var allOrders = new List<OrderDetailsResponse> (ListOrders(accessToken, locationId, startPageNum, endPageNum));

            List<OrderDetailsResponse> filtered = (filter == null) ? allOrders : allOrders.FindAll(x => filter(x));

            if (null != orderby)
            {
                filtered.Sort((x, y) => @orderby(x, y));
            }

            return filtered;
        }

        #endregion

        #region IQueryUser Implementation
        
        public IList<UserAddress> ListUserAddresses(string accessToken)
        {
            UserAddressesQueryRequest request = new UserAddressesQueryRequest(accessToken);
            var response = (request.Accept(_engine) as UserAddressQueryResponse);
            return (response != null) ? response.Addresses : null;
        }

        public User GetUser(string accessToken, int userId)
        {
            UserDetailsQueryRequest request = new UserDetailsQueryRequest(accessToken, userId);
            return request.Accept(_engine) as User;
        }

        #endregion

        #region IRetrievePaymentToken Implementation
        
        public Models.Responses.PaymentToken GetPaymentToken(string accessToken)
        {
            PaymentTokenQueryRequest request = new PaymentTokenQueryRequest(accessToken);
            return request.Accept(_engine) as Models.Responses.PaymentToken;
        }

        #endregion

        #region ICreateGiftCardValue Implementation

        public GiftCardAddValueResponse GiftCardAddValue(string accessToken, int merchantId, GiftCardAddValueRequestBody addValueRequest)
        {
            return GiftCardAddValue(accessToken, merchantId, addValueRequest.LocationId, addValueRequest.GiftCardQrData, addValueRequest.AmountInCents, 
                addValueRequest.IdentifierFromMerchant, addValueRequest.TenderTypes, addValueRequest.AssociatedLevelUpOrderId);
        }

        public GiftCardAddValueResponse GiftCardAddValue(string accessToken, int merchantId, int locationId, string giftCardQrData, int valueToAddInCents)
        {
            return GiftCardAddValue(accessToken, merchantId, locationId, giftCardQrData, valueToAddInCents, string.Empty, null, null);
        }

        public GiftCardAddValueResponse GiftCardAddValue(string accessToken, int merchantId, int locationId, string giftCardQrData, 
            int valueToAddInCents, string identifierFromMerchant, IList<string> tenderTypes = null, string levelUpOrderId = null)
        {
            GiftCardAddValueRequest request = new GiftCardAddValueRequest(accessToken, merchantId, giftCardQrData, valueToAddInCents, locationId, 
                identifierFromMerchant, tenderTypes, levelUpOrderId);
            return request.Accept(_engine) as GiftCardAddValueResponse;
        }

        #endregion

        #region IDestroyGiftCardValue Implementation

        public GiftCardRemoveValueResponse GiftCardDestroyValue(string accessToken, int merchantId, GiftCardRemoveValueRequestBody removeValueRequest)
        {
            return GiftCardDestroyValue(accessToken, merchantId, removeValueRequest.GiftCardQrData, removeValueRequest.AmountInCents);
        }

        public GiftCardRemoveValueResponse GiftCardDestroyValue(string accessToken, int merchantId, string giftCardQrData, int valueToRemoveInCents)
        {
            GiftCardRemoveValueRequest request = new GiftCardRemoveValueRequest(accessToken, merchantId, giftCardQrData, valueToRemoveInCents);
            return request.Accept(_engine) as GiftCardRemoveValueResponse;
        }

        #endregion

        #region IManageRemoteCheckData Implementation

        public UpdateRemoteCheckDataResponse CreateRemoteCheckData(string accessToken, RemoteCheckDataRequestBody createRequest)
        {
            CreateRemoteCheckDataRequest request = new CreateRemoteCheckDataRequest(accessToken, createRequest.LocationId, 
                createRequest.SpendAmountCents, createRequest.TaxAmountCents, createRequest.ExemptionAmountCents, 
                createRequest.IdentifierFromMerchant, createRequest.Register, createRequest.Cashier, 
                createRequest.PartialAuthorizationAllowed, createRequest.Items);
            return request.Accept(_engine) as UpdateRemoteCheckDataResponse;
        }

        public FinalizeRemoteCheckResponse FinalizeRemoteCheck(string accessToken, string checkUuid, FinalizeRemoteCheckRequestBody finalizeRequest)
        {
            FinalizeRemoteCheckRequest request = new FinalizeRemoteCheckRequest(accessToken, checkUuid, finalizeRequest.SpendAmountCents, 
                finalizeRequest.TaxAmountCents, finalizeRequest.AppliedDiscountAmountCents);
            return request.Accept(_engine) as FinalizeRemoteCheckResponse;
        }

        public GetRemoteCheckDataResponse GetRemoteCheckData(string accessToken, string checkUuid)
        {
            GetRemoteCheckDataRequest request = new GetRemoteCheckDataRequest(accessToken, checkUuid);
            return request.Accept(_engine) as GetRemoteCheckDataResponse;
        }

        public UpdateRemoteCheckDataResponse UpdateRemoteCheckData(string accessToken, string checkUuid, RemoteCheckDataRequestBody checkDataRequest)
        {
            UpdateRemoteCheckDataRequest request = new UpdateRemoteCheckDataRequest(accessToken, checkUuid, checkDataRequest.LocationId, 
                checkDataRequest.SpendAmountCents, checkDataRequest.TaxAmountCents, checkDataRequest.ExemptionAmountCents, 
                checkDataRequest.IdentifierFromMerchant, checkDataRequest.Register, checkDataRequest.Cashier, 
                checkDataRequest.PartialAuthorizationAllowed, checkDataRequest.Items);
            return request.Accept(_engine) as UpdateRemoteCheckDataResponse;
        }

        #endregion

        #region IRetrieveMerchantFundedGiftCardCredit Implementation
        
        public GiftCardQueryResponse GetMerchantFundedGiftCardCredit(string accessToken, int locationId, string qrData)
        {
            GiftCardCreditQueryRequest request = new GiftCardCreditQueryRequest(accessToken, locationId, qrData);
            return request.Accept(_engine) as GiftCardQueryResponse;
        }

        #endregion
        
        #region IManageProposedOrders Implementation
        
        public ProposedOrderResponse CreateProposedOrder(string accessToken, int locationId, string qrPaymentData, 
                                                        int totalOutstandingAmountCents, int spendAmountCents, 
                                                        int? taxAmountCents, int exemptionAmountCents, string register, 
                                                        string cashier, string identifierFromMerchant,
                                                        string receiptMessageHtml, bool partialAuthorizationAllowed, 
                                                        IList<Item> items)
        {
            // Adjust spend/tax/exemption amounts in situations where there are multiple payments on a single check.
            var adjustmentsForPartialPayments = ProposedOrderCalculator.CalculateCreateProposedOrderValues(
                totalOutstandingAmountCents, taxAmountCents ?? 0, exemptionAmountCents, spendAmountCents);

            CreateProposedOrderRequest request = new CreateProposedOrderRequest(accessToken, locationId, qrPaymentData, 
                                                                                adjustmentsForPartialPayments.SpendAmount, 
                                                                                adjustmentsForPartialPayments.TaxAmount, 
                                                                                adjustmentsForPartialPayments.ExemptionAmount, 
                                                                                register, cashier, identifierFromMerchant, 
                                                                                receiptMessageHtml, partialAuthorizationAllowed, items);

            return request.Accept(_engine) as ProposedOrderResponse;
        }

        public CompletedOrderResponse CompleteProposedOrder(string accessToken, int locationId, string qrPaymentData,
                                                            string proposedOrderUuid, int totalOutstandingAmountCents, 
                                                            int spendAmountCents, int? taxAmountCents, int exemptionAmountCents,
                                                            int? appliedDiscountAmountCents, string register, 
                                                            string cashier, string identifierFromMerchant,
                                                            string receiptMessageHtml, bool partialAuthorizationAllowed, 
                                                            IList<Item> items)
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
                                                                                    items);

            return request.Accept(_engine) as CompletedOrderResponse;
        }

        #endregion
    }
}
