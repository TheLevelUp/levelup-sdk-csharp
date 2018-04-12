#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="RefundRequestBody.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using JsonEnvelopeSerializer;
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Requests
{
    /// <summary>
    /// Class representing a request for a LevelUp refund
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    [ObjectEnvelope("refund")]
    [JsonConverter(typeof(EnvelopeSerializer))]
    public class RefundRequestBody
    {
        private RefundRequestBody()
        {
            // Private constructor for deserialization
        }

        /// <summary>
        /// Constructor for creating a LevelUp refund request
        /// </summary>
        /// <param name="managerConfirmation">A manager confirmation string if required by the POS system. 
        /// Default is null</param>
        public RefundRequestBody(string managerConfirmation = null)
        {
            ManagerConfirmation = managerConfirmation;
        }

        [JsonProperty(PropertyName = "manager_confirmation")]
        public string ManagerConfirmation { get; private set; }
    }
}
