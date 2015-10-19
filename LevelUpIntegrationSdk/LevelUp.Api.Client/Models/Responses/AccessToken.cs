//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="AccessToken.cs" company="SCVNGR, Inc. d/b/a LevelUp">
//   Copyright(c) 2015 SCVNGR, Inc. d/b/a LevelUp. All rights reserved.
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

using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Responses
{
    /// <summary>
    /// Class representing a LevelUp access token response
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class AccessToken
    {
        public AccessToken()
        {
            TokenContainer = new AccessTokenContainer();
        }

        /// <summary>
        /// The LevelUp access token. This is required for further interaction with the LevelUp API
        /// </summary>
        public virtual string Token { get { return TokenContainer.Token; } }
        
        /// <summary>
        /// Identifier for the requesting user account
        /// </summary>
        public virtual int UserId { get { return TokenContainer.UserId; } }

        /// <summary>
        /// Merchant identifier. This will have a value if the requesting account is a LevelUp merchant
        /// </summary>
        public virtual int? MerchantId { get { return TokenContainer.MerchantId; } }

        /// <summary>
        /// Returns true if the requesting account is a LevelUp merchant
        /// </summary>
        public virtual bool IsMerchant { get { return TokenContainer.IsMerchant; } }

        /// <summary>
        /// This container is used to aid in correct JSON serialization
        /// </summary>
        [JsonProperty(PropertyName = "access_token")]
        private AccessTokenContainer TokenContainer { get; set; }

        public override string ToString()
        {
            return TokenContainer.ToString();
        }
    }
}
