#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="OrderResponse.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
    /// Class representing a response to an order request made to LevelUp
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [LevelUpSerializableModel("order")]
    [JsonConverter(typeof(LevelUpModelSerializer))]
    public class OrderResponse : IResponse
    {
        /// <summary>
        /// Private constructor for deserialization
        /// </summary>
        private OrderResponse() { }

        /// <summary>
        /// Internal constructor for testing
        /// </summary>
        internal OrderResponse(int spendAmount, int tipAmount, int total, string orderIdentifier)
        {
            SpendAmount = spendAmount;
            TipAmount = tipAmount;
            Total = total;
            OrderIdentifier = orderIdentifier;
        }

        /// <summary>
        /// Amount in cents spent on purchase not including tip. 
        /// </summary>
        [JsonProperty(PropertyName = "spend_amount")]
        public int SpendAmount { get; private set; }

        /// <summary>
        /// Amount in cents spent as tip/gratuity
        /// </summary>
        [JsonProperty(PropertyName = "tip_amount")]
        public int TipAmount { get; private set; }

        /// <summary>
        /// Total amount in cents spent on purchase plus tip/gratuity
        /// </summary>
        [JsonProperty(PropertyName = "total_amount")]
        public int Total { get; private set; }

        /// <summary>
        /// ProposedOrderIdentifier that uniquely identifies the order
        /// </summary>
        [JsonProperty(PropertyName = "uuid")]
        public string OrderIdentifier { get; private set; }

        public override string ToString()
        {
            return string.Format("Id: {0}{1}" +
                                 "Spent: {2}¢{1}" +
                                 "Tip: {3}¢{1}" +
                                 "Total: {4}¢{1}",
                                 OrderIdentifier,
                                 Environment.NewLine,
                                 SpendAmount,
                                 TipAmount,
                                 Total);
        }

    }
}
