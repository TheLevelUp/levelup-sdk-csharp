#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="RequestExecutionEngine.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using System.Net;
using LevelUp.Api.Client.Models.Requests;
using LevelUp.Api.Client.Models.RequestVisitors;
using LevelUp.Api.Client.Models.Responses;
using LevelUp.Api.Http;

namespace LevelUp.Api.Client.RequestVisitors
{
    /// <summary>
    /// A visitor for IRequest objects that knows, for each type of object, how
    /// to generate a request to the LevelUp platform.  The job of this class is
    /// to know what type of rest request (GET/PUT/etc.) is associated with each
    /// IRequest object, and to actually make the call.  This is the default
    /// visitor that gets used in the implementation of the LevelUp client interfaces
    /// to execute requests.
    /// </summary>
    public sealed class RequestExecutionEngine : IRequestVisitor<IResponse>
    {
        private readonly LevelUpRestWrapper _restWrapper;
        private readonly IRequestVisitor<string> _endpointCreator;
        private readonly IRequestVisitor<string> _headerTokenCreator;
        private readonly IRequestVisitor<Dictionary<HttpStatusCode, LevelUpRestWrapper.ResponseAction>> _customHttpStatusCodeHandlers;


        public RequestExecutionEngine(IRestfulService restService,
            AgentIdentifier identifier, IRequestVisitor<string> endpointCreator, IRequestVisitor<string> headerTokenCreator,
            IRequestVisitor<Dictionary<HttpStatusCode, LevelUpRestWrapper.ResponseAction>> customHttpStatusCodeHandlers)
        {
            _restWrapper = new LevelUpRestWrapper(restService, identifier);
            _endpointCreator = endpointCreator;
            _headerTokenCreator = headerTokenCreator;
            _customHttpStatusCodeHandlers = customHttpStatusCodeHandlers;
        }

        #region IRequestVisitor Visit Methods
        public IResponse Visit(AccessTokenRequest request)
        {
            return _restWrapper.Post<AccessTokenRequestBody, AccessToken>(  request.Body, 
                                                                            uri: GetEndpoint(request),
                                                                            actions: GetCustomResponseHandlers(request));
        }

        public IResponse Visit(CreateUserRequest request)
        {
            return _restWrapper.Post<CreateUserRequestBody, User>(  request.Body,
                                                                    uri: GetEndpoint(request),
                                                                    accessTokenHeader: GetHeaderToken(request),
                                                                    actions: GetCustomResponseHandlers(request));
        }

        public IResponse Visit(CreateCreditCardRequest request)
        {
            return _restWrapper.Post<CreateCreditCardRequestBody, CreditCard>(  request.Body, 
                                                                                uri: GetEndpoint(request),
                                                                                accessTokenHeader: GetHeaderToken(request),
                                                                                actions: GetCustomResponseHandlers(request));
        }

        public IResponse Visit(DetachedRefundRequest request)
        {
            return _restWrapper.Post<DetachedRefundRequestBody, DetachedRefundResponse>(request.Body,
                                                                                        uri: GetEndpoint(request),
                                                                                        accessTokenHeader: GetHeaderToken(request),
                                                                                        actions: GetCustomResponseHandlers(request));
        }

        public IResponse Visit(FinalizeRemoteCheckRequest request)
        {
            return _restWrapper.Post<FinalizeRemoteCheckRequestBody, FinalizeRemoteCheckResponse>(  request.Body,
                                                                                                    uri: GetEndpoint(request),
                                                                                                    accessTokenHeader: GetHeaderToken(request),
                                                                                                    actions: GetCustomResponseHandlers(request));
        }

        public IResponse Visit(GiftCardAddValueRequest request)
        {
            return _restWrapper.Post<GiftCardAddValueRequestBody, GiftCardAddValueResponse>(request.Body,
                                                                                            uri: GetEndpoint(request),
                                                                                            accessTokenHeader: GetHeaderToken(request),
                                                                                            actions: GetCustomResponseHandlers(request));
        }

        public IResponse Visit(GiftCardRemoveValueRequest request)
        {
            return _restWrapper.Post<GiftCardRemoveValueRequestBody, GiftCardRemoveValueResponse>(  request.Body,
                                                                                                    uri: GetEndpoint(request),
                                                                                                    accessTokenHeader: GetHeaderToken(request),
                                                                                                    actions: GetCustomResponseHandlers(request));
        }

        public IResponse Visit(RefundRequest request)
        {
            return _restWrapper.Post<RefundRequestBody, RefundResponse>(request.Body, 
                                                                        uri: GetEndpoint(request),
                                                                        accessTokenHeader: GetHeaderToken(request),
                                                                        actions: GetCustomResponseHandlers(request));
        }

        public IResponse Visit(UpdateRemoteCheckDataRequest request)
        {
            return _restWrapper.Put<RemoteCheckDataRequestBody, UpdateRemoteCheckDataResponse>( request.Body,
                                                                                                uri: GetEndpoint(request),
                                                                                                accessTokenHeader: GetHeaderToken(request),
                                                                                                actions: GetCustomResponseHandlers(request));
        }

        public IResponse Visit(UpdateUserRequest request)
        {
            return _restWrapper.Put<UpdateUserRequestBody, User>(   request.Body, 
                                                                    uri: GetEndpoint(request),
                                                                    accessTokenHeader: GetHeaderToken(request),
                                                                    actions: GetCustomResponseHandlers(request));
        }

        public IResponse Visit(CreateRemoteCheckDataRequest request)
        {
            return _restWrapper.Post<RemoteCheckDataRequestBody, UpdateRemoteCheckDataResponse>(request.Body,
                                                                                                uri: GetEndpoint(request),
                                                                                                accessTokenHeader: GetHeaderToken(request),
                                                                                                actions: GetCustomResponseHandlers(request));
        }

