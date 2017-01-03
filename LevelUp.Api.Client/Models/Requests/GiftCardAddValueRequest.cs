#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="GiftCardAddValueRequest.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using LevelUp.Api.Client.Models.RequestVisitors;
using LevelUp.Api.Http;

namespace LevelUp.Api.Client.Models.Requests
{
    public class GiftCardAddValueRequest : Request
    {
        protected override LevelUpApiVersion _applicableAPIVersionsBitmask
        {
            get { return LevelUpApiVersion.v15; }
        }

        public int MerchantId { get { return _merchantId; } }
        private readonly int _merchantId;

        /// <summary>
        /// A Serializable http body for the FinalizeRemoteCheckRequest
        /// </summary>
        public GiftCardAddValueRequestBody Body { get { return _body; } }
        private readonly GiftCardAddValueRequestBody _body;

        /// <summary>
        /// Creates a add value request for a LevelUp gift card
        /// </summary>
        /// <param name="accessToken">App access token for the request</param>
        /// <param name="merchantId">The merchant id</param>
        /// <param name="giftCardQrData">The qr code of the target card or account</param>
        /// <param name="amountInCents">The amount of value to add in US Cents</param>
        /// <param name="locationId">The location id of the store where the value add originates</param>
        /// <param name="identifierFromMerchant">The check identifier for the check on which the gift
        /// card is purchased</param>
        /// <param name="tenderTypes">A collection of tender types used to pay for the check on which
        /// the gift card is purchased</param>
        /// <param name="levelUpOrderId">If applicable, a LevelUp order Id associated with the purchase of
        /// the gift card for which value is added</param>
        public GiftCardAddValueRequest( string accessToken,
                                        int merchantId,
                                        string giftCardQrData,
                                        int amountInCents,
                                        int locationId,
                                        string identifierFromMerchant = null,
                                        IList<string> tenderTypes = null,
                                        string levelUpOrderId = null)
            : base(accessToken)
        {
            _merchantId = merchantId;
            _body = new GiftCardAddValueRequestBody(giftCardQrData, amountInCents, locationId, identifierFromMerchant,
                tenderTypes, levelUpOrderId);
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
