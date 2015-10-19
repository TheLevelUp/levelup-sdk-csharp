//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="OrderResponse.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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
    /// Class representing a response to an order request made to LevelUp
    /// </summary>
    [JsonObject]
    public class OrderResponse
    {
        /// <summary>
        /// Amount in cents spent on purchase not including tip. 
        /// </summary>
        [JsonIgnore]
        public virtual int SpendAmount { get { return OrderResponseContainer.SpendAmount; } }

        /// <summary>
        /// Amount in cents spent as tip/gratuity
        /// </summary>
        [JsonIgnore]
        public virtual int TipAmount { get { return OrderResponseContainer.TipAmount; } }

        /// <summary>
        /// Total amount in cents spent on purchase plus tip/gratuity
        /// </summary>
        [JsonIgnore]
        public virtual int Total { get { return OrderResponseContainer.Total; } }

        /// <summary>
        /// UUID that uniquely identifies the order
        /// </summary>
        [JsonIgnore]
        public virtual string Identifier { get { return OrderResponseContainer.Identifier; } }

        /// <summary>
        /// This container is used to aid in correct JSON serialization
        /// </summary>
        [JsonProperty(PropertyName = "order")]
        private OrderContainerBase OrderResponseContainer { get; set; }

        public override string ToString()
        {
            return OrderResponseContainer.ToString();
        }
    }
}
