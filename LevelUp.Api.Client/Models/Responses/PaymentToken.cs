#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="PaymentToken.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Responses
{
    /// <summary>
    /// LevelUp Payment Token
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [LevelUpSerializableModel("payment_token")]
    [JsonConverter(typeof(LevelUpModelSerializer))]
    public class PaymentToken : IResponse
    {
        /// <summary>
        /// Private constructor for deserialization
        /// </summary>
        private PaymentToken() {}

        /// <summary>
        /// Internal constructor for testing
        /// </summary>
        internal PaymentToken(int id, string data)
        {
            Id = id;
            Data = data;
        }
        
        /// <summary>
        /// The id of the token
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public int Id { get; private set; }

        /// <summary>
        /// The user's current payment token
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public string Data { get; private set; }

        public override string ToString()
        {
            return string.Format("Id: {0}{1}Data: {2}{1}", Id, Environment.NewLine, Data);
        }
    }
}
