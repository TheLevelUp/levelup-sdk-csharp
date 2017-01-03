#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="IRequestVisitor.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

namespace LevelUp.Api.Client.Models.RequestVisitors
{
    /// <summary>
    /// An interface to enable the visitor pattern tied to the various request objects.
    /// </summary>
    public interface IRequestVisitor<T>
    {
        T Visit(AccessTokenRequest request);
        T Visit(CreateUserRequest request);
        T Visit(CreateCreditCardRequest request);
        T Visit(DetachedRefundRequest request);
        T Visit(FinalizeRemoteCheckRequest request);
        T Visit(GiftCardAddValueRequest request);
        T Visit(GiftCardRemoveValueRequest request);
        T Visit(MerchantCreditQueryRequest request);
        T Visit(OrderRequest request);
        T Visit(RefundRequest request);
        T Visit(UpdateRemoteCheckDataRequest request);
        T Visit(UpdateUserRequest request);
        T Visit(CreateRemoteCheckDataRequest request);
        T Visit(DeleteCreditCardRequest request);
        T Visit(LocationDetailsQueryRequest request);
        T Visit(UserLoyaltyQueryRequest request);
        T Visit(MerchantOrderDetailsRequest request);
        T Visit(PaymentTokenQueryRequest request);
        T Visit(GetRemoteCheckDataRequest request);
        T Visit(UserDetailsQueryRequest request);
        T Visit(CreditCardQueryRequest request);
        T Visit(LocationQueryRequest request);
        T Visit(OrderQueryRequest request);
        T Visit(UserAddressesQueryRequest request);
        T Visit(PasswordResetRequest request);
        T Visit(CreateProposedOrderRequest request);
        T Visit(CompleteProposedOrderRequest request);
        T Visit(GiftCardCreditQueryRequest request);
        T Visit(ManagedLocationQueryRequest request);
    }
}
