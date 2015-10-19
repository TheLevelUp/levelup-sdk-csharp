//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="UpdateUserRequestContainer.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using System.Collections.Generic;
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Models.Requests
{
    [JsonObject]
    internal class UpdateUserRequestContainer
    {
        [JsonProperty(PropertyName = "born_at", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(SettableJsonConverter))]
        public Settable<string> BornAt { get; set; }

        [JsonProperty(PropertyName = "custom_attributes", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(SettableJsonConverter))]
        public Settable<Dictionary<string, string>> CustomAttributes { get; set; }

        [JsonProperty(PropertyName = "email", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(SettableJsonConverter))]
        public Settable<string> Email { get; set; }

        [JsonProperty(PropertyName = "first_name", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(SettableJsonConverter))]
        public Settable<string> FirstName { get; set; }

        [JsonProperty(PropertyName = "gender", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(SettableJsonConverter))]
        public Settable<string> Gender { get; set; }

        [JsonProperty(PropertyName = "last_name", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(SettableJsonConverter))]
        public Settable<string> LastName { get; set; }

        [JsonProperty(PropertyName = "new_password", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonConverter(typeof(SettableJsonConverter))]
        public Settable<string> NewPassword { get; set; }
    }
}
