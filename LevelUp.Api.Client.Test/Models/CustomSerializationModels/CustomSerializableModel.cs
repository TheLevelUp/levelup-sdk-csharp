#region Copyright (Apache 2.0)
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// <copyright file="CustomSerializableModel.cs" company="SCVNGR, Inc. d/b/a LevelUp">
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

using LevelUp.Api.Client.Models;
using LevelUp.Api.Client.Models.Responses;
using Newtonsoft.Json;

namespace LevelUp.Api.Client.Test.Models.CustomSerializationModels
{
    /// <summary>
    /// Example class that specifies the custom LevelUpModelSerializer for 
    /// json serialization/deserialization.
    /// </summary>
    [JsonObject]
    [LevelUpSerializableModel("custom_serializable_model")]
    [JsonConverter(typeof(LevelUpModelSerializer))]
    public class CustomSerializableModel : IResponse
    {
        [JsonProperty(PropertyName = "property_a")]
        public string PropertyA { get; internal set; }
        
        [JsonProperty(PropertyName = "property_b")]
        public int PropertyB { get; internal set; }

        [JsonProperty(PropertyName = "property_c")]
        public int? PropertyC { get; internal set; }

        public int PropertyUnNamed { get; internal set; }

        [JsonIgnore]
        public int PropertyIgnored { get; internal set; }
    }
}
