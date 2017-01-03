#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="MerchantOrderDetailsRequest.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using LevelUp.Api.Client.Models.RequestVisitors;
using LevelUp.Api.Http;

namespace LevelUp.Api.Client.Models.Requests
{
    /// <summary>
    /// Request to get detailed info pertaining to a particular order.
    /// </summary>
    public class MerchantOrderDetailsRequest : Request
    {
        protected override LevelUpApiVersion _applicableAPIVersionsBitmask
        {
            get { return LevelUpApiVersion.v14 | LevelUpApiVersion.v15; }
        }

        public int MerchantId { get { return _merchantId; } }
        private readonly int _merchantId;

        public string OrderIdentifier { get { return _orderIdentifier; } }
        private readonly string _orderIdentifier;

        public MerchantOrderDetailsRequest(string accessToken, int merchantId, string orderIdentifier) 
            : base(accessToken)
        {
            _merchantId = merchantId;
            _orderIdentifier = orderIdentifier;
        }

        /// <summary>
        /// Acceptance method for Request visitors.
        /// </summary>
        public override T Accept<T>(IRequestVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
