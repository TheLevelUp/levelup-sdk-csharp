#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="RequestHeaderTokenCreator.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using LevelUp.Api.Utilities;

namespace LevelUp.Api.Client.RequestVisitors
{
    /// <summary>
    /// A visitor for IRequest objects that knows, for each type of object, how
    /// to create the authorization token string that may accompany the request 
    /// header.
    /// </summary>
    /// <remarks>
    /// Some endpoints require that the authorization header explicitly specify 
    /// what type of user token is being provided, i.e. user="token_string" or 
    /// merchant="token_string".  Others (and historically this was the way it 
    /// worked) don't require said specification, i.e. the SDK would simply include
    /// the "token_string" with no tag.  In terms of which scheme to use for 
    /// each type of request, we go by what is in the platform API docs.
    /// </remarks>
    public sealed class RequestHeaderTokenCreator : RequestVisitorWithDefault<string>
    {
        private static readonly Utilities.Func<Request, string> DEFAULT_FUNCTION = (request) => FormatAccessTokenString(unspecifiedUserAccessToken: request.AccessToken);
        
        public RequestHeaderTokenCreator()
            : base(DEFAULT_FUNCTION)
        { }

        #region IRequestVisitor Visit Override Methods
        public override string Visit(AccessTokenRequest request)
        {
            return string.Empty;
        }

        public override string Visit(RefundRequest request)
        {
            if (request.ApiVersion == Http.LevelUpApiVersion.v14)
            {
                return FormatAccessTokenString(unspecifiedUserAccessToken: request.AccessToken);
            }
            return FormatAccessTokenString(merchantUserAccessToken: request.AccessToken);
        }

        public override string Visit(UserLoyaltyQueryRequest request)
        {
            return FormatAccessTokenString(consumerUserAccessToken: request.AccessToken);
        }

        public override string Visit(PasswordResetRequest request)
        {
            return string.Empty;
        }

        public override string Visit(OrderRequest request)
        {
            if (request.ApiVersion == Http.LevelUpApiVersion.v14)
            {
                return FormatAccessTokenString(unspecifiedUserAccessToken: request.AccessToken);
            }
            return FormatAccessTokenString(merchantUserAccessToken: request.AccessToken);
        }

        public override string Visit(GiftCardCreditQueryRequest request)
        {
            return FormatAccessTokenString(merchantUserAccessToken: request.AccessToken);
        }
        
        public override string Visit(CreateProposedOrderRequest request)
        {
            return FormatAccessTokenString(merchantUserAccessToken: request.AccessToken);
        }

        public override string Visit(CompleteProposedOrderRequest request)
        {
            return FormatAccessTokenString(merchantUserAccessToken: request.AccessToken);
        }

        public override string Visit(ManagedLocationQueryRequest request)
        {
            return FormatAccessTokenString(merchantUserAccessToken: request.AccessToken);
        }

        #endregion

        #region Helpers
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
                return string.Format("token {0}", levelUpAccessToken);
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
                tokenStrings.Add(string.Format("merchant=\"{0}\"", merchantUserAccessToken));
            }

            if (!string.IsNullOrEmpty(consumerUserAccessToken))
            {
                tokenStrings.Add(string.Format("user=\"{0}\"", consumerUserAccessToken));
            }

            if (!string.IsNullOrEmpty(unspecifiedUserAccessToken))
            {
                tokenStrings.Add(unspecifiedUserAccessToken);
            }

            return string.Join(",", tokenStrings.ToArray());
        }

        #endregion
    }
}
