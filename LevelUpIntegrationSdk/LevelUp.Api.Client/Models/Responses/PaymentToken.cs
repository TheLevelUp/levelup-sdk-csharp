//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="PaymentToken.cs" company="SCVNGR, Inc. d/b/a LevelUp">
//   Copyright(c) 2014 SCVNGR, Inc. d/b/a LevelUp. All rights reserved.
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
    /// LevelUp Payment Token
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class PaymentToken
    {
        public PaymentToken()
        {
            PaymentTokenContainer = new PaymentTokenContainer();
        }

        /// <summary>
        /// The id of the token
        /// </summary>
        public int Id { get { return PaymentTokenContainer.Id; } }

        /// <summary>
        /// The user's current payment token
        /// </summary>
        public string Data { get { return PaymentTokenContainer.Data; } }

        [JsonProperty(PropertyName = "payment_token")]
        private PaymentTokenContainer PaymentTokenContainer { get; set; }

        public override string ToString()
        {
            return PaymentTokenContainer.ToString();
        }
    }
}