        public IResponse Visit(DeleteCreditCardRequest request)
        {
            _restWrapper.Delete(uri: GetEndpoint(request),
                                accessTokenHeader: GetHeaderToken(request),
                                actions: GetCustomResponseHandlers(request));
            return new EmptyResponse();
        }

        public IResponse Visit(LocationDetailsQueryRequest request)
        {
            return _restWrapper.Get<LocationDetails>(uri: GetEndpoint(request),
                                                    accessTokenHeader: GetHeaderToken(request),
                                                    actions: GetCustomResponseHandlers(request));
        }

        public IResponse Visit(UserLoyaltyQueryRequest request)
        {
            return _restWrapper.Get<Loyalty>(uri: GetEndpoint(request),
                                            accessTokenHeader: GetHeaderToken(request),
                                            actions: GetCustomResponseHandlers(request));
        }

        public IResponse Visit(MerchantOrderDetailsRequest request)
        {
            return _restWrapper.Get<OrderDetailsResponse>(  uri: GetEndpoint(request),
                                                            accessTokenHeader: GetHeaderToken(request),
                                                            actions: GetCustomResponseHandlers(request));
        }

        public IResponse Visit(PaymentTokenQueryRequest request)
        {
            return _restWrapper.Get<PaymentToken>(  uri: GetEndpoint(request),
                                                    accessTokenHeader: GetHeaderToken(request),
                                                    actions: GetCustomResponseHandlers(request));
        }

        public IResponse Visit(GetRemoteCheckDataRequest request)
        {
            return _restWrapper.Get<GetRemoteCheckDataResponse>(uri: GetEndpoint(request),
                                                                accessTokenHeader: GetHeaderToken(request),
                                                                actions: GetCustomResponseHandlers(request));
        }

        public IResponse Visit(UserDetailsQueryRequest request)
        {
            return _restWrapper.Get<User>(  uri: GetEndpoint(request),
                                            accessTokenHeader: GetHeaderToken(request),
                                            actions: GetCustomResponseHandlers(request));
        }

        public IResponse Visit(LocationQueryRequest request)
        {
            List<Location> response = _restWrapper.Get<List<Location>>( uri: GetEndpoint(request),
                                                                        accessTokenHeader: GetHeaderToken(request),
                                                                        actions: GetCustomResponseHandlers(request));
            return new LocationQueryResponse(response);
        }

        public IResponse Visit(CreditCardQueryRequest request)
        {
            List<CreditCard> response = _restWrapper.Get<List<CreditCard>>( uri: GetEndpoint(request),
                                                                            accessTokenHeader: GetHeaderToken(request),
                                                                            actions: GetCustomResponseHandlers(request));

            return new CreditCardQueryResponse(response);
        }

        public IResponse Visit(OrderQueryRequest request)
        {
            PagedList<OrderDetailsResponse> pagedResponse = _restWrapper.GetWithPaging<OrderDetailsResponse>(GetEndpoint(request),
                accessTokenHeader: GetHeaderToken(request), currentPageNumber: request.PageNumber);
            return new OrderQueryResponse(pagedResponse);
        }

        public IResponse Visit(UserAddressesQueryRequest request)
        {
            List<UserAddress> addresses = _restWrapper.Get<List<UserAddress>>(  uri: GetEndpoint(request),
                                                                                accessTokenHeader: GetHeaderToken(request),
                                                                                actions: GetCustomResponseHandlers(request));
            return new UserAddressQueryResponse(addresses);
        }

        public IResponse Visit(PasswordResetRequest request)
        {
            _restWrapper.Post<PasswordResetRequestBody>(request.Body, uri: GetEndpoint(request), actions: GetCustomResponseHandlers(request));
            return new EmptyResponse();
        }

        public IResponse Visit(CreateProposedOrderRequest request)
        {
            return _restWrapper.Post<CreateProposedOrderRequestBody, ProposedOrderResponse>(request.Body,
                                                            uri: GetEndpoint(request),
                                                            accessTokenHeader: GetHeaderToken(request),
                                                            actions: GetCustomResponseHandlers(request));
        }

        public IResponse Visit(CompleteProposedOrderRequest request)
        {
            return _restWrapper.Post<CompleteProposedOrderRequestBody, CompletedOrderResponse>(request.Body,
                                                            uri: GetEndpoint(request),
                                                            accessTokenHeader: GetHeaderToken(request),
                                                            actions: GetCustomResponseHandlers(request));
        }

        public IResponse Visit(GiftCardCreditQueryRequest request)
        {
            return _restWrapper.Post<GiftCardCreditQueryRequestBody, GiftCardQueryResponse>(request.Body,
                                                            uri: GetEndpoint(request),
                                                            accessTokenHeader: GetHeaderToken(request),
                                                            actions: GetCustomResponseHandlers(request));
        }

        public IResponse Visit(ManagedLocationQueryRequest request)
        {
            List<ManagedLocation> response = _restWrapper.Get<List<ManagedLocation>> (
                                                                        uri: GetEndpoint(request),
                                                                        accessTokenHeader: GetHeaderToken(request),
                                                                        actions: GetCustomResponseHandlers(request));
            return new ManagedLocationQueryResponse(response);
        }

        #endregion

        private string GetEndpoint(Request request)
        {
            return request.Accept(_endpointCreator);
        }

        private string GetHeaderToken(Request request)
        {
            return request.Accept(_headerTokenCreator);
        }

        private Dictionary<HttpStatusCode, LevelUpRestWrapper.ResponseAction> GetCustomResponseHandlers(Request request)
        {
            return request.Accept(_customHttpStatusCodeHandlers);
        }

    }
}
