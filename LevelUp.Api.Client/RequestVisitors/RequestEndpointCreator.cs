#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="RequestEndpointCreator.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using LevelUp.Api.Client.Models.Requests;
using LevelUp.Api.Client.Models.RequestVisitors;
using LevelUp.Api.Http;

namespace LevelUp.Api.Client.RequestVisitors
{
    /// <summary>
    /// A visitor for IRequest objects that knows, for each type of object, how
    /// to generate the appropriate uri for the LevelUp api.  This is the default
    /// visitor that gets used in the implementation of the LevelUp client interfaces
    /// to generate uri strings.
    /// </summary>
    /// <remarks>
    /// A client could theoretically sub-in a different (or derived) visitor if they
    /// wanted to change the uris that get targeted by this SDK (to target a different
    /// version of the api, for instance) by providing that alternative visitor during 
    /// the creation of the LevelUpClient's request execution engine.
    /// </remarks>
    public class RequestEndpointCreator : IRequestVisitor<string>
    {
        public LevelUpEnvironment TargetEnviornment { get; private set; }

        public RequestEndpointCreator() : this(LevelUpEnvironment.Sandbox) { }

        public RequestEndpointCreator(LevelUpEnvironment targetEnvironment)
        {
            TargetEnviornment = targetEnvironment;
        }

        #region IRequestVisitor Visit Methods
        public string Visit(AccessTokenRequest request)
        {
            return BuildUri(request.ApiVersion, "access_tokens");
        }

        public string Visit(CreateUserRequest request)
        {
            return BuildUri(request.ApiVersion, "users");
        }

        public string Visit(CreateCreditCardRequest request)
        {
            return BuildUri(request.ApiVersion, "credit_cards");
        }

        public string Visit(DetachedRefundRequest request)
        {
            return BuildUri(request.ApiVersion, "detached_refunds");
        }

        public string Visit(GiftCardAddValueRequest request)
        {
            return BuildUri(request.ApiVersion, string.Format("merchants/{0}/gift_card_value_additions", request.MerchantId));
        }

        public string Visit(GiftCardRemoveValueRequest request)
        {
            return BuildUri(request.ApiVersion, string.Format("merchants/{0}/gift_card_value_removals", request.MerchantId));
        }

        public string Visit(FinalizeRemoteCheckRequest request)
        {
            return BuildUri(request.ApiVersion, string.Format("checks/{0}/orders", request.CheckUuid));
        }

        public string Visit(RefundRequest request)
        {
            return BuildUri(request.ApiVersion, string.Format("orders/{0}/refund", request.OrderIdentifier));
        }

        public string Visit(UpdateRemoteCheckDataRequest request)
        {
            return BuildUri(request.ApiVersion, string.Format("checks{0}", !string.IsNullOrEmpty(request.CheckUuid)
                                                              ? "/" + request.CheckUuid
                                                              : string.Empty));
        }

        public string Visit(UpdateUserRequest request)
        {
            return BuildUri(request.ApiVersion, string.Format("users/{0}", request.Body.Id));
        }

        public string Visit(CreateRemoteCheckDataRequest request)
        {
            return BuildUri(request.ApiVersion, "checks");
        }

        public string Visit(DeleteCreditCardRequest request)
        {
            return BuildUri(request.ApiVersion, string.Format("credit_cards/{0}", request.CardId));
        }

        public string Visit(LocationDetailsQueryRequest request)
        {
            return BuildUri(request.ApiVersion, string.Format("locations/{0}", request.LocationId));
        }

        public string Visit(UserLoyaltyQueryRequest request)
        {
            return BuildUri(request.ApiVersion, string.Format("merchants/{0}/loyalty", request.MerchantId));
        }

        public string Visit(MerchantOrderDetailsRequest request)
        {
            return BuildUri(request.ApiVersion, string.Format("merchants/{0}/orders/{1}", request.MerchantId, request.OrderIdentifier));
        }

        public string Visit(PaymentTokenQueryRequest request)
        {
            return BuildUri(request.ApiVersion, "payment_token");
        }

        public string Visit(GetRemoteCheckDataRequest request)
        {
            return BuildUri(request.ApiVersion, string.Format("checks/{0}", request.CheckUuid));
        }

        public string Visit(UserDetailsQueryRequest request)
        {
            return BuildUri(request.ApiVersion, string.Format("users/{0}", request.UserId));
        }

        public string Visit(CreditCardQueryRequest request)
        {
            return BuildUri(request.ApiVersion, "credit_cards");
        }

        public string Visit(LocationQueryRequest request)
        {
            return BuildUri(request.ApiVersion, string.Format("merchants/{0}/locations", request.MerchantId));
        }

        public string Visit(OrderQueryRequest request)
        {
            string path = string.Format("locations/{0}/orders", request.LocationId);
            LevelUpUriBuilder builder = new LevelUpUriBuilder(TargetEnviornment);
            builder.SetApiVersion(request.ApiVersion).SetPath(path);

            if (request.PageNumber > 1)
            {
                builder.AppendQuery("page", request.PageNumber.ToString());
            }

            return builder.Build();
        }

        public string Visit(UserAddressesQueryRequest request)
        {
            return BuildUri(request.ApiVersion, "user_addresses");
        }

        public string Visit(PasswordResetRequest request)
        {
            return BuildUri(request.ApiVersion, "passwords");
        }

        public string Visit(CreateProposedOrderRequest request)
        {
            return BuildUri(request.ApiVersion, "proposed_orders");
        }

        public string Visit(CompleteProposedOrderRequest request)
        {
            return BuildUri(request.ApiVersion, "completed_orders");
        }

        public string Visit(GiftCardCreditQueryRequest request)
        {
            return BuildUri(request.ApiVersion, string.Format(
                "locations/{0}/get_merchant_funded_gift_card_credit", request.LocationId));
        }

        public string Visit(ManagedLocationQueryRequest request)
        {
            return BuildUri(request.ApiVersion, "managed_locations");
        }

        #endregion

        private string BuildUri(LevelUpApiVersion version, string path)
        {
            LevelUpUriBuilder builder = new LevelUpUriBuilder(TargetEnviornment);
            return builder.SetApiVersion(version).SetPath(path).Build();
        }


    }
}
