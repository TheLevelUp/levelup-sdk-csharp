#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="GiftCardRemoveValueRequest.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using LevelUp.Api.Http;

namespace LevelUp.Api.Client.Models.Requests
{
    public class GiftCardRemoveValueRequest : Request
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
        public GiftCardRemoveValueRequestBody Body { get; }

        /// <summary>
        /// Creates a add value request for a LevelUp gift card
        /// </summary>
        /// <param name="accessToken">App access token for the request</param>
        /// <param name="merchantId">The merchant id</param>
        /// <param name="giftCardQrData">The qr code of the target card or account</param>
        /// <param name="amountInCents">The amount of value to add in US Cents</param>
        public GiftCardRemoveValueRequest(string accessToken, int merchantId, string giftCardQrData, int amountInCents)
            : base(accessToken)
        {
            _merchantId = merchantId;
            Body = new GiftCardRemoveValueRequestBody(giftCardQrData, amountInCents);
        }
    }
}
