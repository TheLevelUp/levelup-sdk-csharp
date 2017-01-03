#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="RequestVisitorWithDefault.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using LevelUp.Api.Utilities;

namespace LevelUp.Api.Client.RequestVisitors
{
    /// <summary>
    /// A RequestVisitorClass, provided for convienence, in which a derived class can 
    /// specify a default function to apply for every Visit method.  This might be useful 
    /// in situations where a client wants to operate on request objects using a visitor, 
    /// but some large portion of the visit methods happen to be implemented in the same way.
    /// </summary>
    public abstract class RequestVisitorWithDefault<T> : IRequestVisitor<T>
    {
        private readonly Func<Request, T> DEFAULT_FUNC;

        protected RequestVisitorWithDefault(Func<Request, T> defaultFunc)
        {
            DEFAULT_FUNC = defaultFunc;
        }

        public virtual T Visit(AccessTokenRequest request)
        {
            return DEFAULT_FUNC(request);
        }

        public virtual T Visit(CreateUserRequest request)
        {
            return DEFAULT_FUNC(request);
        }

        public virtual T Visit(CreateCreditCardRequest request)
        {
            return DEFAULT_FUNC(request);
        }

        public virtual T Visit(DetachedRefundRequest request)
        {
            return DEFAULT_FUNC(request);
        }

        public virtual T Visit(FinalizeRemoteCheckRequest request)
        {
            return DEFAULT_FUNC(request);
        }

        public virtual T Visit(GiftCardAddValueRequest request)
        {
            return DEFAULT_FUNC(request);
        }

        public virtual T Visit(GiftCardRemoveValueRequest request)
        {
            return DEFAULT_FUNC(request);
        }

        public virtual T Visit(MerchantCreditQueryRequest request)
        {
            return DEFAULT_FUNC(request);
        }

        public virtual T Visit(OrderRequest request)
        {
            return DEFAULT_FUNC(request);
        }

        public virtual T Visit(RefundRequest request)
        {
            return DEFAULT_FUNC(request);
        }

        public virtual T Visit(UpdateRemoteCheckDataRequest request)
        {
            return DEFAULT_FUNC(request);
        }

        public virtual T Visit(UpdateUserRequest request)
        {
            return DEFAULT_FUNC(request);
        }

        public virtual T Visit(CreateRemoteCheckDataRequest request)
        {
            return DEFAULT_FUNC(request);
        }

        public virtual T Visit(DeleteCreditCardRequest request)
        {
            return DEFAULT_FUNC(request);
        }

        public virtual T Visit(LocationDetailsQueryRequest request)
        {
            return DEFAULT_FUNC(request);
        }

        public virtual T Visit(UserLoyaltyQueryRequest request)
        {
            return DEFAULT_FUNC(request);
        }

        public virtual T Visit(MerchantOrderDetailsRequest request)
        {
            return DEFAULT_FUNC(request);
        }

        public virtual T Visit(PaymentTokenQueryRequest request)
        {
            return DEFAULT_FUNC(request);
        }

        public virtual T Visit(GetRemoteCheckDataRequest request)
        {
            return DEFAULT_FUNC(request);
        }

        public virtual T Visit(UserDetailsQueryRequest request)
        {
            return DEFAULT_FUNC(request);
        }

        public virtual T Visit(CreditCardQueryRequest request)
        {
            return DEFAULT_FUNC(request);
        }

        public virtual T Visit(LocationQueryRequest request)
        {
            return DEFAULT_FUNC(request);
        }

        public virtual T Visit(OrderQueryRequest request)
        {
            return DEFAULT_FUNC(request);
        }

        public virtual T Visit(UserAddressesQueryRequest request)
        {
            return DEFAULT_FUNC(request);
        }

        public virtual T Visit(PasswordResetRequest request)
        {
            return DEFAULT_FUNC(request);
        }

        public virtual T Visit(CreateProposedOrderRequest request)
        {
            return DEFAULT_FUNC(request);
        }

        public virtual T Visit(CompleteProposedOrderRequest request)
        {
            return DEFAULT_FUNC(request);
        }

        public virtual T Visit(GiftCardCreditQueryRequest request)
        {
            return DEFAULT_FUNC(request);
        }

        public virtual T Visit(ManagedLocationQueryRequest request)
        {
            return DEFAULT_FUNC(request);
        }
    }
}
