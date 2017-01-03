#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="PasswordResetRequestBody.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Requests
{
    [JsonObject(MemberSerialization.OptIn)]
    [LevelUpSerializableModel(null)]
    [JsonConverter(typeof(LevelUpModelSerializer))]
    public class PasswordResetRequestBody
    {
        private PasswordResetRequestBody()
        {
            // Private constructor for deserialization
        }

        public PasswordResetRequestBody(string emailAddress)
        {
            Email = emailAddress;
        }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; private set; }
    }
}
