//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="AccessTokenContainer.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using System;
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Responses
{
    [JsonObject]
    internal class AccessTokenContainer
    {
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }

        [JsonProperty(PropertyName = "user_id")]
        public int UserId { get; set; }

        [JsonProperty(PropertyName = "merchant_id")]
        public int? MerchantId { get; set; }

        [JsonIgnore]
        public bool IsMerchant { get { return MerchantId.HasValue && MerchantId.Value > 0; } }

        public override string ToString()
        {
            string output = string.Format("Access Token: {0}{1}" +
                                          "User Id: {2}{1}",
                                          Token,
                                          Environment.NewLine,
                                          UserId);
            if (IsMerchant)
            {
                output += string.Format("Merchant Id: {0}{1}", MerchantId.Value, Environment.NewLine);
            }

            return output;
        }
    }
}
